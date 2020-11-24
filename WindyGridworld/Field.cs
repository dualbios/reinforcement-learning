using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    class Field {
        public int Height { get; }
        public int Width { get; }

        public Pos Start { get; }
        public Pos Goal { get; }

        public IReadOnlyList<int> Wind { get; }

        public static Field FieldForThisGame () => new Field (7, 10,
            new Pos (3, 0), new Pos (3, 7),
            new int[10] { 0, 0, 0, 1, 1, 1, 2, 2, 1, 0 });
        private Field (int height, int width, Pos start, Pos goal, IReadOnlyList<int> wind) {
            Height = height;
            Width = width;
            Start = start;
            Goal = goal;
            Wind = wind;
        }
    }
}
