using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    class Dirs8 : IDirs {
        public int Count => All.Count;
        public IReadOnlyList<IDir> All { get; }

        private static IReadOnlyList<(int x, int y)> offsets = new[] {
            (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1)
        };
        private static IReadOnlyList<string> names = new[] {
            "N", "NE", "E", "SE", "S", "SW", "W", "NW"
        };

        private static readonly float Sqrt2_Half = (float) Math.Sqrt (2);
        private static readonly float Petal = 1f / Mathf.CosPi (0.125f);
        public Dirs8 () => All = Enumerable.Range (0, 8)
            .Select (i => new Dir (offsets[i].y, offsets[i].x, i, i * 45, names[i],
                MakeArea (i)))
            .ToList ();
        private static PointF[] MakeArea (int i) =>
            i % 2 == 0 ? MakeEvenArea (i) : MakeOddArea (i);
        private static PointF[] MakeEvenArea (int i) =>
            new[] {
                new PointF (0, 0),
                Mathf.PolarPi (-0.625f + i * 0.25f, Petal),
                Mathf.PolarPi (-0.375f + i * 0.25f, Petal)
            };
        private static PointF[] MakeOddArea (int i) =>
            new[] {
                new PointF (0, 0),
                Mathf.PolarPi (-0.375f + (i - 1) * 0.25f, Petal),
                Mathf.PolarPi (-0.25f + (i - 1) * 0.25f, Sqrt2_Half),
                Mathf.PolarPi (-0.125f + (i - 1) * 0.25f, Petal)
            };
    }
}
