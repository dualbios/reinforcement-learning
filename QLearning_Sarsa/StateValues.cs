using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning_Sarsa {
    class StateValues {
        private IDictionary<State, float> values;

        public StateValues () =>
            values = State.All
                .ToDictionary (state => state, state => 0f);
        public StateValues (IDictionary<State, float> values) =>
            this.values = values.ToDictionary (pair => pair.Key, pair => pair.Value);

        public float this[State state] {
            get => values[state];
            private set => values[state] = value;
        }

        public Color GetColor (State state) {
            float value = this[state];
            int red = Math.Max (0, Math.Min (255, (int) (-value * 5 * 2.55f)));
            return Color.FromArgb (red, 0, 0);
        }

        public void TemporalDifferenceUpdate (State from, float reward, State to) {
            this[from] = (1 - Learning.Rate) * this[from] +
                Learning.Rate * (reward + Learning.FutureDiscount * this[to]);
        }
    }
}
