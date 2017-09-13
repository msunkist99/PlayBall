using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_EventData.Model;
using Retrosheet_ReferenceData.Model;

namespace Retrosheet_RetrieveData
{
    public class DataModels
    {
        public class xGameInformation : GameInformationDTO
        {
            public BallparkInformation Ballpark = new BallparkInformation();
            public TeamInformation VisitingTeam = new TeamInformation();
            public TeamInformation HomeTeam = new TeamInformation();
            public UmpireInformation UmpireHome = new UmpireInformation();
            public UmpireInformation UmpireFirst = new UmpireInformation();
            public UmpireInformation UmpireSecond = new UmpireInformation();
            public UmpireInformation UmpireThird = new UmpireInformation();
            public PlayerInformation WinningPitcher = new PlayerInformation();
            public PlayerInformation LosingPitcher = new PlayerInformation();
            public PlayerInformation SavePitcher = new PlayerInformation();
            public PlayerInformation WinningRBIPlayer = new PlayerInformation();

            public string GameNumberDesc { get; set; }
            public string WindDirectionDesc { get; set; }
            public string SeasonGameTypeDesc { get; set; }
        }

        public class UmpireInformation
        {
            public PersonnelDTO UmpirePersonel = new PersonnelDTO();
            public string UmpireRole { get; set; }
        }

        public class ReferenceData
        {
            //public ReferenceDataDTO ReferenceItem = new ReferenceDataDTO();
            public Guid recordId { get; set; }
            public string referenceDataType { get; set; }
            public string referenceDataCode { get; set; }
            public string referenceDataDesc { get; set; }
        }

        public class PlayerInformation
        {
            //public PlayerDTO Player = new PlayerDTO();
            //public string GameID { get; set; }

            public System.Guid RecordID { get; set; }
            public string PlayerID { get; set; }
            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public string PlayerThrows { get; set; }
            public string PlayerThrowsDesc { get; set; }
            public string PlayerBats { get; set; }
            public string PlayerBatsDesc { get; set; }

            // key to Team.ID
            public string PlayerTeamID { get; set; }
            public string PlayerTeamName { get; set; }

            // field position reference data available
            public string PlayerFieldPosition { get; set; }
            public string PlayerFieldPositionDesc { get; set; }

            public string SeasonYear { get; set; }
            public string SeasonGameType { get; set; }

        }

        public class StartingPlayerInformation
        {
            //public StartingPlayerDTO StartingPlayer = new StartingPlayerDTO();
            public System.Guid RecordID { get; set; }
            public string GameID { get; set; }
            // key player.ID
            public string PlayerID { get; set; }

            // home or visiting
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int BattingOrder { get; set; }

            // may have to cast from int to string to access reference value
            public int FieldPosition { get; set; }
            public string FieldPositionDesc { get; set; }
            public string TeamID { get; set; }
            public string TeamName { get; set; }

            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public string Throws { get; set; }
            public string ThrowsDesc { get; set; }
            public string Bats { get; set; }
            public string BatsDesc { get; set; }
        }

        public class TeamInformation
        {
            public TeamDTO Team = new TeamDTO();
            public string TeamLeagueDesc { get; set; }
        }

        public class BallparkInformation
        {
            public BallparkDTO Ballpark = new BallparkDTO();
            public string BallparkLeagueDesc { get; set; }
        }

        public class PlayInformation
        {
            //public PlayDTO Play = new PlayDTO();

            public System.Guid RecordID { get; set; }

            public string GameID { get; set; }
            public int Inning { get; set; }
            // home or visiting
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int Sequence { get; set; }
            // key player.ID
            public string PlayerID { get; set; }
            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public int CountBalls { get; set; }
            public int CountStrikes { get; set; }
            public string Pitches { get; set; }
            public string EventSequence { get; set; }
            public string EventModifier { get; set; }
            public string EventRunnerAdvance { get; set; }
            public string EventHitLocation { get; set; }

            public string PitchDesc { get; set; }
            public string EventHitLocationDesc { get; set; }
            public string EventSequenceDesc { get; set; }
            public string EventSequenceModifierDesc { get; set; }
            public string EventRunnerAdvanceDesc { get; set; }
            public string TeamID { get; set; }
            public string TeamName { get; set; }

        }

        public class GameInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }

            public string VisitingTeamID { get; set; }
            public string VisitTeamLeague { get; set; }
            public string VisitTeamName { get; set; }
            public string VisitTeamCity { get; set; }

            public string HomeTeamID { get; set; }
            public string HomeTeamLeague { get; set; }
            public string HomeTeamName { get; set; }
            public string HomeTeamCity { get; set; }

            public DateTime GameDate { get; set; }
            public int GameNumber { get; set; }
            public string GameNumberDesc { get; set; }

            public string StartTime { get; set; }
            public string DayNight { get; set; }
            public string UsedDH { get; set; }
            public string Pitches { get; set; }

            public string UmpireHomeID { get; set; }
            public string UmpireHomeLastName { get; set; }
            public string UmpireHomeFirstName { get; set; }

            public string UmpireFirstBaseID { get; set; }
            public string UmpireFirstBaseLastName { get; set; }
            public string UmpireFirstBaseFirstName { get; set; }

            public string UmpireSecondBaseID { get; set; }
            public string UmpireSecondBaseLastName { get; set; }
            public string UmpireSecondBaseFirstName { get; set; }

            public string UmpireThirdBaseID { get; set; }
            public string UmpireThirdBaseLastName { get; set; }
            public string UmpireThirdBaseFirstName { get; set; }

            public string FieldCondition { get; set; }
            public string Recipitation { get; set; }
            public string Sky { get; set; }
            public int Temperature { get; set; }
            public string WindDirection { get; set; }
            public string WindDirectionDesc { get; set; }
            public int WindSpeed { get; set; }
            public int GameTimeLengthMinutes { get; set; }
            public int Attendance { get; set; }

            public string BallparkID { get; set; }
            public string BallparkAKA { get; set; }
            public string BallparkCity { get; set; }
            public string BallparkName { get; set; }
            public string BallparkNotes { get; set; }

            public string WinningPitcherID { get; set; }
            public string LosingPitcherID { get; set; }
            public string SavePitcherID { get; set; }
            public string WinningRBIPlayerID { get; set; }

            public string Oscorer { get; set; }
            public string SeasonYear { get; set; }
            public string SeasonGameType { get; set; }
            public string EditTime { get; set; }
            public string HowScored { get; set; }
            public string InputProgVers { get; set; }
            public string Inputter { get; set; }
            public string InputTime { get; set; }
            public string Scorer { get; set; }
            public string Translator { get; set; }
        }

        public class BatterAdjustmentInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public int Inning { get; set; }
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int Sequence { get; set; }
            public string PlayerID { get; set; }
            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public string PlayerBats { get; set; }
            public string BatsDesc { get; set; }
            public string TeamID { get; set; }
            public string TeamName { get; set; }
        }

        public class PlayerEjectionInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public int Inning { get; set; }
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int Sequence { get; set; }
            public int CommentSequence { get; set; }
            public string PlayerID { get; set; }
            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public string JobCode { get; set; }
            public string JobCodeDesc { get; set; }
            public string UmpireCode { get; set; }
            public string UmpireLastName { get; set; }
            public string UmpireFirstName { get; set; }
            public string UmpireRole { get; set; }
            public string Reason { get; set; }
        }

        public class SubstitutePlayerInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public int Inning { get;  set;}
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int Sequence { get; set; }
            public string PlayerID { get; set; }
            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public string PlayerThrows { get; set; }
            public string PlayerThrowsDesc { get; set; }
            public string PlayerBats { get; set; }
            public string PlayerBatsDesc { get; set; }
            public int BattingOrder { get; set; }
            public int FieldPosition { get; set; }
            public string FieldPositionDesc { get; set; }
            public string TeamID { get; set; }
            public string TeamName { get; set; }
        }

        public class GameCommentInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public int Inning { get; set; }
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int Sequence { get; set; }
            public int CommentSequence { get; set; }
            public string Comment { get; set; }
        }

        public class ReplayInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public int Inning { get; set; }
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int Sequence { get; set; }
            public int CommentSequence { get; set; }
            public string PlayerID { get; set; }
            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public string PlayerTeamID { get; set; }
            public string PlayerTeamName { get; set; }
            public string UmpireID { get; set; }
            public string UmpireLastName { get; set; }
            public string UmpireFirstName { get; set; }
            public string BallparkID { get; set; }
            public string BallparkName { get; set; }
            public string BallparkAKA { get; set; }
            public string Reason { get; set; }
            public string ReasonDesc { get; set; }
            public string Reversed { get; set; }
            public string Initiator { get; set; }
            public string InitiatorDesc { get; set; }
            public string InitiatorTeamID { get; set; }
            public string InitiatorTeamName { get; set; }
            public string Type { get; set; }
            public string TypeDesc { get; set; }

        }

        public class GameDataInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public string DataType { get; set; }
            public string DataTypeDesc { get; set; }
            public string PlayerID { get; set; }
            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public string DataValue { get; set; }
        }

        public class PitcherAdjustmentInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public int Inning { get; set; }
            public int GameTeamCode { get; set; }
            public int Sequence { get; set; }
            public string PlayerID { get; set; }
            public string PlayerLastName { get; set; }
            public string PlayerFirstName { get; set; }
            public string PlayerHand { get; set; }
            public string PlayerHandDesc { get; set; }
            public string TeamID { get; set; }
            public string TeamName { get; set; }
        }

        public class SubstituteUmpireInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public int Inning { get; set; }
            public int Sequence { get; set; }
            public int CommentSequence { get; set; }
            public string FieldPosition { get; set; }
            public string FieldPositionDesc { get; set; }
            public string UmpireID { get; set; }
            public string UmpireLastName { get; set; }
            public string UmpireFirstName { get; set; }

        }
    }

}


