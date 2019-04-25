using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning_Sarsa {
    // Position on the board
    class State {
        public const int Rows = 10;
        public const int Cols = 10;

        public static State Start { get; } = new State (0, Cols - 1);
        public static State Goal { get; } = new State (Rows - 1, Cols - 1);

        public static IEnumerable<State> All =>
            Enumerable.Range (0, Rows)
                .SelectMany (row => Enumerable.Range (0, Cols)
                    .Select (col => new State (row, col)));

        public int Row { get; }
        public int Col { get; }

        public State (int row, int col) {
            Row = row;
            Col = col;
        }

        public bool IsOffCliff => Row == Rows - 1 && Cols > 0 && Cols < Cols - 1;
        public bool IsTerminal => Row == Rows - 1 && Cols > 0;

        public static State operator + (State a, State b) =>
            new State (
                Math.Max (0, Math.Min (Rows - 1, a.Row + b.Row)),
                Math.Max (0, Math.Min (Cols - 1, a.Col + b.Col)));

        public static bool operator == (State a, State b) =>
            a.Row == b.Row && a.Col == b.Col;
        public static bool operator != (State a, State b) => !(a == b);

        public override bool Equals (object obj) {
            if (obj is null || this.GetType () != obj.GetType ())
                return false;
            State that = (State) obj;
            return this.Row == that.Row && this.Col == that.Col;
        }
        public override int GetHashCode () => Row * Cols + Col;

        public override string ToString () => $"r={Row}, c={Col}";
    }
}
