// Robert Tetreault (rrt2850@g.rit.edu)

/*******************************************************************************************
* File: Program.cs
* -----------------------------------------------------------------------------------------
* This program is the entry point for the PrimeGen program for project 2 of CSCI251. It
* generates a specified quantity of prime numbers with a specified number of bits and
* prints them to the console as they are generated.
********************************************************************************************/

using System;
using System.Numerics;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    /// <summary>
    /// Main method
    /// </summary>
    /// <param name="args">Command line arguments</param>
    static void Main(string[] args){
        // Initialize variables to hold arguments
        int bits, count;
        
            // Exit if no arguments or first argument is not a valid integer or not in a valid range
        if (args.Length < 1 || !int.TryParse(args[0], out bits) || bits < 32 || bits % 8 != 0)
        {
            Console.WriteLine("Usage: dotnet run <bits> [<count>]");
            return;
        }

        // Check for the second argument, if invalid or not passed set count to 1
        if (!(args.Length > 1 && int.TryParse(args[1], out count) && count >= 1))
            count = 1;
        
        Console.WriteLine($"BitLength: {bits}"); // Output the bit length

        // If all inputs are valid, make a new PrimeGenerator
        PrimeGenerator generator = new PrimeGenerator();

        // Start a stopwatch to time how long it takes to generate primes
        Stopwatch stopwatch = Stopwatch.StartNew();

        Task[] tasks = new Task[count];

        
        int completedTasks = 0;             // Create a counter for completed tasks
        object consoleLock = new object();   // Create a lock object for the console

        for (int i = 0; i < count; i++){
            tasks[i] = Task.Run(() =>   {
                BigInteger prime = generator.GeneratePrime(bits);   // Generate a prime

                lock (consoleLock){
                    // Print the prime number
                    Console.Write($"{Task.CurrentId}: {prime}");

                    // If it's not the last task, print a newline.
                    if (++completedTasks < count) Console.WriteLine("\n");
                }
            });
        }

        Task.WaitAll(tasks);

        TimeSpan time = stopwatch.Elapsed;                  // Get the elapsed time
        Console.WriteLine($"\nTime to Generate: {time}");     // Output the time
    }
}
