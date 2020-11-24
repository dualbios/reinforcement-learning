using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    class Dirs4 : IDirs {
        public int Count => All.Count;
        public IReadOnlyList<IDir> All { get; }

        private static IReadOnlyList<(int x, int y)> offsets = new[] {
            (0, -1), (1, 0), (0, 1), (-1, 0)
        };
        private static IReadOnlyList<string> names = new[] {
            "Up", "Right", "Down", "Left"
        };

        private static readonly float Sqrt2_Half = (float) Math.Sqrt (2);
        public Dirs4 () => All = Enumerable.Range (0, 4)
            .Select (i => new Dir (offsets[i].y, offsets[i].x, i, i * 90, names[i],
                new[] {
                    new PointF (0, 0),
                    Mathf.PolarPi (-0.75f + i * 0.5f, Sqrt2_Half),
                    Mathf.PolarPi (-0.25f + i * 0.5f, Sqrt2_Half)
                }))
            .ToList ();
    }
}
