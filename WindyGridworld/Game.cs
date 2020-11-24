using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyGridworld {
    class Game {
        public Field Field { get; }
        public IDirs Dirs { get; }

        public Pos Player { get; private set; }
        private IDir NextMove { get; set; }

        public float[, , ] StateActionValues { get; }

        public int Height => Field.Height;
        public int Width => Field.Width;

        public int LocalStep { get; private set; }
        public int GlobalStep { get; private set; }
        public int Episode { get; private set; }

        public Font Font { get; }

        public Game (IDirs dirs, Font font) {
            Field = Field.FieldForThisGame ();
            Dirs = dirs;
            Player = Field.Start;
            StateActionValues = new float[Height, Width, Dirs.Count];
            NextMove = ChooseAction ();
            Font = font;
        }

        public void Draw (Graphics g) {
            for (int row = 0; row < Height; row++)
                for (int col = 0; col < Width; col++) {
                    Pos pos = new Pos (row, col);
                    IDir bestMove = BestMove (pos);
                    FillCell (g, pos);
                    DrawArrow (g, pos, bestMove, StateActionValues[pos.Row, pos.Col, bestMove.Index]);
                }
            g.DrawString ($"Episode {Episode}\r\nStep {LocalStep}\r\nGlobal step {GlobalStep}",
                Font, Brushes.Black, Width * Cell, Height * Cell);
        }
        private const int Cell = 50;
        private static readonly Pen arrowPen = new Pen (Brushes.Black, 3);
        private void FillCell (Graphics g, Pos pos) {
            if (pos == Player || pos == Field.Goal) {
                g.FillRectangle (pos == Player ? Brushes.Yellow : Brushes.Green, pos.Col * Cell, pos.Row * Cell, Cell, Cell);
                return;
            }

            g.TranslateTransform (pos.Col * Cell + Cell / 2, pos.Row * Cell + Cell / 2);
            g.ScaleTransform (Cell / 2, Cell / 2);
            foreach (IDir dir in Dirs.All)
                using (Brush fill = BrushForValue (StateActionValues[pos.Row, pos.Col, dir.Index]))
                    g.FillPolygon (fill, dir.Area);
            g.ResetTransform ();
        }
        private static readonly FloatColor red = new FloatColor (1, 0, 0);
        private static readonly FloatColor blue = new FloatColor (0, 0, 1);
        private static readonly FloatColor grey = new FloatColor (0.5f, 0.5f, 0.5f);
        private Brush BrushForValue (float value) {
            if (value > 0)
                return FloatColor.Mix (red, grey, value).ToBrush ();
            else
                return FloatColor.Mix (blue, grey, -value).ToBrush ();
        }
        private void DrawArrow (Graphics g, Pos pos, IDir dir, float score) {
            g.TranslateTransform (pos.Col * Cell + Cell / 2, pos.Row * Cell + Cell / 2);
            g.RotateTransform (dir.Degrees + 180);

            float length = Cell / 2;
            g.DrawLine (arrowPen, 0, -length, 0, length);
            g.DrawLine (arrowPen, -length / 4, length * 0.75f, 0, length);
            g.DrawLine (arrowPen, length / 4, length  * 0.75f, 0, length);

            g.ResetTransform ();
            g.DrawString ($"{score:F2}", Font, Brushes.Black, pos.Col * Cell, pos.Row * Cell);
        }

        public void Step () {
            Pos lastPos = Player;
            IDir lastMove = NextMove;
            float reward = Move (NextMove);

            NextMove = ChooseAction ();
            UpdateValues (lastPos, lastMove, reward, Player, NextMove);
            LocalStep++;
            GlobalStep++;

            if (IsGoal (Player)) {
                Player = Field.Start;
                NextMove = ChooseAction ();
                LocalStep = 0;
                Episode++;
            }
        }
        private float Move (IDir dir) {
            int windStrength = Field.Wind[Player.Col];
            Player = (Player + dir).ClampTo (Field);
            Player = (Player + new Pos (-windStrength, 0)).ClampTo (Field);
            return IsGoal (Player) ? 0f : -1f;
        }
        private const float ExploreChance = 0.1f;
        private IDir ChooseAction () {
            if (Rng.Random.NextDouble () < ExploreChance)
                return Dirs.All[Rng.Random.Next (Dirs.Count)];
            return BestMove (Player);
        }
        private IDir BestMove (Pos pos) =>
            Dirs.All[Dirs.All
                .Select(dir => StateActionValues[pos.Row, pos.Col, dir.Index])
                .ArgMax ()];
        // SARSA
        private const float LearningStep = 0.1f;
        private const float FutureWeight = 1f;
        private void UpdateValues (Pos lastPos, IDir lastMove, float reward, Pos newPos, IDir nextMove) {
            float newStateActionScore = IsGoal (newPos) ? 0f : StateActionValues[newPos.Row, newPos.Col, nextMove.Index];
            float oldStateActionScore = StateActionValues[lastPos.Row, lastPos.Col, lastMove.Index];

            StateActionValues[lastPos.Row, lastPos.Col, lastMove.Index] +=
                LearningStep * (reward + FutureWeight * newStateActionScore - oldStateActionScore);
        }
        private bool IsGoal (Pos pos) => pos == Field.Goal;
        private float[] MoveValues (Pos pos) =>
             Dirs.All
                .Select (dir => StateActionValues[pos.Row, pos.Col, dir.Index])
                .ToArray ();
    }
}
