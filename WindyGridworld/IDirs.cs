using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    interface IDirs {
        int Count { get; }
        IReadOnlyList<IDir> All { get; }
    }
}
