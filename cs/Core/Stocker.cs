using System;
using System.Collections.Generic;

class Stocker
{
    public Action<string> OnProgress;
    FloorNavigator Navigation = new FloorNavigator();
    Dictionary<Entity, int> Capacities = new Dictionary<Entity, int>();
    string LastStatus = "";
    List<Product> Pending;

    Dictionary<Entity, List<Interactable>> Sales = new Dictionary<Entity, List<Interactable>>();
    Dictionary<Entity, List<Interactable>> Reserve = new Dictionary<Entity, List<Interactable>>();
    List<Interactable> NoPoints = new List<Interactable>();
    List<Product> Priority;
    int PriorityDay = -1;

    public Stocker()
    {
        Pending = robot.GetList();
        // Build a direct destination index once; workers never rescan the store.
        foreach (var building in FloorMap.Buildings)
            foreach (Interactable point in building.Value)
            {
                if (point.Role != Role.Any && (point.Role & Role.Stocker) == 0) continue;
                var index = point.Type == InteractableType.SalesFloor ? Sales : Reserve;
                if (!index.ContainsKey(building.Key.Entity))
                    index[building.Key.Entity] = new List<Interactable>();
                index[building.Key.Entity].Add(point);
            }
    }

    List<Interactable> Points(Product product, InteractableType type)
    {
        var index = type == InteractableType.SalesFloor ? Sales : Reserve;
        Entity rack = product.GetRack();
        return index.ContainsKey(rack) ? index[rack] : NoPoints;
    }

    Vector2d Front(Interactable point)
    {
        int x = point.Position.X;
        int y = point.Position.Y;
        if (point.Facing == Direction.Up) y++;
        else if (point.Facing == Direction.Down) y--;
        else if (point.Facing == Direction.Right) x++;
        else x--;
        return new Vector2d(x, y);
    }

    Product[] First = new Product[]
    {
        Product.Cognac, Product.Caviar, Product.Lobster,
        Product.Whisky, Product.Steak, Product.Truffle,
        Product.Chocolate, Product.Popcorn, Product.Coffee
    };
    Product[] Seasonal = new Product[]
    {
        Product.Tulips, Product.Fertilizer, Product.Eastercake,
        Product.Mask, Product.Jam, Product.Honey,
        Product.Tree, Product.Fireworks, Product.Tangerine,
        Product.Icecream, Product.Watermelon, Product.Strawberry
    };
    Product[] Basics = new Product[]
    {
        Product.Bread, Product.Soda, Product.Milk, Product.Tomato,
        Product.Pumpkin, Product.Beer, Product.Cigarettes, Product.Wine
    };

    public List<Product> GetPriorities()
    {
        int day = Clock.Day;
        if (Priority != null && PriorityDay == day) return Priority;
        List<Product> result = new List<Product>();
        foreach (Product product in First)
            if (!product.IsInfinite()) result.Add(product);
        foreach (Product product in Seasonal)
            if (product.IsInSeason()) result.Add(product);
        foreach (Product product in Seasonal)
            if (!product.IsInSeason()) result.Add(product);
        foreach (Product product in Basics) result.Add(product);
        Priority = result;
        PriorityDay = day;
        return result;
    }

    public void Run()
    {
        Report("Stocker ready");
        while (true)
        {
            if (RunOnce()) continue;
            // Empty/free hands wait at the garage, not in a map-scanning parking loop.
            if (robot.GetLoad() < robot.GetCapacity()
                && (Pending.Count == 0 || robot.IsAtTruck() || store.IsTruckHere()))
            {
                Navigation.WaitTurn();
                continue;
            }
            Park();
            WaitForRetry();
        }
    }

    // One finite cycle, also used for isolated validation without running a robot.
    public bool RunOnce()
    {
        // Grab first. No product, destination, stock, season, or pending-list checks.
        bool loaded = false;
        if (robot.GetLoad() < robot.GetCapacity())
            loaded = LoadTruck();

        int before = robot.GetLoad();
        if (before > 0)
        {
            List<Product> priorities = GetPriorities();
            StockCarried(priorities);
            if (robot.GetLoad() > 0)
            {
                Report("Retrying skipped items");
                StockCarried(priorities);
            }
        }
        bool placed = robot.GetLoad() < before;
        SyncPending();
        if (Pending.Count > 0)
        {
            if (robot.GetLoad() < robot.GetCapacity() && (loaded || placed))
            {
                Report("Keeping " + Pending.Count + " held items; returning to truck");
                return true;
            }
            Report("Deferred " + Pending.Count + " held items; retrying shortly");
            return false;
        }
        if (loaded || placed) return true;
        Report(store.IsTruckHere() ? "Waiting for truck access" : "Waiting for delivery");
        return false;
    }

    Dictionary<Product, int> Counts(List<Product> items)
    {
        Dictionary<Product, int> result = new Dictionary<Product, int>();
        foreach (Product product in items)
        {
            if (!result.ContainsKey(product)) result[product] = 0;
            result[product]++;
        }
        return result;
    }

    void SyncPending()
    {
        List<Product> carried = robot.GetCarried();
        Dictionary<Product, int> remaining = Counts(carried);
        foreach (Product product in new List<Product>(Pending))
        {
            if (!remaining.ContainsKey(product) || remaining[product] <= 0)
                Pending.Remove(product);
            else remaining[product]--;
        }
        Dictionary<Product, int> tracked = Counts(Pending);
        foreach (var item in Counts(carried))
        {
            int count = tracked.ContainsKey(item.Key) ? tracked[item.Key] : 0;
            for (int i = count; i < item.Value; i++) Pending.Add(item.Key);
        }
    }

    int Capacity(Entity rack)
    {
        if (Capacities.ContainsKey(rack)) return Capacities[rack];
        if (FloorMap.ShelfCapacities.ContainsKey(rack)) return FloorMap.ShelfCapacities[rack];
        return 0;
    }

    int Space(Product product, InteractableType type)
    {
        Entity rack = product.GetRack();
        int capacity = Capacity(rack);
        int result = 0;
        foreach (Interactable point in Points(product, type))
        {
            Vector2d front = Front(point);
            if (store.GetEntity(front.X, front.Y) == rack)
                result += Math.Max(0, capacity - store.GetStock(front.X, front.Y));
        }
        return result;
    }

    bool LoadTruck()
    {
        if (robot.GetLoad() >= robot.GetCapacity()) return false;
        Report("Returning to delivery");
        if (!GoToTruck())
        {
            Report(store.IsTruckHere() ? "At garage; waiting for truck to open" : "At garage; waiting for delivery");
            return false;
        }

        int before = robot.GetLoad();
        List<Product> delivery = robot.GetDelivery();
        Dictionary<Product, int> picked = new Dictionary<Product, int>();
        foreach (Product product in delivery)
        {
            if (robot.GetLoad() >= robot.GetCapacity() || !robot.IsAtTruck()) break;
            if (!robot.Pick(product)) continue;
            if (!picked.ContainsKey(product)) picked[product] = 0;
            picked[product]++;
        }
        foreach (var item in picked)
            Report("Loaded " + item.Key.GetName() + " x" + item.Value);
        return robot.GetLoad() > before;
    }

    bool GoToTruck()
    {
        if (robot.IsAtTruck()) return true;
        int x = store.GetGarageX();
        int y = store.GetGarageY();
        Direction facing = store.GetGarageSide();
        int[] offsets = new int[] { 0, 1, -1 };
        foreach (int offset in offsets)
        {
            int px = x;
            int py = y;
            if (facing == Direction.Up || facing == Direction.Down) px += offset;
            else py += offset;
            if (px < 0 || py < 0 || px >= FloorMap.Width || py >= FloorMap.Height) continue;
            if (!store.IsFree(px, py)) continue;
            Interactable gate = new Interactable(new Vector2d(px, py), facing, Role.Stocker);
            if (Navigation.Go(gate)) return robot.IsAtTruck();
        }
        return false;
    }

    void StockCarried(List<Product> priorities)
    {
        // A single fast pass. RunOnce performs the one end-of-cycle retry.
        foreach (Product product in priorities)
        {
            if (robot.Has(product)) Fill(product, InteractableType.SalesFloor);
        }
        foreach (Product product in priorities)
        {
            if (!robot.Has(product)) continue;
            // Full SalesFloor goes to Overstock. A blocked shelf is retried later.
            if (Space(product, InteractableType.SalesFloor) > 0) continue;
            Fill(product, InteractableType.Overstock);
        }
        if (robot.GetLoad() == 0) Report("Batch Complete");
    }

    void Fill(Product product, InteractableType type)
    {
        Entity rack = product.GetRack();
        foreach (Interactable point in Points(product, type))
        {
            if (!robot.Has(product)) break;
            Vector2d front = Front(point);
            if (store.GetEntity(front.X, front.Y) != rack) continue;
            int capacity = Capacity(rack);
            if (capacity > 0 && store.GetStock(front.X, front.Y) >= capacity) continue;
            Report("Restocking " + product.GetName() + " (" + type + ")");
            if (!Navigation.Go(point))
            {
                Report("Path blocked: " + product.GetName() + " (" + type + ")");
                continue;
            }
            Shelf shelf = robot.GetShelf();
            if (!robot.IsAtPickup() || shelf == null || shelf.GetProduct() != product) continue;
            Capacities[rack] = shelf.GetCapacity();
            int placed = 0;
            while (robot.Has(product) && !shelf.IsFull())
            {
                if (!robot.IsAtPickup() || !robot.PutOnShelf(product)) break;
                Pending.Remove(product);
                placed++;
            }
            if (placed > 0) Report("Stocked " + product.GetName() + " x" + placed + " (" + type + ")");
        }
    }

    void Park()
    {
        int gx = store.GetGarageX();
        int gy = store.GetGarageY();
        Direction side = store.GetGarageSide();
        int ax = side == Direction.Up || side == Direction.Down ? 1 : 0;
        int ay = 1 - ax;
        int ix = side == Direction.Right ? -1 : side == Direction.Left ? 1 : 0;
        int iy = side == Direction.Up ? -1 : side == Direction.Down ? 1 : 0;
        Vector2d[] rest = new Vector2d[]
        {
            new Vector2d(gx + 4 * ax, gy + 4 * ay),
            new Vector2d(gx - 4 * ax, gy - 4 * ay),
            new Vector2d(gx + 3 * ax + ix, gy + 3 * ay + iy),
            new Vector2d(gx - 3 * ax + ix, gy - 3 * ay + iy),
            new Vector2d(gx + 4 * ix, gy + 4 * iy),
            new Vector2d(gx + 5 * ix, gy + 5 * iy)
        };
        foreach (Vector2d point in rest)
        {
            if (point.X < 0 || point.Y < 0 || point.X >= PathMap.Width || point.Y >= PathMap.Height) continue;
            int cell = point.Y * PathMap.Width + point.X;
            if (PathMap.Reserved[cell] == '1' || !store.IsFree(point.X, point.Y)) continue;
            if (Navigation.GoTo(point)) return;
        }
    }

    void WaitForRetry()
    {
        bool truckHere = store.IsTruckHere();
        int started = Clock.TotalMinutes;
        int last = -1;
        while (Clock.TotalMinutes - started < Clock.RetryMinutes)
        {
            // A newly docked truck wakes the robot before the normal retry timer.
            if (!truckHere && store.IsTruckHere()) return;
            int seconds = Math.Max(1, 5 - (Clock.TotalMinutes - started) * 5 / Clock.RetryMinutes);
            if (seconds != last)
            {
                Report("Retrying in " + seconds + (seconds == 1 ? " second" : " seconds"));
                last = seconds;
            }
            Navigation.WaitTurn();
        }
    }

    void Report(string status)
    {
        string message = "Stocker | " + status + " | Carry " + robot.GetLoad() + "/" + robot.GetCapacity();
        if (message == LastStatus) return;
        LastStatus = message;
        if (OnProgress != null) OnProgress(message);
    }
}

