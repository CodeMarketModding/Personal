using System;
using System.Collections.Generic;

var inventory = new Inventory();
inventory.OnOrdered = products =>
{
    var counts = new Dictionary<Product, int>();
    foreach (Product product in products)
    {
        if (!counts.ContainsKey(product)) counts[product] = 0;
        counts[product]++;
    }
    Console.WriteLine("Delivery ordered: " + products.Count + " items");
    string line = "";
    foreach (var item in counts)
    {
        string entry = item.Key.GetName() + " x" + item.Value;
        // Keep each line below the robot console's character limit.
        if (line.Length > 0 && line.Length + 3 + entry.Length > 180)
        {
            Console.WriteLine(line);
            line = "";
        }
        if (line.Length > 0) line += " | ";
        line += entry;
    }
    if (line.Length > 0) Console.WriteLine(line);
};
inventory.Run();
