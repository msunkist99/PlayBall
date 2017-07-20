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
    public class PitcherAdjustmentPersist
    {
        public static void CreatePitcherAdjustment(PitcherAdjustmentDTO pitcherAdjustmentDTO)
        {

            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var pitcherAdjustment = convertToEntity(pitcherAdjustmentDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Pitcher_Adjustment.Add(pitcherAdjustment);
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

        private static Pitcher_Adjustment convertToEntity(PitcherAdjustmentDTO pitcherAdjustmentDTO)
        {
            var pitcherAdjustment = new Pitcher_Adjustment();

            pitcherAdjustment.record_id = pitcherAdjustmentDTO.RecordID;
            pitcherAdjustment.game_id = pitcherAdjustmentDTO.GameID;
            pitcherAdjustment.inning = pitcherAdjustmentDTO.Inning;
            pitcherAdjustment.game_team_code = pitcherAdjustmentDTO.GameTeamCode;
            pitcherAdjustment.sequence = pitcherAdjustmentDTO.Sequence;
            pitcherAdjustment.player_id = pitcherAdjustmentDTO.PlayerID;
            pitcherAdjustment.pitcher_hand = pitcherAdjustmentDTO.PitcherHand;

            return pitcherAdjustment;
        }
    }
}
