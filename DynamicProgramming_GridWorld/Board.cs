using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming_GridWorld {
    class Board<T> {
        public int Rows => Pos.Rows;
        public int Cols => Pos.Cols;
        public T[, ] Cells { get; } = new T[Pos.Rows, Pos.Cols];

        public Board (Func<T> generate) {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    Cells[i, j] = generate ();
        }
        public Board (T[, ] values) {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    Cells[i, j] = values[i, j];
        }

        public T this[Pos pos] {
            get => Cells[pos.Row, pos.Col];
            set => Cells[pos.Row, pos.Col] = value;
        }

        public Board<R> Select<R> (Func<T, R> convert) {
            R[, ] result = new R[Rows, Cols];
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    result[i, j] = convert (Cells[i, j]);
            return new Board<R> (result);
        }

        public void Print () {
            for (int i = 0; i < Rows; i++) {
                for (int j = 0; j < Cols; j++) 
                    Console.Write ($"{Cells[i, j],4} ");
                Console.WriteLine ();
            }
        }
    }
}
