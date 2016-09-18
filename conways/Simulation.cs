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
        private readonly uint iterationNum;
        private readonly CellState[][] startingStates;
        private CellState[][] curStates;
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
