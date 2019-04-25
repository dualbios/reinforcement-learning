using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning_Sarsa {
    // Direction of movement
    class AgentAction : State {
        public static AgentAction Up { get; } = new AgentAction (-1, 0, "↑");
        public static AgentAction Right { get; } = new AgentAction (0, 1, "→");
        public static AgentAction Down { get; } = new AgentAction (1, 0, "↓");
        public static AgentAction Left { get; } = new AgentAction (0, -1, "←");

        public static new IEnumerable<AgentAction> All { get; } = new[] {
            Up, Right, Down, Left
        };

        public string Name { get; }

        public AgentAction (int row, int col, string name) : base (row, col) => Name = name;
    }
}
