using System.Collections.Generic;

static class FloorMap
{
    public const int World = 10;
    public const int Width = 29;
    public const int Height = 19;

    // Keys identify each entity by its type and anchor cell (X east, Y north).
    // Four-point indices: North 0, East 1, South 2, West 3. Cashier tries South before West.
    // Shelf double-point indices follow the building facing. Lowest Y is Cashier; ties use index 0.
    // Registers use native POS and Bag fixture cells; both robot workpoints belong to Cashier.
    // Lowercase map labels are Overstock; other interactions default to SalesFloor.
    public static Dictionary<(Entity Entity, int X, int Y), List<Interactable>> Buildings =
        new Dictionary<(Entity Entity, int X, int Y), List<Interactable>>
    {
        // Aquarium at (0, 0), building faces Down.
        { (Entity.Aquarium, 0, 0), new List<Interactable>() },
        // Aquarium at (0, 1), building faces Down.
        { (Entity.Aquarium, 0, 1), new List<Interactable>() },
        // Aquarium at (0, 2), building faces Down.
        { (Entity.Aquarium, 0, 2), new List<Interactable>() },
        // Aquarium at (0, 3), building faces Down.
        { (Entity.Aquarium, 0, 3), new List<Interactable>() },
        // Aquarium at (0, 4), building faces Down.
        { (Entity.Aquarium, 0, 4), new List<Interactable>() },
        // Aquarium at (0, 5), building faces Down.
        { (Entity.Aquarium, 0, 5), new List<Interactable>() },
        // Aquarium at (14, 0), building faces Up.
        { (Entity.Aquarium, 14, 0), new List<Interactable>() },
        // Aquarium at (16, 0), building faces Up.
        { (Entity.Aquarium, 16, 0), new List<Interactable>() },
        // Aquarium at (16, 1), building faces Down.
        { (Entity.Aquarium, 16, 1), new List<Interactable>() },
        // Aquarium at (18, 0), building faces Up.
        { (Entity.Aquarium, 18, 0), new List<Interactable>() },
        // Aquarium at (18, 1), building faces Down.
        { (Entity.Aquarium, 18, 1), new List<Interactable>() },
        // Aquarium at (20, 0), building faces Up.
        { (Entity.Aquarium, 20, 0), new List<Interactable>() },
        // Aquarium at (20, 1), building faces Down.
        { (Entity.Aquarium, 20, 1), new List<Interactable>() },
        // Aquarium at (22, 0), building faces Down.
        { (Entity.Aquarium, 22, 0), new List<Interactable>() },
        // Aquarium at (22, 1), building faces Down.
        { (Entity.Aquarium, 22, 1), new List<Interactable>() },
        // Aquarium at (24, 0), building faces Up.
        { (Entity.Aquarium, 24, 0), new List<Interactable>() },
        // Aquarium at (24, 1), building faces Down.
        { (Entity.Aquarium, 24, 1), new List<Interactable>() },
        // Aquarium at (24, 5), building faces Down.
        { (Entity.Aquarium, 24, 5), new List<Interactable>() },
        // Aquarium at (24, 6), building faces Down.
        { (Entity.Aquarium, 24, 6), new List<Interactable>() },
        // Aquarium at (24, 7), building faces Down.
        { (Entity.Aquarium, 24, 7), new List<Interactable>() },
        // Aquarium at (24, 8), building faces Down.
        { (Entity.Aquarium, 24, 8), new List<Interactable>() },
        // Aquarium at (24, 9), building faces Down.
        { (Entity.Aquarium, 24, 9), new List<Interactable>() },
        // Aquarium at (24, 10), building faces Down.
        { (Entity.Aquarium, 24, 10), new List<Interactable>() },
        // Aquarium at (24, 11), building faces Down.
        { (Entity.Aquarium, 24, 11), new List<Interactable>() },
        // Aquarium at (24, 12), building faces Down.
        { (Entity.Aquarium, 24, 12), new List<Interactable>() },
        // Aquarium at (24, 13), building faces Down.
        { (Entity.Aquarium, 24, 13), new List<Interactable>() },
        // Aquarium at (24, 14), building faces Down.
        { (Entity.Aquarium, 24, 14), new List<Interactable>() },
        // Aquarium at (24, 15), building faces Down.
        { (Entity.Aquarium, 24, 15), new List<Interactable>() },
        // Aquarium at (24, 16), building faces Down.
        { (Entity.Aquarium, 24, 16), new List<Interactable>() },
        // Aquarium at (24, 17), building faces Down.
        { (Entity.Aquarium, 24, 17), new List<Interactable>() },
        // Aquarium at (25, 18), building faces Down.
        { (Entity.Aquarium, 25, 18), new List<Interactable>() },
        // Aquarium at (26, 0), building faces Down.
        { (Entity.Aquarium, 26, 0), new List<Interactable>() },
        // Aquarium at (26, 1), building faces Down.
        { (Entity.Aquarium, 26, 1), new List<Interactable>() },
        // Aquarium at (26, 2), building faces Down.
        { (Entity.Aquarium, 26, 2), new List<Interactable>() },
        // Aquarium at (26, 3), building faces Down.
        { (Entity.Aquarium, 26, 3), new List<Interactable>() },
        // Aquarium at (26, 4), building faces Down.
        { (Entity.Aquarium, 26, 4), new List<Interactable>() },
        // Aquarium at (26, 5), building faces Down.
        { (Entity.Aquarium, 26, 5), new List<Interactable>() },
        // Aquarium at (26, 6), building faces Down.
        { (Entity.Aquarium, 26, 6), new List<Interactable>() },
        // Aquarium at (26, 7), building faces Down.
        { (Entity.Aquarium, 26, 7), new List<Interactable>() },
        // Aquarium at (26, 8), building faces Down.
        { (Entity.Aquarium, 26, 8), new List<Interactable>() },
        // Aquarium at (26, 9), building faces Down.
        { (Entity.Aquarium, 26, 9), new List<Interactable>() },
        // Aquarium at (26, 10), building faces Down.
        { (Entity.Aquarium, 26, 10), new List<Interactable>() },
        // Aquarium at (26, 11), building faces Down.
        { (Entity.Aquarium, 26, 11), new List<Interactable>() },
        // Aquarium at (26, 12), building faces Down.
        { (Entity.Aquarium, 26, 12), new List<Interactable>() },
        // Aquarium at (26, 13), building faces Down.
        { (Entity.Aquarium, 26, 13), new List<Interactable>() },
        // Aquarium at (26, 14), building faces Down.
        { (Entity.Aquarium, 26, 14), new List<Interactable>() },
        // Aquarium at (26, 15), building faces Down.
        { (Entity.Aquarium, 26, 15), new List<Interactable>() },
        // Aquarium at (26, 16), building faces Down.
        { (Entity.Aquarium, 26, 16), new List<Interactable>() },
        // Aquarium at (26, 17), building faces Down.
        { (Entity.Aquarium, 26, 17), new List<Interactable>() },
        // Aquarium at (27, 18), building faces Down.
        { (Entity.Aquarium, 27, 18), new List<Interactable>() },
        // Aquarium at (28, 0), building faces Left.
        { (Entity.Aquarium, 28, 0), new List<Interactable>() },
        // Aquarium at (28, 2), building faces Left.
        { (Entity.Aquarium, 28, 2), new List<Interactable>() },
        // Aquarium at (28, 4), building faces Left.
        { (Entity.Aquarium, 28, 4), new List<Interactable>() },
        // Aquarium at (28, 6), building faces Left.
        { (Entity.Aquarium, 28, 6), new List<Interactable>() },
        // Aquarium at (28, 8), building faces Left.
        { (Entity.Aquarium, 28, 8), new List<Interactable>() },
        // Aquarium at (28, 10), building faces Left.
        { (Entity.Aquarium, 28, 10), new List<Interactable>() },
        // Aquarium at (28, 12), building faces Left.
        { (Entity.Aquarium, 28, 12), new List<Interactable>() },
        // Aquarium at (28, 14), building faces Left.
        { (Entity.Aquarium, 28, 14), new List<Interactable>() },
        // Aquarium at (28, 16), building faces Left.
        { (Entity.Aquarium, 28, 16), new List<Interactable>() },
        // BeerFridge at (2, 16), building faces Down.
        { (Entity.BeerFridge, 2, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(2, 15), Direction.Up, Role.Any, 0, InteractableType.Overstock),
            }
        },
        // BeerFridge at (15, 7), building faces Down.
        { (Entity.BeerFridge, 15, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(15, 6), Direction.Up, Role.Any, 0),
            }
        },
        // BreadShelf at (5, 7), building faces Down.
        { (Entity.BreadShelf, 5, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(5, 6), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(6, 6), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
        // BreadShelf at (20, 16), building faces Down.
        { (Entity.BreadShelf, 20, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(20, 15), Direction.Up, Role.Cashier, 0, InteractableType.Overstock),
                new Interactable(new Vector2d(21, 15), Direction.Up, Role.Stocker | Role.Inventory, 1, InteractableType.Overstock),
            }
        },
        // CaviarFridge at (17, 16), building faces Down.
        { (Entity.CaviarFridge, 17, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(17, 15), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(18, 15), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
        // CigaretteStand at (3, 16), building faces Down.
        { (Entity.CigaretteStand, 3, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(3, 15), Direction.Up, Role.Any, 0, InteractableType.Overstock),
            }
        },
        // CigaretteStand at (16, 7), building faces Down.
        { (Entity.CigaretteStand, 16, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(16, 6), Direction.Up, Role.Any, 0),
            }
        },
        // CoffeeMachine at (18, 4), building faces Down.
        { (Entity.CoffeeMachine, 18, 4), new List<Interactable>
            {
                new Interactable(new Vector2d(18, 3), Direction.Up, Role.Any, 0),
            }
        },
        // CognacCabinet at (5, 16), building faces Down.
        { (Entity.CognacCabinet, 5, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(5, 15), Direction.Up, Role.Any, 0, InteractableType.Overstock),
            }
        },
        // CognacCabinet at (22, 7), building faces Down.
        { (Entity.CognacCabinet, 22, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(22, 6), Direction.Up, Role.Any, 0),
            }
        },
        // EasterTable at (5, 10), building faces Down.
        { (Entity.EasterTable, 5, 10), new List<Interactable>
            {
                new Interactable(new Vector2d(5, 11), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(6, 10), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(5, 9), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(4, 10), Direction.Right, Role.Cashier, 3),
            }
        },
        // FertilizerTable at (2, 10), building faces Left.
        { (Entity.FertilizerTable, 2, 10), new List<Interactable>
            {
                new Interactable(new Vector2d(2, 11), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(3, 10), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(2, 9), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(1, 10), Direction.Right, Role.Cashier, 3),
            }
        },
        // FireworksStand at (23, 10), building faces Down.
        { (Entity.FireworksStand, 23, 10), new List<Interactable>
            {
                new Interactable(new Vector2d(23, 11), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(24, 10), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(23, 9), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(22, 10), Direction.Right, Role.Cashier, 3),
            }
        },
        // FortuneWheel at (23, 18), building faces Down.
        { (Entity.FortuneWheel, 23, 18), new List<Interactable>() },
        // Fridge at (0, 16), building faces Down.
        { (Entity.Fridge, 0, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(0, 15), Direction.Up, Role.Any, 0, InteractableType.Overstock),
            }
        },
        // Fridge at (8, 7), building faces Down.
        { (Entity.Fridge, 8, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(8, 6), Direction.Up, Role.Any, 0),
            }
        },
        // HoneyStall at (4, 13), building faces Down.
        { (Entity.HoneyStall, 4, 13), new List<Interactable>
            {
                new Interactable(new Vector2d(4, 12), Direction.Up, Role.Any, 0),
            }
        },
        // HoneyStall at (6, 16), building faces Down.
        { (Entity.HoneyStall, 6, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(6, 15), Direction.Up, Role.Any, 0, InteractableType.Overstock),
            }
        },
        // IceCreamCounter at (12, 13), building faces Down.
        { (Entity.IceCreamCounter, 12, 13), new List<Interactable>
            {
                new Interactable(new Vector2d(12, 12), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(13, 12), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
        // JamTable at (17, 10), building faces Down.
        { (Entity.JamTable, 17, 10), new List<Interactable>
            {
                new Interactable(new Vector2d(17, 11), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(18, 10), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(17, 9), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(16, 10), Direction.Right, Role.Cashier, 3),
            }
        },
        // LobsterFridge at (14, 16), building faces Down.
        { (Entity.LobsterFridge, 14, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(14, 15), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(15, 15), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
        // MaskStand at (20, 10), building faces Down.
        { (Entity.MaskStand, 20, 10), new List<Interactable>
            {
                new Interactable(new Vector2d(20, 11), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(21, 10), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(20, 9), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(19, 10), Direction.Right, Role.Cashier, 3),
            }
        },
        // MeatFridge at (6, 13), building faces Down.
        { (Entity.MeatFridge, 6, 13), new List<Interactable>
            {
                new Interactable(new Vector2d(6, 12), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(7, 12), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
        // MilkStand at (1, 16), building faces Down.
        { (Entity.MilkStand, 1, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(1, 15), Direction.Up, Role.Any, 0, InteractableType.Overstock),
            }
        },
        // MilkStand at (9, 7), building faces Down.
        { (Entity.MilkStand, 9, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(9, 6), Direction.Up, Role.Any, 0),
            }
        },
        // PopcornMachine at (24, 4), building faces Down.
        { (Entity.PopcornMachine, 24, 4), new List<Interactable>
            {
                new Interactable(new Vector2d(24, 3), Direction.Up, Role.Any, 0),
            }
        },
        // PumpkinTable at (13, 7), building faces Down.
        { (Entity.PumpkinTable, 13, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(13, 8), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(14, 7), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(13, 6), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(12, 7), Direction.Right, Role.Cashier, 3),
            }
        },
        // Register #1 POS: native entity cell; robot faces Down.
        { (Entity.Register, 2, 3), new List<Interactable>
            {
                new Interactable(new Vector2d(2, 4), Direction.Down, Role.Cashier, 0),
            }
        },
        // Register #1 bag: native entity cell; robot faces Down.
        { (Entity.Bag, 3, 3), new List<Interactable>
            {
                new Interactable(new Vector2d(3, 4), Direction.Down, Role.Cashier, 1),
            }
        },
        // Register #2 POS: native entity cell; robot faces Down.
        { (Entity.Register, 7, 3), new List<Interactable>
            {
                new Interactable(new Vector2d(7, 4), Direction.Down, Role.Cashier, 0),
            }
        },
        // Register #2 bag: native entity cell; robot faces Down.
        { (Entity.Bag, 8, 3), new List<Interactable>
            {
                new Interactable(new Vector2d(8, 4), Direction.Down, Role.Cashier, 1),
            }
        },
        // Register #3 POS: native entity cell; robot faces Down.
        { (Entity.Register, 12, 3), new List<Interactable>
            {
                new Interactable(new Vector2d(12, 4), Direction.Down, Role.Cashier, 0),
            }
        },
        // Register #3 bag: native entity cell; robot faces Down.
        { (Entity.Bag, 13, 3), new List<Interactable>
            {
                new Interactable(new Vector2d(13, 4), Direction.Down, Role.Cashier, 1),
            }
        },
        // StrawberryStand at (8, 10), building faces Down.
        { (Entity.StrawberryStand, 8, 10), new List<Interactable>
            {
                new Interactable(new Vector2d(8, 11), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(9, 10), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(8, 9), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(7, 10), Direction.Right, Role.Cashier, 3),
            }
        },
        // TangerineTable at (14, 10), building faces Down.
        { (Entity.TangerineTable, 14, 10), new List<Interactable>
            {
                new Interactable(new Vector2d(14, 11), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(15, 10), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(14, 9), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(13, 10), Direction.Right, Role.Cashier, 3),
            }
        },
        // TomatoTable at (11, 7), building faces Down.
        { (Entity.TomatoTable, 11, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(11, 8), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(12, 7), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(11, 6), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(10, 7), Direction.Right, Role.Cashier, 3),
            }
        },
        // TreeStand at (11, 16), building faces Down.
        { (Entity.TreeStand, 11, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(11, 15), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(12, 15), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
        // TruffleFridge at (9, 13), building faces Down.
        { (Entity.TruffleFridge, 9, 13), new List<Interactable>
            {
                new Interactable(new Vector2d(9, 12), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(10, 12), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
        // TulipCounter at (8, 16), building faces Down.
        { (Entity.TulipCounter, 8, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(8, 15), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(9, 15), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
        // VendingMachine at (21, 4), building faces Down.
        { (Entity.VendingMachine, 21, 4), new List<Interactable>
            {
                new Interactable(new Vector2d(21, 3), Direction.Up, Role.Any, 0),
            }
        },
        // WatermelonTable at (11, 10), building faces Down.
        { (Entity.WatermelonTable, 11, 10), new List<Interactable>
            {
                new Interactable(new Vector2d(11, 11), Direction.Down, Role.Stocker, 0),
                new Interactable(new Vector2d(12, 10), Direction.Left, Role.Inventory, 1),
                new Interactable(new Vector2d(11, 9), Direction.Up, Role.Cashier, 2),
                new Interactable(new Vector2d(10, 10), Direction.Right, Role.Cashier, 3),
            }
        },
        // WhiskyCabinet at (4, 16), building faces Down.
        { (Entity.WhiskyCabinet, 4, 16), new List<Interactable>
            {
                new Interactable(new Vector2d(4, 15), Direction.Up, Role.Any, 0, InteractableType.Overstock),
            }
        },
        // WhiskyCabinet at (21, 7), building faces Down.
        { (Entity.WhiskyCabinet, 21, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(21, 6), Direction.Up, Role.Any, 0),
            }
        },
        // WineStand at (18, 7), building faces Down.
        { (Entity.WineStand, 18, 7), new List<Interactable>
            {
                new Interactable(new Vector2d(18, 6), Direction.Up, Role.Cashier, 0),
                new Interactable(new Vector2d(19, 6), Direction.Up, Role.Stocker | Role.Inventory, 1),
            }
        },
    };



    // Native catalog capacities with the saved produce-table upgrade applied.
    public static Dictionary<Entity, int> ShelfCapacities =
        new Dictionary<Entity, int>
    {
        { Entity.Aquarium, 0 },
        { Entity.Bag, 0 },
        { Entity.Balloons, 0 },
        { Entity.BeerFridge, 12 },
        { Entity.Boxes, 0 },
        { Entity.BreadShelf, 8 },
        { Entity.CaviarFridge, 3 },
        { Entity.ChristmasTree, 0 },
        { Entity.CigaretteStand, 9 },
        { Entity.CoffeeMachine, 1 },
        { Entity.CognacCabinet, 9 },
        { Entity.Daisies, 0 },
        { Entity.Dracaena, 0 },
        { Entity.EasterTable, 8 },
        { Entity.FanPalm, 0 },
        { Entity.FertilizerTable, 4 },
        { Entity.FireworksStand, 8 },
        { Entity.Flower, 0 },
        { Entity.FortuneWheel, 0 },
        { Entity.Fridge, 25 },
        { Entity.HoneyStall, 6 },
        { Entity.IceCreamCounter, 5 },
        { Entity.JamTable, 9 },
        { Entity.LobsterFridge, 3 },
        { Entity.MaskStand, 8 },
        { Entity.MeatFridge, 3 },
        { Entity.MilkStand, 12 },
        { Entity.Monstera, 0 },
        { Entity.PalmTree, 0 },
        { Entity.Plant, 0 },
        { Entity.PopcornMachine, 6 },
        { Entity.PumpkinTable, 9 },
        { Entity.Register, 0 },
        { Entity.SignBoard, 0 },
        { Entity.StrawberryStand, 8 },
        { Entity.TangerineTable, 14 },
        { Entity.TomatoTable, 9 },
        { Entity.TreeStand, 2 },
        { Entity.TruffleFridge, 3 },
        { Entity.TulipCounter, 8 },
        { Entity.VendingMachine, 20 },
        { Entity.WaterCooler, 0 },
        { Entity.WatermelonTable, 9 },
        { Entity.WhiskyCabinet, 9 },
        { Entity.WineStand, 24 },
    };
}
