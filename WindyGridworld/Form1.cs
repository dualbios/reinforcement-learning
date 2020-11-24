using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindyGridworld {
    public partial class Form1 : Form {
        internal Game Game { get; }

        public Form1 () {
            InitializeComponent ();

            Game = new Game (new Dirs8 (), Font);
        }

        private void Form1_Paint (object sender, PaintEventArgs e) => Game.Draw (e.Graphics);

        private void gameTimer_Tick (object sender, EventArgs e) {
            Game.Step ();
            Invalidate ();
        }
    }
}
