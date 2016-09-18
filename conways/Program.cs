using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conways
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display help message if requested
            if (args.Contains("--help"))
            {
                Console.WriteLine("Conway's Game of Life - A C# implementation");
                Console.WriteLine("\tUsage: conways [options] <output_file>");
                Console.WriteLine("\t\t<number_of_interations>\t\t- The number of iterations of the game to run");
                Console.WriteLine("\t\t<output_file>\t\t- The file to save output to");
                Console.WriteLine("\tOptions:");
                Console.WriteLine("\t\t--help\t\t- Display this help message");
                Console.WriteLine("\t\t-i\t\t- Provide a file containing a grid");
                Console.WriteLine("\t\t-w\t\t- Provide a grid width");
                Console.WriteLine("\t\t-h\t\t- Provide a grid height");
                Console.WriteLine("\t\t-n\t\t- Provide a number of iterations to run");
            }
            else
            {
                Random r = new Random();
                // Load input file if provided, otherwise generate grid
                var grid = new CellState[0][];
                var inputFlagPos = Array.IndexOf(args, "-i");
                uint gridHeight = 0;
                uint gridWidth = 0;
                // Load input grid from file
                if (inputFlagPos != -1 && inputFlagPos + 1 < args.Length - 3)
                {
                    Console.WriteLine("Using input file: {0}", args[inputFlagPos + 1]);
                    // TODO : load grid from file
                    throw new NotSupportedException("Grid loading is not currently supported");
                }
                else
                {
                    // Generate random grid
                    Console.WriteLine("Using randomly generated starting grid");
                    var heightFlagPos = Array.IndexOf(args, "-h");
                    if (heightFlagPos != -1 && heightFlagPos + 1 <= args.Length - 2 && !uint.TryParse(args[heightFlagPos + 1], out gridHeight))
                    {
                        throw new ArgumentException("Invalid value for -h flag");
                    }
                    else
                    {
                        gridHeight = (uint)r.Next(25, 100);
                    }
                    Console.WriteLine("Grid height: {0}", gridHeight);
                    var widthFlagPos = Array.IndexOf(args, "-w");
                    if (widthFlagPos != -1 && widthFlagPos + 1 <= args.Length - 2 && !uint.TryParse(args[widthFlagPos + 1], out gridWidth))
                    {
                        throw new ArgumentException("Invalid value for -w flag");
                    }
                    else
                    {
                        gridWidth = (uint)r.Next(25, 100);
                    }
                    Console.WriteLine("Grid width: {0}", gridWidth);
                    grid = new CellState[gridHeight][];
                    for (int i = 0; i < gridHeight; i++)
                    {
                        grid[i] = new CellState[gridWidth];
                        for (int j = 0; j < gridWidth; j++)
                        {
                            if (r.NextDouble() < 0.5)
                            {
                                grid[i][j] = CellState.Dead;
                            }
                            else
                            {
                                grid[i][j] = CellState.Alive;
                            }
                        }
                    }
                    Console.WriteLine("Grid generated");
                }
                // Load iteration number
                uint iterationNumber = 100;
                var iterFlagPos = Array.IndexOf(args, "-n");
                if (iterFlagPos != -1 && iterFlagPos + 1 <= args.Length - 2)
                {
                    if (!uint.TryParse(args[iterFlagPos + 1], out iterationNumber))
                    {
                        throw new ArgumentException("Invalid value for -n flag");
                    }
                }
                // Run simulation
                var sim = new Simulation(iterationNumber, grid);
                sim.Run();
                // Output to file
                // TODO : output to file
            }
        }
    }
}
