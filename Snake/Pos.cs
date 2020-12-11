using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Snake {
    struct Pos {
        public static IReadOnlyList<Pos> Dir4 { get; } = new[] {
            new Pos (-1, 0), new Pos (0, 1), new Pos (1, 0), new Pos (0, -1)
        };
        public static IReadOnlyList<Pos> Dir8 { get; } = new[] {
            new Pos (-1, 0), new Pos (-1, 1), new Pos (0, 1), new Pos (1, 1),
            new Pos (1, 0), new Pos (1, -1), new Pos (0, -1), new Pos (-1, -1)
        };


        public int Row { get; }
        public int Col { get; }

        public Pos (int row, int col) {
            Row = row;
            Col = col;
        }
        public static Pos Random (Field field) =>
            new Pos (Rng.IntEx (field.Height), Rng.IntEx (field.Width));

        public override bool Equals (object obj) =>
            obj is Pos that && this.Row == that.Row && this.Col == that.Col;

        public override int GetHashCode () => (Row, Col).GetHashCode ();

        public override string ToString () => $"({Row}r, {Col}c)";

        public static bool operator == (Pos a, Pos b) => a.Equals (b);
        public static bool operator != (Pos a, Pos b) => !a.Equals (b);

        public static Pos operator + (Pos a, Pos b) => new Pos (a.Row + b.Row, a.Col + b.Col);
        public static Pos operator * (int n, Pos a) => new Pos (n * a.Row, n * a.Col);
    }
}
