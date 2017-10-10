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
        public class UmpireInformation
        {
            public PersonnelDTO UmpirePersonel = new PersonnelDTO();
            public string UmpireRole { get; set; }
        }

        public class ReferenceData
        {
            public Guid recordId { get; set; }
            public string referenceDataType { get; set; }
            public string referenceDataCode { get; set; }
            public string referenceDataDesc { get; set; }
        }

        public class GameInformationItem
        {
            public string GameItemDesc { get; set; }
            public string GameItemValue { get; set; }
        }

        public class PlayerInformation
        {

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
            public System.Guid RecordID { get; set; }
            public string GameID { get; set; }
            // key player.ID
            public string PlayerID { get; set; }

            // home or visiting
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int BattingOrder { get; set; }
            public string BattingOrderDesc { get; set; }

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

        public class PlayerRosterInformationByEvent
        {
            public int EventNum { get; set; }
            public PlayerRosterInformation playerRosterInformation { get; set; }
        }

        public class PlayerRosterInformation
        {
            public string PlayerID { get; set; }
            public int GameTeamCode { get; set; }
            public int BattingOrder { get; set; }
        }

        public class BallparkInformation
        {
            public BallparkDTO Ballpark = new BallparkDTO();
            public string BallparkLeagueDesc { get; set; }
        }

        public class PlayInformation
        {
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
            public string PlayerName { get; set; }
            public int CountBalls { get; set; }
            public int CountStrikes { get; set; }
            public string Count { get; set; }
            public string Pitches { get; set; }
            public string EventSequence { get; set; }
            public string EventModifier { get; set; }
            public string EventRunnerAdvance { get; set; }
            public string EventHitLocation { get; set; }

            public string PitchDesc { get; set; }
            public string EventHitLocationDesc { get; set; }
            public string EventSequenceDesc { get; set; }
            public string EventModifierDesc { get; set; }
            public string EventRunnerAdvanceDesc { get; set; }
            public string TeamID { get; set; }
            public string TeamName { get; set; }

            public string EventFieldedBy { get; set; }
            public string EventFieldedByDesc { get; set; }
            public string EventPlayOnRunner { get; set; }
            public string EventPlayOnRunnerDesc { get; set; }
            public string EventType { get; set; }
            public string EventTypeDesc { get; set; }
            public string EventColumnSix { get; set; }
            public string EventHitLocationDiagram { get; set; }
        }

        public class PlayBeventInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }
            public int Inning { get; set; }
            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int EventNum { get; set; }

            public string TopBottom { get; set; }

            public string BatterPlayerID { get; set; }
            public string BatterLastName { get; set; }
            public string BatterFirstName { get; set; }
            public string BatterName { get; set; }
            public int BatterDefPosition { get; set; }
            public string BatterDefPositionDesc { get; set; }
            public int BatterLineUpPosition { get; set; }

            public int CountBalls { get; set; }
            public int CountStrikes { get; set; }
            public string CountDesc { get; set; }
            public int CountOuts { get; set; }

            public string Pitches { get; set; }
            public string PitchDesc { get; set; }

            public string BattedBallType { get; set; }
            public string BattedBallTypeDesc { get; set; }

            public int DestBatter { get; set; }
            public int DestFirstRunner { get; set; }
            public int DestSecondRunner { get; set; }
            public int DestThirdRunner { get; set; }
            public string DestBatterDesc { get; set; }
            public string DestFirstRunnerDesc { get; set; }
            public string DestSecondRunnerDesc { get; set; }
            public string DestThirdRunnerDesc { get; set; }

            public string HitLocation { get; set; }
            public string HitLocationDesc { get; set; }

            public string TeamID { get; set; }
            public string TeamName { get; set; }

            public int FieldedBy { get; set; }
            public string FieldedByDesc { get; set; }

            public string PlayOnBatter { get; set; }
            public string PlayOnFirstRunner { get; set; }
            public string PlayOnSecondRunner { get; set; }
            public string PlayOnThirdRunner { get; set; }
            public string PlayOnBatterDesc { get; set; }
            public string PlayOnFirstRunnerDesc { get; set; }
            public string PlayOnSecondRunnerDesc { get; set; }
            public string PlayOnThirdRunnerDesc { get; set; }

            public string EventType { get; set; }
            public string EventTypeDesc { get; set; }

            public string PlayDetails { get; set; }
            public string DestinationDetails { get; set; }
            public string DestinationRunnersOnBaseDiagram { get; set; }
            public string HitLocationDiagram { get; set; }
            public string PlayOnRunnerDetails { get; set; }

            public int FirstErrorPosition { get; set; }
            public string FirstErrorPositionDesc { get; set; }
            public string FirstErrorType { get; set; }
            public string FirstErrorTypeDesc { get; set; }

            public int SecondErrorPosition { get; set; }
            public string SecondErrorPositionDesc { get; set; }
            public string SecondErrorType { get; set; }
            public string SecondErrorTypeDesc { get; set; }

            public int ThirdErrorPosition { get; set; }
            public string ThirdErrorPositionDesc { get; set; }
            public string ThirdErrorType { get; set; }
            public string ThirdErrorTypeDesc { get; set; }
            public string ErrorDetails { get; set; }

            public string RunnerFirstRemovedForPinchPlayerID { get; set; }
            public string RunnerFirstRemovedForPinchLastName { get; set; }
            public string RunnerFirstRemovedForPinchFirstName { get; set; }
            public string RunnerFirstRemovedForPinchBats { get; set; }
            public string RunnerFirstRemovedForPinchThrows { get; set; }

            public string RunnerSecondRemovedForPinchPlayerID { get; set; }
            public string RunnerSecondRemovedForPinchLastName { get; set; }
            public string RunnerSecondRemovedForPinchFirstName { get; set; }
            public string RunnerSecondRemovedForPinchBats { get; set; }
            public string RunnerSecondRemovedForPinchThrows { get; set; }

            public string RunnerThirdRemovedForPinchPlayerID { get; set; }
            public string RunnerThirdRemovedForPinchLastName { get; set; }
            public string RunnerThirdRemovedForPinchFirstName { get; set; }
            public string RunnerThirdRemovedForPinchBats { get; set; }
            public string RunnerThirdRemovedForPinchThrows { get; set; }

            public string BatterRemovedForPinchPlayerID { get; set; }
            public string BatterRemovedForPinchLastName { get; set; }
            public string BatterRemovedForPinchFirstName { get; set; }
            public string BatterRemovedForPinchBats { get; set; }
            public string BatterRemovedForPinchThrows { get; set; }

            public int BatterRemovedForPinchFieldPosition { get; set; }
            public string BatterRemovedForPinchFieldPositionDesc { get; set; }

            public int VisitingTeamScore { get; set; }
            public int HomeTeamScore { get; set; }

            public int HitValue { get; set; }
            public int NumErrors { get; set; }

            public int VistingTeamHitCount { get; set; }
            public int VisitingTeamErrorCount { get; set; }

            public int HomeTeamHitCount { get; set; }
            public int HomeTeamErrorCount { get; set; }
        }

        public class GameInformation
        {
            public Guid RecordID { get; set; }
            public string GameID { get; set; }

            public string VisitingTeamID { get; set; }
            public string VisitTeamLeague { get; set; }
            public string VisitTeamLeagueName { get; set; }
            public string VisitTeamName { get; set; }
            public string VisitTeamCity { get; set; }

            public string HomeTeamID { get; set; }
            public string HomeTeamLeague { get; set; }
            public string HomeTeamLeagueName { get; set; }
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
            public string Precipitation { get; set; }
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
            public string SeasonGameTypeDesc { get; set; }
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
            public int Inning { get; set; }
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
            public string UmpireName { get; set; }
            public string BallparkID { get; set; }
            public string BallparkName { get; set; }
            public string BallparkAKA { get; set; }
            public string Reason { get; set; }
            public string ReasonDesc { get; set; }
            public string Reversed { get; set; }
            public string ReversedDesc { get; set; }
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

        public class PlayerLineUpInformation
        {
            public string GameID { get; set; }
            public string PlayerID { get; set; }

            public int GameTeamCode { get; set; }
            public string GameTeamCodeDesc { get; set; }
            public int BattingOrder { get; set; }

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

            public int EventNum { get; set; }

        }

        public class PlayerGameLineupInformation
        {
            public int EventNum { get; set; }
            public List<PlayerLineUpInformation> VisitingPlayerLineupInformation { get; set; }
            public List<PlayerLineUpInformation> HomePlayerLineupInformation { get; set; }
        }

        public class BeventPlayer
        {
            public int BattingOrder { get; set; }
            public string Name { get; set; }
            public string FieldPositionDesc { get; set; }
            public string BatsThrows { get; set; }
        }

        public class BeventCounts
        {
            public string TeamName { get; set; }
            public string Inning { get; set; }
            public int Score { get; set; }
            public int Hits { get; set; }
            public int Errors { get; set; }
        }
    }
}


