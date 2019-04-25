using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning_Sarsa {
    class QValues {
        private IDictionary<(State, AgentAction), float> values;

        public QValues () {
            values = State.All
                .SelectMany (state => AgentAction.All
                    .Select (action => (state, action)))
                .ToDictionary (pair => pair, pair => 0f);
        }
        public QValues (IDictionary<(State, AgentAction), float> values) =>
            this.values = values.ToDictionary (pair => pair.Key, pair => pair.Value);

        public float this[State state, AgentAction action] {
            get => values[(state, action)];
        }
    }
}
