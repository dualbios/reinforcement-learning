using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake {
    public partial class Form1 : Form {
        private Game game = Game.NewRandomGame ();
        private float speed = 1;
        private int frameSkip = 0;

        private DateTime started = DateTime.Now;
        private int globalStep = 0;

        public Form1 () {
            InitializeComponent ();
        }

        private void timer_Tick (object sender, EventArgs e) {
            globalStep++;
            for (int i = 0; i < frameSkip + 1; i++)
                Game.Step (ref game);
            Invalidate ();
        }

        private void Form1_Paint (object sender, PaintEventArgs e) =>
            game.Draw (e.Graphics, GetStatisticsString ());
        private string GetStatisticsString () {
            TimeSpan spent = DateTime.Now - started;
            string spentString = spent.TotalHours >= 1 ? $"{spent:hh\\:mm\\:ss}" : $"{spent:m\\:ss}";
            return $"{spentString}\r\nStep: {globalStep}";
        }

        private void Form1_KeyPress (object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '=' || e.KeyChar == '+')
                SetSpeed (speed * 1.1f);
            else if (e.KeyChar == '-' || e.KeyChar == '_')
                SetSpeed (speed / 1.1f);
        }
        private void SetSpeed (float speed) {
            this.speed = speed;
            float interval = 100f / speed;
            timer.Interval = (int) interval;
            if (interval >= 1)
                frameSkip = 0;
            else
                frameSkip = (int) (speed / 100);
        }
    }
}
