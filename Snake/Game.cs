using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Snake {
    class Game {
        public const int Scale = 20;
        public const int InitialFood = 400;
        public const int FoodPerApple = 200;

        public Field Field { get; }
        public Snake Snake { get; private set; }
        public Pos Apple { get; private set; }
        public IBrain Brain { get; }

        public int Height => Field.Height;
        public int Width => Field.Width;

        public int Age { get; private set; }
        public int AteApples { get; private set; }
        public int FoodRemaining { get; private set; }
        public bool IsTerminal { get; }

        public Game (
            Field field, Snake snake, Pos apple, IBrain brain,
            int age, int ateApples, int foodRemaining, bool isTerminal
        ) {
            Field = field;
            Snake = snake;
            Apple = apple;
            Brain = brain;
            Age = age;
            AteApples = ateApples;
            FoodRemaining = foodRemaining;
            IsTerminal = isTerminal;
        }
        public static Game NewRandomGame () {
            Field field = new Field ();
            Game game = new Game (
                field, Snake.Random (field), Pos.Random (field), new SarsaBrain (),
                0, 0, InitialFood, isTerminal: false
            );
            game.Reset ();
            return game;
        }

        public void Reset () {
            Apple = Pos.Random (Field);
            Snake = Snake.Random (Field);
            Brain.NextEpisode (this);

            Age = 0;
            AteApples = 0;
            FoodRemaining = InitialFood;
        }

        private Font font = new Font (new FontFamily ("Arial"), 18);
        public void Draw (Graphics g, string statistics) {
            for (int row = 0; row < Height; row++)
                for (int col = 0; col < Width; col++) {
                    Pos pos = new Pos (row, col);
                    if (pos == Snake.Head)
                        DrawCell (g, pos, Brushes.Green);
                    else if (Snake.Body.Contains (pos))
                        DrawCell (g, pos, Brushes.White);
                    else if (pos == Apple)
                        DrawCell (g, pos, Brushes.Red);
                    else
                        DrawCell (g, pos, Brushes.Black);
                }
            g.DrawString ($"{statistics}\r\n{GetStatisticsString ()}", font, Brushes.Yellow, new Point (0, 0));
        }
        private void DrawCell (Graphics g, Pos pos, Brush brush) =>
            g.FillRectangle (brush, pos.Col * Scale, pos.Row * Scale, Scale, Scale);
        private string GetStatisticsString () =>
            $"Age: {Age}\r\nFood: {FoodRemaining}\r\nApples: {AteApples}\r\n"
                + Brain.GetStatisticsString ();

        public static void Step (ref Game game) {
            int lastAction = game.Brain.ChooseLastAction (game);
            Pos dir = Pos.Dir4[lastAction];
            (float reward, Game afterstate) = game.TakeAction (dir);
            game = afterstate;
            int nextAction = game.Brain.ChooseNextAction (game);
            game.Brain.Correct (reward);
            if (game.IsTerminal || game.FoodRemaining < 0)
                game.Reset ();
        }

        public IReadOnlyList<float> GatherSensors () {
            int sensorKinds = 3;
            int dirs = Pos.Dir8.Count;
            int maxRange = Math.Max (Width, Height) + 1;
            const int WallSensor = 0;
            const int AppleSensor = 1;
            const int TailSensor = 2;

            float[, ] sensors = new float[sensorKinds, dirs];
            for (int i = 0; i < dirs; i++) {
                Pos dir = Pos.Dir8[i];

                bool foundWall = false, foundApple = false, foundTail = false;
                for (int distance = 1; distance <= maxRange; distance++) {
                    Pos pos = Snake.Head + distance * dir;
                    if (!foundWall && !Field.Contains (pos)) {
                        foundWall = true;
                        sensors[WallSensor, i] = 1f / distance;
                        break;
                    }
                    if (!foundApple && pos == Apple) {
                        foundApple = true;
                        sensors[AppleSensor, i] = 1f / distance;
                    }
                    if (!foundTail && Snake.Body.Contains (pos)) {
                        foundTail = true;
                        sensors[TailSensor, i] = 1f / distance;
                    }
                }
            }
            return Enumerable.Range (0, sensorKinds)
                .SelectMany (i => Enumerable.Range (0, dirs)
                    .Select (j => sensors[i, j]))
                .ToArray (sensorKinds * dirs);
        }

        public (float reward, Game afterstate) TakeAction (Pos dir) {
            Pos pos = Snake.Head + dir;
            Snake newSnake = Snake.MoveTo (pos, pos == Apple);
            if (!Field.Contains (pos) || Snake.Body.Contains (pos))
                return (reward: -1, new Game (Field, newSnake, Apple, Brain,
                    Age + 1, AteApples, FoodRemaining - 1, isTerminal: true));
            else if (pos == Apple)
                return (reward: 1, new Game (Field, newSnake, Pos.Random (Field), Brain,
                    Age + 1, AteApples + 1, FoodRemaining + FoodPerApple - 1, isTerminal: false));
            else
                return (reward: -0.01f, new Game (Field, newSnake, Apple, Brain,
                    Age + 1, AteApples, FoodRemaining - 1, isTerminal: false));
        }
    }
}
