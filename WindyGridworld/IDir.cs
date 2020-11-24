using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    interface IDir {
        int Row { get; }
        int Col { get; }

        int Index { get; }
        int Degrees { get; }

        string Name { get; }
        PointF[] Area { get; }
    }
}
