using Gates;
using LearningStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Snake {
    class SarsaBrain : IBrain {
        private const float Alpha_LearningRate = 0.02f;
        private const float Gamma_FutureDiscount = 0.9f;
        private const float Epsilon_Greed = 0.05f;
        private const int Sensors = 3 * 8;  // 24
        private const int Hiddens = 100;
        private ComputationalNetwork valueFunction;

        private IReadOnlyList<float> oldState;
        private int lastAction;
        private IReadOnlyList<float> newState;
        private int nextAction;

        private float oldStateValue;

        public SarsaBrain () {
            valueFunction = BuildNetwork ();
        }
        private ComputationalNetwork BuildNetwork () {
            ComputationalNetwork net = new ComputationalNetwork (
                "value function", new StochasticGradientAscent (new HyperParameters (0f, Alpha_LearningRate)));
            IGate last = net.AddInput ("input", new InputGate (TensorSize.Column (Sensors)));
            last = net.AddHidden ("fc1", new FullyConnectedGate (last, Sensors, Hiddens, afterRelu: false));
            last = net.AddHidden ("relu", new RectifierGate (last));
            last = net.AddOutput ("fc2", new FullyConnectedGate (last, Hiddens, 1, afterRelu: true));
            net.InitParameters ();
            return net;
        }

        public void NextEpisode (Game state) {
            oldState = state.GatherSensors ();
            lastAction = EpsilonGreedyAction (state);
        }

        public int ChooseLastAction (Game state) =>
            lastAction;

        public int ChooseNextAction (Game state) {
            newState = state.GatherSensors ();
            nextAction = EpsilonGreedyAction (state);
            return nextAction;
        }
        private int EpsilonGreedyAction (Game state) {
            int actions = Pos.Dir4.Count;

            if (Rng.Float () < Epsilon_Greed)
                return Rng.IntEx (actions);

            float[] values = new float[actions];
            foreach ((int i, Pos dir) in Pos.Dir4.WithIndex ()) {
                (float _, Game afterState) = state.TakeAction (dir);
                if (afterState.IsTerminal)
                    continue;  // value is 0 by definition
                values[i] = ValueFunction (afterState.GatherSensors ());
            }
            int best = values.IndexOfMax ();
            return best;
        }

        private float ValueFunction (IReadOnlyList<float> sensors) =>
            valueFunction.Forward (new[] { Tensor.Column (sensors) }).ToScalar ();

        public void Correct (float reward) {
            // Sarsa here
            float newStateValue = ValueFunction (newState);
            float oldStateValue = ValueFunction (oldState);  // side effect: prepare for learning
            float delta = reward + Gamma_FutureDiscount * oldStateValue - newStateValue;

            valueFunction.ScalarBackward (1f);
            //foreach ((IParameterGate gate, Tensor gradient) in valueFunction.ParameterGradients)
            //    gate.Output += Alpha_LearningRate * delta * gradient;
            valueFunction.LearnAfterBackpropagation ();

            oldState = newState;
            lastAction = nextAction;
            this.oldStateValue = newStateValue;
            newState = null;
            nextAction = -1;
        }

        public string GetStatisticsString () =>
            $"Value = {oldStateValue}\r\nAction = {lastAction}";
    }
}
