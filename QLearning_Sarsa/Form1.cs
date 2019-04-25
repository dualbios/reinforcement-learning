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
        private new const float Scale = 60;
        private const float Explore = 0.1f;

        private QValues qValues = new QValues ();
        private StateValues stateValues = new StateValues ();

        private State wandering = State.Start;
        private AgentAction planAction;
        private float score, reward;
        private int episode;
        private bool greedyEpisode;
        private int speedFactor = 20;

        private Random random = new Random ();

        public Form1 () {
            InitializeComponent ();

            NewEpisode ();
        }

        private void NewEpisode () {
            wandering = State.Start;
            planAction = NextAction (wandering);
            score = 0;

            episode++;
            // greedyEpisode = episode % 20 == 0;
        }

        private void Form1_Paint (object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            for (int row = 0; row < State.Rows; row++)
                for (int col = 0; col < State.Cols; col++) {
                    g.ResetTransform ();
                    g.ScaleTransform (Scale, Scale);
                    g.TranslateTransform (col, row);

                    State drawing = new State (row, col);
                    Color color;
                    if (drawing == wandering && greedyEpisode)
                        color = Color.Yellow;
                    else if (drawing == wandering)
                        color = Color.Green;
                    else
                        color = stateValues.GetColor (drawing);
                    g.FillRectangle (new SolidBrush (color), 0, 0, 1, 1);
                }
        }

        private void timer_Tick (object sender, EventArgs e) {
            for (int i = 0; i < speedFactor; i++) {
                if (wandering.IsTerminal) {
                    NewEpisode ();
                    continue;
                }

                State nextPos = wandering + planAction;
                AgentAction nextAction = NextAction (nextPos);
                float reward = nextPos.IsOffCliff ? -100 : -1;
                score += reward;
                if (!greedyEpisode) {
                    qValues.SarsaUpdate (wandering, planAction, reward, nextPos, nextAction);
                    stateValues.TemporalDifferenceUpdate (wandering, planAction, reward, nextPos);
                }

                wandering = nextPos;
                planAction = nextAction;
                if (wandering.IsTerminal) {
                    Invalidate ();
                    return;
                }
            }
            Invalidate ();
        }

        private void Form1_KeyPress (object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '=')
                speedFactor++;
            else if (e.KeyChar == '-')
                speedFactor = Math.Max (1, speedFactor - 1);
        }

        private AgentAction NextAction (State from) {
            if (greedyEpisode)
                return qValues.Greedy (from);
            else if (random.NextDouble () < Explore)
                return AgentAction.Random ();
            else
                return qValues.Greedy (from);
        }
    }
}
