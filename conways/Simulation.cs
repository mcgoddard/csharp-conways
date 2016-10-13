using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace conways
{
    enum CellState
    {
        Dead = 0,
        Alive = 1
    };

    class Simulation
    {
        #region Private members
        // Immutable
        private readonly uint iterationNum;
        private readonly CellState[][] startingStates;
        private readonly int height;
        private readonly int width;
        private readonly string outputDir;
        // Mutable
        private Cell[][] curStates;
        private uint currentIter = 0;
        #endregion
        
        #region Constructors
        public Simulation (uint iterationNum, CellState[][] states, string outputDir)
        {
            this.iterationNum = iterationNum;
            this.startingStates = states;
            this.curStates = states.Select(r => r.Select(s => new Cell(s)).ToArray()).ToArray();
            this.height = states.Length;
            this.width = states[0].Length;
            this.outputDir = outputDir;
        }
        #endregion

        #region Public methods
        public void Run()
        {
            while (currentIter < iterationNum)
            {
                var newStates = new Cell[height][];
                var threads = new Thread[height * width];
                for (int i = 0; i < height; i++)
                {
                    var tmpI = i;
                    newStates[tmpI] = new Cell[width];
                    for (int j = 0; j < width; j++)
                    {
                        var tmpJ = j;
                        threads[(tmpI * width) + tmpJ] = new Thread(new ThreadStart(() =>
                        {
                            var neighbours = new Cell[3][];
                            for (int k = -1; k <= 1; k++)
                            {
                                neighbours[k + 1] = new Cell[]
                                {
                                    ((tmpI + k >= 0) && (tmpI + k < height) && (tmpJ - 1 > 0)) ? curStates[tmpI + k][tmpJ - 1] : new Cell(CellState.Dead),
                                    ((tmpI + k >= 0)  && (tmpI + k < height)) ? curStates[tmpI + k][tmpJ] : new Cell(CellState.Dead),
                                    ((tmpI + k >= 0) && (tmpI + k < height) && (tmpJ + 1 < width)) ? curStates[tmpI + k][tmpJ + 1] : new Cell(CellState.Dead),
                                };
                            }
                            newStates[tmpI][tmpJ] = curStates[tmpI][tmpJ].Iterate(neighbours);
                        }));
                        threads[(tmpI * width) + tmpJ].Start();
                    }
                }
                foreach (var t in threads)
                {
                    t.Join();
                }
                curStates = newStates;
                var filePath = Path.Combine(outputDir, String.Format("{0}.csv", currentIter));
                using (StreamWriter file = new StreamWriter(filePath))
                {
                    foreach (var row in curStates)
                    {
                        file.WriteLine(string.Join(",", row.Select(r => (int)r.State)));
                    }
                }
                currentIter++;
            }
        }
        #endregion
    }
}
