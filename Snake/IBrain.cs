using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake {
    interface IBrain {
        void NextEpisode (Game state);

        int ChooseLastAction (Game state);
        int ChooseNextAction (Game state);
        void Correct (float reward);

        void AteApple ();

        IReadOnlyList<string> GetStatisticsStrings ();
    }
}
