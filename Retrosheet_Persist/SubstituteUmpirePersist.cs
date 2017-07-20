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
    public class SubstituteUmpirePersist
    {
        public static void CreateSubstituteUmpire(SubstituteUmpireDTO substituteUmpireDTO)
        {
            // ballpark instance of Umpire class in Retrosheet_Persist.Retrosheet
            var substituteUmpire = convertToEntity(substituteUmpireDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Substitute_Umpire.Add(substituteUmpire);
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

        private static Substitute_Umpire convertToEntity(SubstituteUmpireDTO substituteUmpireDTO)
        {
            var substituteUmpire = new Substitute_Umpire();

            substituteUmpire.record_id = substituteUmpireDTO.RecordID;
            substituteUmpire.game_id = substituteUmpireDTO.GameID;
            substituteUmpire.inning = substituteUmpireDTO.Inning;
            substituteUmpire.sequence = substituteUmpireDTO.Sequence;
            substituteUmpire.comment_sequence = substituteUmpireDTO.ComSequence;
            substituteUmpire.umpire_id = substituteUmpireDTO.UmpireID;
            substituteUmpire.field_position = substituteUmpireDTO.FieldPosition;

            return substituteUmpire;
        }
    }
}
