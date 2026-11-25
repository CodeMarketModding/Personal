using System;

var stocker = new Stocker();
stocker.OnProgress = message => Console.WriteLine(message);
stocker.Run();

