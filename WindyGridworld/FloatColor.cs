using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    class FloatColor {
        public IReadOnlyList<float> RGB { get; }  // 0=R, 1=G, 2=B. Range 0..1

        public FloatColor (float r, float g, float b) =>
            RGB = new[] { r, g, b };
        public FloatColor (IEnumerable<float> values) =>
            RGB = values.ToList ();

        public FloatColor Select (Func<float, float> map) =>
            new FloatColor (RGB.Select (map));

        public FloatColor Clamp () =>
            Select (x => Math.Max (0, Math.Min (1, x)));

        private const int Components = 3;
        public static FloatColor operator + (FloatColor a, FloatColor b) =>
            Enumerable.Range (0, Components)
                .Select (i => a.RGB[i] + b.RGB[i])
                .ToFloatColor ();
        public static FloatColor operator * (FloatColor a, float b) =>
            a.Select (x => x * b);

        public static FloatColor Mix (FloatColor a, FloatColor b, float ratio) =>
            a * ratio + b * (1 - ratio);

        public Brush ToBrush () => new SolidBrush (ToColor ());
        public Color ToColor () => Color.FromArgb (
            To0_255 (RGB[0]), To0_255 (RGB[1]), To0_255 (RGB[2]));

        private static int To0_255 (float x) =>
            Math.Max (0, Math.Min (255, (int) (x * 255)));
    }
}
