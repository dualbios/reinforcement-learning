using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    class Dir : IDir {
        public int Row { get; }
        public int Col { get; }

        public int Index { get; }
        public int Degrees { get; }

        public string Name { get; }
        public PointF[] Area { get; }

        public Dir (int row, int col, int index, int degrees, string name, PointF[] area) {
            Row = row;
            Col = col;
            Index = index;
            Degrees = degrees;
            Name = name;
            Area = area;
        }

        public override string ToString () => $"{Name} ({Col}, {Row})";
    }
}
