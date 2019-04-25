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
            private set => values[(state, action)] = value;
        }

        public AgentAction Greedy (State where) {
            IList<AgentAction> best = new List<AgentAction> ();
            float max = float.NegativeInfinity;
            foreach (AgentAction action in AgentAction.All) {
                float value = values[(where, action)];
                if (value > max + 0.001f) {
                    best.Clear ();
                    best.Add (action);
                    max = value;
                }
                else if (value > max - 0.001f)
                    best.Add (action);
            }
            return Rng.Choose (best);
        }

        public void SarsaUpdate (State from, AgentAction action, float reward, State to, AgentAction nextAction) {
            this[from, action] = (1 - Learning.Rate) * this[from, action] +
                Learning.Rate * (reward + Learning.FutureDiscount * this[to, nextAction]);
        }

        public void QLearningUpdate (State from, AgentAction action, float reward, State to) {
            this[from, action] = (1 - Learning.Rate) * this[from, action] +
                Learning.Rate * (reward + Learning.FutureDiscount * AgentAction.All
                    .Select (nextAction => this[to, nextAction]).Max ());
        }
    }
}
