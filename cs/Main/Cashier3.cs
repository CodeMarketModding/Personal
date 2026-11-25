using System;

// Assigned to native register number 3.
var cashier = new Cashier(3);
cashier.OnProgress = message => Console.WriteLine(message);
cashier.Run();
