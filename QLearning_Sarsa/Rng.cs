using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning_Sarsa {
    class Rng {
        public static Random Random { get; } = new Random ();

        public static T Choose<T> (IReadOnlyList<T> items) => items[Random.Next (0, items.Count)];
        public static T Choose<T> (IList<T> items) => items[Random.Next (0, items.Count)];
    }
}
