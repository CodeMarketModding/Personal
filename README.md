# Personal Code Market scripts

My current robot scripts for Code Market, with shared components, store data, and precomputed walking routes. This repository currently contains C# scripts and a floor map image.

![World 10 floor map and interaction legend](maps/FloorMap.png)

[Full-size PNG](maps/FloorMap.png) · [SVG](maps/FloorMap.svg). Decorations are omitted from the image; navigation still accounts for their occupied tiles.

## Scripts

| Entry script | Job |
| --- | --- |
| `Main/Cashier` | Serves register **1**. |
| `Main/Cashier2` | Serves register **2**. |
| `Main/Cashier3` | Serves register **3**. |
| `Main/Inventory` | Orders deliveries, prioritizing SalesFloor shortages before Overstock refills and accounting for existing stock, incoming goods, money, and truck capacity. |
| `Main/Stocker` | Unloads deliveries and fills SalesFloor shelves before Overstock. |

Cashiers collect orders, bag goods, check age and alcohol sale hours, and retry missing products before removing unavailable items. Their robot consoles show order progress, stock checks, retry countdowns, and completion. Inventory prints each successful delivery's total and grouped product quantities. Stocker prints loading and restocking progress.

## Layout

```text
cs/
  Core/          Shared classes, enums, components, and navigation
  Data/          FloorMap and PathMap
  Main/          Robot entry scripts
maps/
  FloorMap.png   Store layout and interaction legend
  FloorMap.svg   Scalable version of the same map
```

There are currently no JavaScript or Python script versions.

## Use in the game

These `.cs` files are exports of the game's virtual scripts. Import or paste all files under `cs/` into the game's script editor, preserving their folder paths and removing the `.cs` extension. For example, `cs/Core/Components/Interactable.cs` becomes `Core/Components/Interactable`.

Assign each cashier robot its matching numbered entry script. Assign the other robots `Main/Inventory` and `Main/Stocker`. Shared files under `Core/` and `Data/` supply their dependencies.

The included maps describe **World 10, 29 × 19 cells**. `FloorMap` stores fixture interaction points, roles, SalesFloor/Overstock classifications, and capacities. `PathMap` stores routes and parking reservations so robots can follow prepared steps.

After moving fixtures or changing the store layout, save the game and regenerate **both maps** before restarting the robots. A different store needs maps generated for its own layout.
