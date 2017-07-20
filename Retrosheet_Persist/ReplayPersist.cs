using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Retrosheet_EventData.Model;
namespace Retrosheet_Persist
{
    public class ReplayPersist
    {    
        public static void CreateReplay(ReplayDTO replayDTO)
        {

            // ballpark instance of Replayer class in Retrosheet_Persist.Retrosheet
            var replay = convertToEntity(replayDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Replays.Add(replay);
            try
            {
                dbCtx.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                string text;
                text = e.Message;
            }
        }

        private static Replay convertToEntity(ReplayDTO replayDTO)
        {
            var replay = new Replay();

            replay.record_id = replayDTO.RecordID;
            replay.game_id = replayDTO.GameID;
            replay.inning = replayDTO.Inning;
            replay.game_team_code = replayDTO.GameTeamCode;
            replay.sequence = replayDTO.Sequence;
            replay.comment_sequence = replayDTO.ComSequence;
            replay.player_id = replayDTO.PlayerID;
            replay.team_id = replayDTO.TeamID;
            replay.umpire_id = replayDTO.UmpireID;
            replay.ballpark_id = replayDTO.BallparkID;
            replay.reason = replayDTO.Reason;
            if (replayDTO.Reversed == true)
            {
                replay.reversed = "Y";
            }
            else
            {
                replay.reversed = "N";
            }
            replay.initiated = replayDTO.Initiated;
            replay.replay_challenge_team_id = replayDTO.ReplayChallengeTeamID;
            replay.type = replayDTO.Type;

            return replay;
        }
    }
}
