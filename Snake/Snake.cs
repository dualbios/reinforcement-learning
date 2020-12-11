using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake {
    class Snake {
        public const int StartLength = 5;

        public IReadOnlyList<Pos> Body { get; private set; }

        public Pos Head => Body[0];

        public Snake (IReadOnlyList<Pos> body) => Body = body;

        public static Snake Random (Field field) =>
            new Snake (Enumerable.Repeat (Pos.Random (field), StartLength).ToList ());

        public Snake MoveTo (Pos pos, bool ateApple) =>
            new Snake (new[] { pos }
                .Concat (Body.Take (Body.Count - (ateApple ? 0 : 1)))
                .ToList ());
    }
}
