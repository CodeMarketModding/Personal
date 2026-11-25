using System;

// Assigned to native register number 1.
var cashier = new Cashier(1);
cashier.OnProgress = message => Console.WriteLine(message);
cashier.Run();
