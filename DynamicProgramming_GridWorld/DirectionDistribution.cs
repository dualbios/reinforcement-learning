using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming_GridWorld {
    class DirectionDistribution : IDictionary<Direction, float> {
        private IDictionary<Direction, float> probabilities { get; }

        public DirectionDistribution (Func<Direction, float> prob) {
            probabilities = Direction.All.ToDictionary (dir => dir, prob);
        }


        public bool ContainsKey (Direction dir) => probabilities.ContainsKey (dir);

        public void Add (Direction dir, float prob) => throw new InvalidOperationException ();
        public bool Remove (Direction dir) => throw new InvalidOperationException ();

        public bool TryGetValue (Direction dir, out float prob) => probabilities.TryGetValue (dir, out prob);

        public float this[Direction dir] {
            get => probabilities[dir];
            set => probabilities[dir] = value;
        }

        public ICollection<Direction> Keys => probabilities.Keys;
        public ICollection<float> Values => probabilities.Values;

        public void Add (KeyValuePair<Direction, float> pair) => probabilities.Add (pair);

        public void Clear () => throw new InvalidOperationException ();
        public bool Contains (KeyValuePair<Direction, float> item) => throw new InvalidOperationException ();
        public bool Remove (KeyValuePair<Direction, float> item) => throw new InvalidOperationException ();

        public void CopyTo (KeyValuePair<Direction, float>[] array, int arrayIndex) =>
            probabilities.CopyTo (array, arrayIndex);

        public int Count => probabilities.Count;

        public bool IsReadOnly => probabilities.IsReadOnly;

        public IEnumerator<KeyValuePair<Direction, float>> GetEnumerator () => probabilities.GetEnumerator ();
        IEnumerator IEnumerable.GetEnumerator () => probabilities.GetEnumerator ();

        public override string ToString () {
            return string.Join ("", probabilities
                .Where (pair => pair.Value > 0)
                .Select (pair => pair.Key.ToString ()));
        }
    }
}
