using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLearning_Sarsa {
    public partial class Form1 : Form {
        private float Scale = 60;

        private QValues qValues = new QValues ();
        private StateValues stateValues = new StateValues ();

        public Form1 () {
            InitializeComponent ();
        }

        private void Form1_Paint (object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            for (int row = 0; row < State.Rows; row++)
                for (int col = 0; col < State.Cols; col++) {
                    g.ResetTransform ();
                    g.ScaleTransform (Scale, Scale);
                    g.TranslateTransform (col, row);
                    g.FillRectangle (new SolidBrush (stateValues.GetColor (new State (row, col))),
                        0, 0, 1, 1);
                }
        }
    }
}
