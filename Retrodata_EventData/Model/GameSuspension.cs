using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    // suspension of game due to weather or other conditions
    class GameSuspension
    {
        public string GameID { get; private set; }
        public int Inning { get; private set; }
        public int Sequence { get; private set; }
        public DateTime CompletionDate { get; private set; }
        //key BallPark.ID
        public int BallParkID { get; private set; }
        public int VisitorTeamScore { get; private set; }
        public int HomeTeamScore { get; private set; }
        public int GameOuts { get; private set; }
    }
}
