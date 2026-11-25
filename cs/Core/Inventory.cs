using System;
using System.Collections.Generic;

class Inventory
{
    public Action<List<Product>> OnOrdered;
    FloorNavigator Navigation = new FloorNavigator();
    List<Product> Shopping;
    Dictionary<Product, int> SalesCapacity = new Dictionary<Product, int>();
    Dictionary<Product, int> ReserveCapacity = new Dictionary<Product, int>();
    Dictionary<Product, int> Available = new Dictionary<Product, int>();
    Dictionary<Product, int> Planned = new Dictionary<Product, int>();
    List<Product> Active = new List<Product>();
    int MoneyLeft;
    int TruckCapacity;
    Product[] Products = new Product[]
    {
        Product.Tomato, Product.Pumpkin, Product.Bread, Product.Soda,
        Product.Beer, Product.Cigarettes, Product.Wine, Product.Milk,
        Product.Tulips, Product.Honey, Product.Watermelon, Product.Tangerine,
        Product.Strawberry, Product.Eastercake, Product.Fireworks, Product.Tree,
        Product.Steak, Product.Truffle, Product.Caviar, Product.Lobster,
        Product.Chocolate, Product.Popcorn, Product.Coffee, Product.Icecream,
        Product.Jam, Product.Fertilizer, Product.Mask, Product.Whisky, Product.Cognac
    };

    public Inventory() { Shopping = robot.GetList(); }

    public void Run()
    {
        Shopping.Clear();
        while (true)
        {
            while (!Navigation.MoveAside()) Navigation.WaitTurn();
            OrderDelivery();
            Clock.WaitSeconds(5);
        }
    }

    public bool OrderDelivery()
    {
        if (!store.IsTruckAway()) return false;
        BuildOrder();
        if (Shopping.Count == 0 || !store.CallDelivery(Shopping)) return false;
        if (OnOrdered != null) OnOrdered(new List<Product>(Shopping));
        return true;
    }

    public void BuildOrder()
    {
        Shopping.Clear();
        SalesCapacity.Clear();
        ReserveCapacity.Clear();
        Available.Clear();
        Planned.Clear();
        Active.Clear();
        MoneyLeft = store.GetMoney();
        TruckCapacity = store.GetTruckCapacity();

        foreach (Product product in Products)
        {
            // Finite vending products also need stock. Infinite coffee does not.
            if (product.IsInfinite() || !product.IsInSeason()) continue;
            Entity rack = product.GetRack();
            if (!FloorMap.ShelfCapacities.ContainsKey(rack)) continue;
            int capacity = FloorMap.ShelfCapacities[rack];
            if (capacity <= 0) continue;
            int sales = 0;
            int reserve = 0;
            int stock = 0;
            foreach (var building in FloorMap.Buildings)
            {
                if (building.Key.Entity != rack || building.Value.Count == 0) continue;
                // Count each fixture once, including racks occupying two cells.
                if (building.Value[0].Type == InteractableType.Overstock) reserve += capacity;
                else sales += capacity;
                stock += store.GetStock(building.Key.X, building.Key.Y);
            }
            if (sales + reserve == 0) continue;
            Active.Add(product);
            SalesCapacity[product] = sales;
            ReserveCapacity[product] = reserve;
            Available[product] = stock + store.GetRemaining(product);
            Planned[product] = 0;
        }

        // Existing reserve stock and incoming goods can supply SalesFloor first.
        // Finish this tier for every product before buying reserve refills.
        foreach (Product product in Active)
            Add(product, SalesCapacity[product] - Available[product]);
        foreach (Product product in Active)
            Add(product, SalesCapacity[product] + ReserveCapacity[product]
                - Available[product] - Planned[product]);
    }

    void Add(Product product, int needed)
    {
        if (needed <= 0) return;
        int cost = store.GetBuyPrice(product);
        if (cost <= 0) return;
        while (needed > 0 && Shopping.Count < TruckCapacity && MoneyLeft >= cost)
        {
            Shopping.Add(product);
            Planned[product]++;
            MoneyLeft -= cost;
            needed--;
        }
    }
}
