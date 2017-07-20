using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class GameCommentDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get;  set; }
        public int Inning { get; set; }
        // home or visiting
        public int GameTeamCode { get; set; }

        public int Sequence { get; set; }
        public int CommentSequence { get; set; }
        public string Comment { get;  set; }
    }
}
