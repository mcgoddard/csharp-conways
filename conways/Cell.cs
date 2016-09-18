using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conways
{
    class Cell
    {
        #region Private members
        private readonly CellState state;
        #endregion

        #region Public members
        public CellState State
        {
            get
            {
                return state;
            }
        }
        #endregion

        #region Constructors
        public Cell(CellState state)
        {
            this.state = state;
        }
        #endregion

        #region Public methods
        public Cell Iterate(Cell[][] neighbours)
        {
            if (neighbours.Length != 3 || neighbours.Any(s => s.Length != 3))
            {
                throw new ArgumentException("Incorrect shape of neighbours array");
            }
            uint aliveCount = (uint)neighbours.Aggregate(0, (acc, r) => acc + r.Where(c => c.State == CellState.Alive).Count());
            if (state == CellState.Alive)
            {
                aliveCount -= 1;
            }
            return Rules(aliveCount);
        }
        #endregion

        #region Private methods
        private Cell Rules(uint aliveCount)
        {
            throw new NotImplementedException("Conway's cell rules not yet implemented");
        }
        #endregion
    }
}
