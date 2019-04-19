using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming_GridWorld {
    class Direction : Pos {
        public static Direction Left { get; } = new Direction (0, -1, "←");
        public static Direction Up { get; } = new Direction (-1, 0, "↑");
        public static Direction Down { get; } = new Direction (1, 0, "↓");
        public static Direction Right { get; } = new Direction (0, 1, "→");

        public static IReadOnlyList<Direction> All { get; } = new[] {
            Left, Up, Down, Right
        };

        public string Name { get; }

        public Direction (int row, int col, string name) : base (row, col) =>
            Name = name;

        public override string ToString () => Name;
    }
}
