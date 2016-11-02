using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
                Console.WriteLine("\tUsage: conways [options]");
                Console.WriteLine("\t\t<number_of_interations>\t\t- The number of iterations of the game to run");
                Console.WriteLine("\t\t<output_file>\t\t- The file to save output to");
                Console.WriteLine("\tOptions:");
                Console.WriteLine("\t\t--help\t\t- Display this help message");
                Console.WriteLine("\t\t-i\t\t- Provide a file containing a grid");
                Console.WriteLine("\t\t-w\t\t- Provide a grid width");
                Console.WriteLine("\t\t-h\t\t- Provide a grid height");
                Console.WriteLine("\t\t-n\t\t- Provide a number of iterations to run");
                Console.WriteLine("\t\t-p\t\t- Pause when finished");
                Console.WriteLine("\t\t-o\t\t- Output directory");
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
                    var inputFilePath = args[inputFlagPos + 1];
                    Console.WriteLine("Using input file: {0}", inputFilePath);
                    if (File.Exists(inputFilePath))
                    {
                        var tmpGrid = new List<CellState[]>();
                        foreach (var line in File.ReadLines(inputFilePath))
                        {
                            var characters = line.Split(',');
                            var cellStates = new CellState[characters.Length];
                            for (int i = 0; i < characters.Length; i++)
                            {
                                try
                                {
                                    int parsedCharacter = -1;
                                    if (int.TryParse(characters[i], out parsedCharacter))
                                    {
                                        cellStates[i] = (CellState)parsedCharacter;
                                    }
                                    else
                                    {
                                        throw new ArgumentException(String.Format("Invalid value in input file \"{0}\"", characters[i]));
                                    }
                                }
                                catch (InvalidCastException ex)
                                {
                                    throw new AggregateException("Cannot parse input file", ex);
                                }
                            }
                            tmpGrid.Add(cellStates);
                        }
                        grid = tmpGrid.ToArray();
                        gridHeight = (uint)grid.Length;
                        gridWidth = (uint)grid[0].Length;
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("Input file \"{0}\" not found", inputFilePath));
                    }
                }
                else
                {
                    // Generate random grid
                    Console.WriteLine("Using randomly generated starting grid");
                    var heightFlagPos = Array.IndexOf(args, "-h");
                    if (heightFlagPos != -1 && heightFlagPos + 1 <= args.Length - 1)
                    {
                        if (!uint.TryParse(args[heightFlagPos + 1], out gridHeight))
                        {
                            throw new ArgumentException("Invalid value for -h flag");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Using random grid height");
                        gridHeight = (uint)r.Next(25, 100);
                    }
                    var widthFlagPos = Array.IndexOf(args, "-w");
                    if (widthFlagPos != -1 && widthFlagPos + 1 <= args.Length - 1)
                    {
                        if (!uint.TryParse(args[widthFlagPos + 1], out gridWidth))
                        {
                            throw new ArgumentException("Invalid value for -w flag");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Using random grid width");
                        gridWidth = (uint)r.Next(25, 100);
                    }
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
                Console.WriteLine("Grid height: {0}", gridHeight);
                Console.WriteLine("Grid width: {0}", gridWidth);
                // Load iteration number
                uint iterationNumber = 100;
                var iterFlagPos = Array.IndexOf(args, "-n");
                if (iterFlagPos != -1 && iterFlagPos + 1 <= args.Length - 1)
                {
                    if (!uint.TryParse(args[iterFlagPos + 1], out iterationNumber))
                    {
                        throw new ArgumentException("Invalid value for -n flag");
                    }
                }
                Console.WriteLine(String.Format("Running {0} iterations", iterationNumber));
                // Check output directory exists
                string outputDir = null;
                var outputFlagPos = Array.IndexOf(args, "-o");
                if (outputFlagPos != -1 && outputFlagPos + 1 <= args.Length - 1)
                {
                    outputDir = args[outputFlagPos + 1];
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                Console.WriteLine(String.Format("Output directory set to \"{0}\"", outputDir));
                // Run simulation
                Stopwatch s = new Stopwatch();
                s.Start();
                Simulation sim;
                if (outputDir != null)
                {
                    sim = new Simulation(iterationNumber, grid, outputDir);
                }
                else
                {
                    sim = new Simulation(iterationNumber, grid);
                }
                sim.Run();
                s.Stop();
                // Print stats
                Console.WriteLine("Iterated {0} in {1}ms", gridHeight * gridWidth * iterationNumber, s.ElapsedMilliseconds);
                // Pause
                if (args.Contains("-p"))
                {
                    Console.WriteLine("Press <<enter>> to exit...");
                    Console.ReadLine();
                }
            }
        }
    }
}
