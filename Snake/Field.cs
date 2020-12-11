using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake {
    class Field {
        public const int DefaultWidth = 40;
        public const int DefaultHeight = 40;

        public int Width { get; }
        public int Height { get; }

        public Field () : this (DefaultWidth, DefaultHeight) { }
        public Field (int width, int height) {
            Width = width;
            Height = height;
        }

        public bool Contains (Pos pos) =>
            pos.Row >= 0 && pos.Row < Height && pos.Col >= 0 && pos.Col < Width;
    }
}
