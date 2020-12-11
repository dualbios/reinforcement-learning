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
        private const float Epsilon_Greed = 0.00f;
        private const float RegularizationWeight = 0.000f;
        private const int Sensors = 3 * 8;  // 24
        private const int Hiddens = 100;
        private ComputationalNetwork valueFunction;

        private IReadOnlyList<float> oldState;
        private int lastAction;
        private IReadOnlyList<float> newState;
        private int nextAction;

        private float oldStateValue;

        private IGate valueGate;

        public SarsaBrain () {
            valueFunction = BuildNetwork ();
        }
        private ComputationalNetwork BuildNetwork () {
            ComputationalNetwork net = new ComputationalNetwork (
                "value function", new StochasticGradientAscent (
                    new HyperParameters (RegularizationWeight, Alpha_LearningRate)));

            IGate last = net.AddInput ("input", new InputGate (TensorSize.Column (Sensors)));
            IGate inputReward = net.AddInput ("reward", new InputGate (TensorSize.Scalar));
            IGate inputNextStateValue = net.AddInput ("next state value", new InputGate (TensorSize.Scalar));
            last = net.AddHidden ("fc1", new FullyConnectedGate (last, Sensors, Hiddens, afterRelu: false));
            last = net.AddHidden ("relu", new RectifierGate (last));
            last = net.AddOutput ("fc2", new FullyConnectedGate (last, Hiddens, 1, afterRelu: true));
            valueGate = last;

            last = net.AddHidden ("gamma * V(last)", new ConstMultGate (last, Gamma_FutureDiscount));
            last = net.AddHidden ("R + gamma * V(last)", new AddGate (last, inputReward));
            last = net.AddHidden ("delta = R + gamma * V(last) - V(next)", new SubtractGate (last, inputNextStateValue));
            last = net.AddHidden ("delta^2", new SquareGate (last));
            last = net.AddHidden ("update Loss", new ConstMultGate (last, 0.5f));

            IGate reg = net.AddHidden ("weights squares sum", new L2RegularizationGate (net.ParameterGates));
            reg = net.AddHidden ("reg Loss", new ConstMultGate (reg, RegularizationWeight));

            last = net.AddOutput ("total Loss", new AddGate (last, reg));

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
                (float reward, Game afterState) = state.TakeAction (dir);
                if (afterState.IsTerminal) {
                    values[i] = reward;
                    continue;  // value is 0 by definition
                }
                values[i] = reward + ValueFunction (afterState.GatherSensors ());
            }
            int best = values.IndexOfMax ();
            return best;
        }

        private float ValueFunction (IReadOnlyList<float> sensors) =>
            valueFunction.PartialForward (new[] {
                Tensor.Column (sensors), Tensor.Scalar (0), Tensor.Scalar (0)
            }, valueGate, isExam: false).ToScalar ();
        private float ValueFunction_PrepareToLearn (IReadOnlyList<float> sensors, float reward, float newStateValue) {
            valueFunction.Forward (new[] {
                Tensor.Column (sensors), Tensor.Scalar (reward), Tensor.Scalar (newStateValue)
            });
            return valueGate.Output.ToScalar ();
        }

        public void Correct (float reward) {
            // Sarsa here
            float newStateValue = ValueFunction (newState);
            float oldStateValue = ValueFunction_PrepareToLearn (oldState, reward, newStateValue);

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
