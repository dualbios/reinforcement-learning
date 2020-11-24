using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    static class Mathf {
        public static readonly float Pi = (float) Math.PI;

        public static float CosPi (float angle) => (float) Math.Cos (angle * Pi);
        public static float SinPi (float angle) => (float) Math.Sin (angle * Pi);

        public static PointF PolarPi (float angle, float distance) =>
            new PointF (
                CosPi (angle) * distance,
                SinPi (angle) * distance);
    }
}
