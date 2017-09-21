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
    public class PlayPersist
    {
        public static void CreatePlay(PlayDTO playDTO)
        {

            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var play = convertToEntity(playDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Plays.Add(play);
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

        private static Play convertToEntity(PlayDTO playDTO)
        {
            var play = new Play();

            play.record_id = playDTO.RecordID;
            play.game_id = playDTO.GameID;
            play.inning = playDTO.Inning;
            play.game_team_code = playDTO.GameTeamCode;
            play.sequence = playDTO.Sequence;
            play.player_id = playDTO.PlayerID;
            play.count_balls = playDTO.CountBalls;
            play.count_strikes = playDTO.CountStrikes;
            play.pitches = playDTO.Pitches;
            play.event_sequence = playDTO.EventSequence;
            play.event_modifier = playDTO.EventModifier;
            play.event_runner_advance = playDTO.EventRunnerAdvance;
            play.event_hit_location = playDTO.EventHitLocation;
            play.event_fielded_by = playDTO.EventFieldedBy;
            play.event_play_on_runner = playDTO.EventPlayOnRunner;
            play.event_type = playDTO.EventType;
            play.event_columnSix = playDTO.EventColumnSix;

            return play;
        }
    }
}
