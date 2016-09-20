using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                Task<Cell>[][] newStates = new Task<Cell>[height][];
                for (int i = 0; i < height; i++)
                {
                    newStates[i] = new Task<Cell>[width];
                    for (int j = 0; j < width; j++)
                    {
                        var tmpI = i;
                        var tmpJ = j;
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
                        newStates[i][j] = Task.Factory.StartNew(() => curStates[tmpI][tmpJ].Iterate(neighbours));
                    }
                }
                curStates = newStates.Select(tcr => tcr.Select(tc => tc.Result).ToArray()).ToArray();
                using (StreamWriter file = new StreamWriter(Path.Combine(outputDir, String.Format("{0}.csv", currentIter))))
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
