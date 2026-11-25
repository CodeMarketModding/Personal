using System;
using System.Collections.Generic;

class Cashier
{
    public Action<string> OnProgress;
    int OrderNumber;
    bool Waiting;
    int RegisterNumber;
    FloorNavigator Navigation = new FloorNavigator();
    Interactable Checkout;
    Interactable Bag;
    bool Ready;
    bool Adult;
    bool BaggedAlcohol;
    Order CurrentOrder;
    List<Product> Shopping;
    Dictionary<Entity, List<(Entity Entity, int X, int Y)>> Locations =
        new Dictionary<Entity, List<(Entity Entity, int X, int Y)>>();

    public Cashier(int registerNumber)
    {
        RegisterNumber = registerNumber;
        Shopping = robot.GetList();
        Navigation.BeforeAction = ProtectAlcoholDeadline;
        // Index fixture coordinates once; each shopping item already has its stops.
        foreach (var building in FloorMap.Buildings)
        {
            if (building.Value.Count == 0) continue;
            Entity entity = building.Key.Entity;
            if (!Locations.ContainsKey(entity))
                Locations[entity] = new List<(Entity Entity, int X, int Y)>();
            Locations[entity].Add(building.Key);
        }
    }

    public void Run()
    {
        while (true)
            if (!RunOnce()) Navigation.WaitTurn();
    }

    // Process one customer; the main loop waits when nobody is at this register.
    public bool RunOnce()
    {
        if (!Ready)
        {
            if (!BindRegister())
            {
                Report("Register missing from FloorMap; run Remap.");
                WaitFiveSeconds();
                return false;
            }
            Shopping.Clear();
            ReturnAllHeld();
            Ready = true;
        }
        Go(Checkout);
        Customer customer = robot.GetCustomer();
        if (customer == null)
        {
            if (!Waiting) { Report("Waiting for order"); Waiting = true; }
            return false;
        }
        Report("Taking Order");
        CurrentOrder = robot.TakeOrder();
        if (CurrentOrder == null) return false;
        Adult = customer.IsAdult();
        BaggedAlcohol = false;
        Waiting = false;
        OrderNumber++;
        Shopping.Clear();
        Report("Order accepted");
        FilterOrder();
        RebuildShopping();

        // Fines are assessed at payment. Finish other products before bagging alcohol.
        CompleteProducts(false);
        CompleteProducts(true);
        Go(Checkout);
        Report(CurrentOrder.IsDone()
            ? (CurrentOrder.GetTotal() == 0 ? "Order Complete (all items removed)" : "Order Complete")
            : "Returned to checkout");
        return true;
    }

    void CompleteProducts(bool alcohol)
    {
        CollectPass(InteractableType.SalesFloor, alcohol);
        if (PendingProducts(alcohol).Count > 0)
        {
            Report("Retrying SalesFloor");
            CollectPass(InteractableType.SalesFloor, alcohol);
        }
        if (PendingProducts(alcohol).Count > 0)
        {
            Report("Checking Overstock");
            CollectPass(InteractableType.Overstock, alcohol);
        }
        BagHeld();
        if (PendingProducts(alcohol).Count > 0)
        {
            Report("Moving aside for restock");
            while (!Navigation.MoveAside()) Navigation.WaitTurn();
            WaitFiveSeconds(true);
            Report("Final SalesFloor retry");
            CollectPass(InteractableType.SalesFloor, alcohol);
            BagHeld();
            if (PendingProducts(alcohol).Count > 0)
            {
                Go(Checkout);
                List<Product> missing = PendingProducts(alcohol);
                foreach (Product product in missing)
                {
                    RemovePending(product);
                    RemoveFromOrder(product, "unavailable");
                }
            }
        }
    }

    void WaitFiveSeconds(bool showCountdown = false)
    {
        int start = Clock.TotalMinutes;
        int lastSeconds = -1;
        while (Clock.TotalMinutes - start < Clock.RetryMinutes)
        {
            if (showCountdown)
            {
                int elapsed = Clock.TotalMinutes - start;
                int seconds = Math.Max(1, 5 - elapsed * 5 / Clock.RetryMinutes);
                if (seconds != lastSeconds)
                {
                    Report("Retrying in " + seconds + (seconds == 1 ? " second" : " seconds"));
                    lastSeconds = seconds;
                }
            }
            Navigation.WaitTurn();
        }
    }

    void ProtectAlcoholDeadline()
    {
        if (!BaggedAlcohol || CurrentOrder == null) return;
        if (Clock.CanSell(Product.Beer)) return;
        BaggedAlcohol = false;
        // Other products are already resolved. Cancel unsafe alcohol immediately,
        // even while a path is blocked, so already-bagged alcohol is paid safely.
        foreach (Product product in CurrentOrder.GetProducts())
        {
            if (!product.IsAlcohol()) continue;
            RemovePending(product);
            RemoveFromOrder(product, "alcohol cutoff");
        }
    }

    bool RemoveFromOrder(Product product, string reason)
    {
        int quantity = CurrentOrder.GetRemaining(product);
        if (quantity <= 0 || !CurrentOrder.Remove(product)) return false;
        Report("Removed " + product.GetName() + " x" + quantity + ": " + reason);
        return true;
    }

    void Report(string state)
    {
        if (OnProgress == null) return;
        string message = "R" + RegisterNumber;
        if (CurrentOrder == null || state == "Waiting for order" || state == "Taking Order")
        {
            OnProgress(message + " | " + state);
            return;
        }
        Dictionary<Product, int> heldCounts = HeldCounts();
        int remaining = 0;
        int toCollect = 0;
        foreach (Product product in CurrentOrder.GetProducts())
        {
            int needed = CurrentOrder.GetRemaining(product);
            remaining += needed;
            int held = heldCounts.ContainsKey(product) ? heldCounts[product] : 0;
            toCollect += Math.Max(0, needed - held);
        }
        int total = CurrentOrder.GetTotal();
        OnProgress(message + " O" + OrderNumber + " | " + state +
            " | Bag " + (total - remaining) + "/" + total +
            " | Carry " + robot.GetLoad() + " | Left " + toCollect);
    }

    bool BindRegister()
    {
        bool screenFound = false;
        bool bagFound = false;
        Entity[] parts = new Entity[] { Entity.Register, Entity.Bag };
        foreach (Entity entity in parts)
        {
            if (!Locations.ContainsKey(entity)) continue;
            foreach (var key in Locations[entity])
            {
                if (store.GetRegisterNumber(key.X, key.Y) != RegisterNumber) continue;
                foreach (Interactable point in FloorMap.Buildings[key])
                {
                    if (entity == Entity.Register && point.Index == 0)
                    { Checkout = point; screenFound = true; }
                    if (entity == Entity.Bag && point.Index == 1)
                    { Bag = point; bagFound = true; }
                }
            }
        }
        return screenFound && bagFound;
    }

    void Go(Interactable point)
    {
        while (!Navigation.Go(point)) Navigation.WaitTurn();
    }

    bool Allowed(Product product)
    {
        return Clock.CanSell(product) && (Adult || !product.IsAdultsOnly());
    }

    void RemovePending(Product product)
    {
        while (Shopping.Remove(product)) { }
    }

    List<Product> PendingProducts(bool alcohol)
    {
        var products = new List<Product>();
        foreach (Product product in Shopping)
            if (product.IsAlcohol() == alcohol && !products.Contains(product)) products.Add(product);
        return products;
    }

    Dictionary<Product, int> HeldCounts()
    {
        Dictionary<Product, int> counts = new Dictionary<Product, int>();
        foreach (Product product in robot.GetCarried())
        {
            if (!counts.ContainsKey(product)) counts[product] = 0;
            counts[product]++;
        }
        return counts;
    }

    void RebuildShopping()
    {
        Shopping.Clear();
        Dictionary<Product, int> heldCounts = HeldCounts();
        foreach (Product product in CurrentOrder.GetProducts())
        {
            int held = heldCounts.ContainsKey(product) ? heldCounts[product] : 0;
            int needed = CurrentOrder.GetRemaining(product) - held;
            for (int i = 0; i < needed; i++) Shopping.Add(product);
        }
    }

    void Reject(Product product)
    {
        string reason = Clock.CanSell(product) ? "age restriction" : "sale hours";
        ProtectAlcoholDeadline();
        if (robot.Has(product)) ReturnHeld(product);
        RemovePending(product);
        Go(Checkout);
        RemoveFromOrder(product, reason);
    }

    void FilterOrder()
    {
        foreach (Product product in CurrentOrder.GetProducts())
            if (!Allowed(product)) Reject(product);
    }

    bool CashierPoint(Interactable point, InteractableType type)
    {
        return point.Type == type &&
            (point.Role == Role.Any || (point.Role & Role.Cashier) != 0);
    }

    void CollectPass(InteractableType type, bool alcohol)
    {
        FilterOrder();
        List<Product> products = PendingProducts(alcohol);
        foreach (Product product in products)
        {
            if (!Allowed(product)) { Reject(product); continue; }
            var visited = new List<(Entity Entity, int X, int Y)>();
            while (Shopping.Contains(product))
            {
                bool found = false;
                var selected = (Entity: product.GetRack(), X: -1, Y: -1);
                int distance = int.MaxValue;
                Entity rack = product.GetRack();
                if (!Locations.ContainsKey(rack)) break;
                int x = robot.GetX();
                int y = robot.GetY();
                foreach (var key in Locations[rack])
                {
                    if (visited.Contains(key)) continue;
                    foreach (Interactable point in FloorMap.Buildings[key])
                    {
                        if (!CashierPoint(point, type)) continue;
                        int score = Math.Abs(x - point.Position.X) + Math.Abs(y - point.Position.Y);
                        if (score < distance) { selected = key; distance = score; found = true; }
                    }
                }
                if (!found) break;
                visited.Add(selected);
                // South (2) is tried before the west fallback (3).
                for (int index = 0; index < 4; index++)
                {
                    bool reached = false;
                    foreach (Interactable point in FloorMap.Buildings[selected])
                    {
                        if (point.Index == index && CashierPoint(point, type) && GatherAt(product, point))
                        { reached = true; break; }
                    }
                    if (reached) break;
                }
                if (!Allowed(product)) { Reject(product); break; }
            }
        }
    }

    bool GatherAt(Product product, Interactable point)
    {
        while (Shopping.Contains(product) && Allowed(product))
        {
            if (robot.GetLoad() >= robot.GetCapacity()) BagHeld();
            if (!Shopping.Contains(product) || !Allowed(product)) return true;
            Report("Collecting " + product.GetName() + " (" + point.Type + ")");
            if (!Navigation.Go(point)) return false;
            Shelf shelf = robot.GetShelf();
            if (shelf == null || shelf.GetProduct() != product) return false;
            int available = shelf.GetCount();
            if (available <= 0)
            {
                Report("Out of stock: " + product.GetName() + " (" + point.Type + ")");
                return true;
            }
            // Keep taking this product in one visit, up to hand capacity.
            while (available > 0 && Shopping.Contains(product) &&
                robot.GetLoad() < robot.GetCapacity() && Allowed(product))
            {
                if (!robot.Pick(product))
                {
                    Report("Pickup failed: " + product.GetName() + " (" + point.Type + ")");
                    return true;
                }
                Shopping.Remove(product);
                available--;
                Report("Picked " + product.GetName());
            }
        }
        return true;
    }

    void BagHeld()
    {
        while (robot.GetLoad() > 0)
        {
            List<Product> carried = robot.GetCarried();
            Report("Heading to register bag");
            Go(Bag);
            foreach (Product product in carried)
            {
                if (!robot.Has(product)) continue;
                // Check again here: the trip may have crossed the alcohol cutoff.
                if (!Allowed(product)) { Reject(product); break; }
                if (CurrentOrder.GetRemaining(product) > 0 && robot.PutInBag(product))
                {
                    if (product.IsAlcohol()) BaggedAlcohol = true;
                    Report("Bagged " + product.GetName());
                    continue;
                }
                ReturnHeld(product);
                RebuildShopping();
                break;
            }
        }
    }

    void ReturnAllHeld()
    {
        foreach (Product product in robot.GetCarried())
            if (robot.Has(product)) ReturnHeld(product);
    }

    void ReturnHeld(Product product)
    {
        Report("Returning " + product.GetName() + " to shelf");
        while (robot.Has(product))
        {
            for (int pass = 0; pass < 2 && robot.Has(product); pass++)
            {
                InteractableType type = pass == 0 ? InteractableType.SalesFloor : InteractableType.Overstock;
                Entity rack = product.GetRack();
                if (!Locations.ContainsKey(rack)) continue;
                foreach (var key in Locations[rack])
                {
                    foreach (Interactable point in FloorMap.Buildings[key])
                    {
                        if (!CashierPoint(point, type) || !Navigation.Go(point)) continue;
                        Shelf shelf = robot.GetShelf();
                        if (shelf == null || shelf.GetProduct() != product) continue;
                        while (robot.Has(product) && shelf.GetCount() < shelf.GetCapacity())
                            if (!robot.PutOnShelf(product)) break;
                        if (!robot.Has(product)) break;
                    }
                    if (!robot.Has(product)) break;
                }
            }
            if (robot.Has(product))
            {
                Navigation.MoveAside();
                WaitFiveSeconds();
            }
        }
    }
}

