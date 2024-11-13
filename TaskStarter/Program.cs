using System;

   namespace TaskStarter
   {
       class Program
       {
           static void Main(string[] args)
           {
            Task.Program.Main(new string[] { });

            Console.WriteLine("\nНатисніть Enter, щоб завершити...");
            Console.ReadLine();
           }
       }
   }