using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    class Pos {
        public int Row { get; }
        public int Col { get; }

        public Pos (int row, int col) {
            Row = row;
            Col = col;
        }

        public static Pos operator + (Pos pos, IDir dir) =>
            new Pos (pos.Row + dir.Row, pos.Col + dir.Col);
        public static Pos operator + (Pos pos, Pos offset) =>
            new Pos (pos.Row + offset.Row, pos.Col + offset.Col);

        public Pos ClampTo (Field field) => new Pos (
            Math.Max (0, Math.Min (field.Height - 1, Row)),
            Math.Max (0, Math.Min (field.Width - 1, Col)));

        public override bool Equals (object obj) =>
            obj is Pos that && this.Row == that.Row && this.Col == that.Col;

        public override int GetHashCode () => Row.GetHashCode () * 11 + 5 + Col.GetHashCode ();

        public static bool operator == (Pos a, Pos b) => a.Equals (b);
        public static bool operator != (Pos a, Pos b) => !a.Equals (b);

        public override string ToString () => $"({Col}, {Row})";
    }
}
