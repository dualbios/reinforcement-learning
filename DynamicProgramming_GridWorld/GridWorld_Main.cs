using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming_GridWorld {
    class GridWorld_Main {
        private const int Iterations = 10;

        static void Main (string[] args) => new GridWorld_Main ().Run ();

        private Board<float> value;
        private Board<DirectionDistribution> policy;

        private void Run () {
            value = new Board<float> (() => 0f);
            policy = new Board<DirectionDistribution> (() => new DirectionDistribution (dir => 0.25f));

            value.Print ();
            policy.Print ();

            for (int i = 0; i < Iterations; i++) {
                value = NextValue ();
                value.Print ();
                policy = GreedyPolicy ();
                policy.Print ();
            }
        }
        private Board<float> NextValue () {
            float[,] future = new float[Pos.Rows, Pos.Cols];
            for (int i = 0; i < Pos.Rows; i++)
                for (int j = 0; j < Pos.Cols; j++) {
                    Pos pos = new Pos (i, j);
                    if (pos.IsFinal)
                        continue;
                    float expectation = 0;
                    foreach (Direction dir in Direction.All) {
                        Pos newPos = (pos + dir).Limit ();
                        expectation += (-1 + value[newPos]) * policy[pos][dir];
                    }
                    future[i, j] = expectation;
                }
            return new Board<float> (future);
        }
        private Board<DirectionDistribution> GreedyPolicy () {
            DirectionDistribution[,] future = new DirectionDistribution[Pos.Rows, Pos.Cols];
            for (int i = 0; i < Pos.Rows; i++)
                for (int j = 0; j < Pos.Cols; j++) {
                    Pos pos = new Pos (i, j);
                    if (pos.IsFinal) {
                        future[i, j] = new DirectionDistribution (dir => 0f);
                        continue;
                    }
                    IList<Direction> argMax = new List<Direction> ();
                    float max = float.NegativeInfinity;
                    foreach (Direction dir in Direction.All) {
                        Pos newPos = (pos + dir).Limit ();
                        float dirValue = value[newPos];
                        if (dirValue > max) {
                            argMax.Clear ();
                            argMax.Add (dir);
                            max = dirValue;
                        }
                        else if (dirValue == max)
                            argMax.Add (dir);
                    }
                    float evenlyDistributed = 1f / argMax.Count ();
                    future[i, j] = new DirectionDistribution (dir => argMax.Contains (dir) ? evenlyDistributed : 0f);
                }
            return new Board<DirectionDistribution> (future);
        }
    }
}
