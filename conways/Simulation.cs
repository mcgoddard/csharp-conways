using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conways
{
    enum CellState
    {
        Dead,
        Alive
    };

    class Simulation
    {
        #region Private members
        // Immutable
        private readonly uint iterationNum;
        private readonly CellState[][] startingStates;
        private readonly int height;
        private readonly int width;
        // Mutable
        private Cell[][] curStates;
        private uint currentIter = 0;
        #endregion
        
        #region Constructors
        public Simulation (uint iterationNum, CellState[][] states)
        {
            this.iterationNum = iterationNum;
            this.startingStates = states;
            this.curStates = states.Select(r => r.Select(s => new Cell(s)).ToArray()).ToArray();
            this.height = states.Length;
            this.width = states[0].Length;
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
                currentIter++;
            }
        }
        #endregion
    }
}
