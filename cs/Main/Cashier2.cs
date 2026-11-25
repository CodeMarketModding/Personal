using System;

// Assigned to native register number 2.
var cashier = new Cashier(2);
cashier.OnProgress = message => Console.WriteLine(message);
cashier.Run();
