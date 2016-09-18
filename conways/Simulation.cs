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
        // Mutable
        private CellState[][] curStates;
        private uint currentIter = 0;
        #endregion
        
        #region Constructors
        public Simulation (uint iterationNum, CellState[][] states)
        {
            this.iterationNum = iterationNum;
            this.startingStates = states;
            this.curStates = states;
        }
        #endregion

        #region Public methods
        public void Run()
        {
            throw new NotImplementedException("Simulations can not yet be run");
        }
        #endregion
    }
}
