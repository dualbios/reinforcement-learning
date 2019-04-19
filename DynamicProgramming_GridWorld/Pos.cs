using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming_GridWorld {
    class Pos {
        public const int Rows = 4;
        public const int Cols = 4;

        public static Pos TopLeft { get; } = new Pos (0, 0);
        public static Pos BottomRight { get; } = new Pos (Rows - 1, Cols - 1);

        public static IEnumerable<Pos> Final { get; } = new[] {
            TopLeft, BottomRight
        };

        public int Row { get; }
        public int Col { get; }

        public Pos (int row, int col) {
            Row = row;
            Col = col;
        }

        public bool IsFinal => Final.Contains (this);

        public static bool operator == (Pos a, Pos b) =>
            a.Row == b.Row && a.Col == b.Col;
        public static bool operator != (Pos a, Pos b) => !(a == b);

        public static Pos operator + (Pos a, Pos b) =>
            new Pos (a.Row + b.Row, a.Col + b.Col);
        public static Pos operator - (Pos a, Pos b) =>
            new Pos (a.Row - b.Row, a.Col - b.Col);
        public static Pos operator * (Pos a, int b) =>
            new Pos (a.Row * b, a.Col * b);

        public override bool Equals (object obj) {
            if (obj == null || GetType () != obj.GetType ())
                return false;
            Pos that = (Pos) obj;
            return this.Row == that.Row && this.Col == that.Col;
        }
        public override int GetHashCode () =>
            Row.GetHashCode () * 5 + Col.GetHashCode ();
    }
}
