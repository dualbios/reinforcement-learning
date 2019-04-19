using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming_GridWorld {
    class GridWorld_Main {
        static void Main (string[] args) => new GridWorld_Main ().Run ();

        private Board<float> value;
        private Board<DirectionDistribution> policy;

        private void Run () {
            value = new Board<float> (() => 0f);
            policy = new Board<DirectionDistribution> (() => new DirectionDistribution (dir => 0.25f));

            value.Print ();
            policy.Print ();
        }
    }
}
