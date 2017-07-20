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
    public class EjectionPersist
    {
        public static void CreateEjection(EjectionDTO ejectionDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var ejection = convertToEntity(ejectionDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Ejections.Add(ejection);
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

        private static Ejection convertToEntity(EjectionDTO ejectionDTO)
        {
            var ejection = new Ejection();

            ejection.record_id = ejectionDTO.RecordID;
            ejection.game_id = ejectionDTO.GameID;
            ejection.inning = ejectionDTO.Inning;
            ejection.game_team_code = ejectionDTO.GameTeamCode;
            ejection.sequence = ejectionDTO.Sequence;
            ejection.comment_sequence = ejectionDTO.ComSequence;
            ejection.player_id = ejectionDTO.PlayerID;
            ejection.job_code = ejectionDTO.JobCode;
            ejection.umpire_id = ejectionDTO.UmpireID;
            ejection.reason = ejectionDTO.Reason;

            return ejection;
        }
    }
}
