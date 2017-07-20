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
    public class BatterAdjustmentPersist
    {
        public static void CreateBatterAdjustment(BatterAdjustmentDTO batterAdjustmentDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var batterAdjustment = convertToEntity(batterAdjustmentDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Batter_Adjustment.Add(batterAdjustment);
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

        private static Batter_Adjustment convertToEntity(BatterAdjustmentDTO batterAdjustmentDTO)
        {
            var batterAdjustment = new Batter_Adjustment();

            batterAdjustment.record_id = batterAdjustmentDTO.RecordID;
            batterAdjustment.game_id = batterAdjustmentDTO.GameID;
            batterAdjustment.inning = batterAdjustmentDTO.Inning;
            batterAdjustment.game_team_code = batterAdjustmentDTO.GameTeamCode;
            batterAdjustment.sequence = batterAdjustmentDTO.Sequence;
            batterAdjustment.player_id = batterAdjustmentDTO.PlayerID;
            batterAdjustment.bats = batterAdjustmentDTO.Bats;

            return batterAdjustment;
        }
    }
}
