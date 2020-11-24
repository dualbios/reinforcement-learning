using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    static class Extensions {
        public static int ArgMax (this IEnumerable<float> values) {
            int bestIndex = -1;
            float bestValue = float.MinValue;
            int index = 0;
            foreach (float value in values) {
                if (value > bestValue) {
                    bestIndex = index;
                    bestValue = value;
                }
                index++;
            }
            if (bestIndex == -1)
                throw new InvalidOperationException ("Collection was empty");
            return bestIndex;
        }

        public static FloatColor ToFloatColor (this IEnumerable<float> values) =>
            new FloatColor (values);
    }
}
