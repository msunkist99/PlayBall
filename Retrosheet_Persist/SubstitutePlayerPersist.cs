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
    public class SubstitutePlayerPersist
    {
        public static void CreateSubstitutePlayer(SubstitutePlayerDTO substitutePlayerDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var substitutePlayer = convertToEntity(substitutePlayerDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Substitute_Player.Add(substitutePlayer);
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

        private static Substitute_Player convertToEntity(SubstitutePlayerDTO substitutePlayerDTO)
        {
            var substitutePlayer = new Substitute_Player();

            substitutePlayer.record_id = substitutePlayerDTO.RecordID;
            substitutePlayer.game_id = substitutePlayerDTO.GameID;
            substitutePlayer.inning = substitutePlayerDTO.Inning;
            substitutePlayer.game_team_code = substitutePlayerDTO.GameTeamCode;
            substitutePlayer.sequence = substitutePlayerDTO.Sequence;
            substitutePlayer.player_id = substitutePlayerDTO.PlayerID;
            substitutePlayer.game_team_code = substitutePlayerDTO.GameTeamCode;
            substitutePlayer.batting_order = substitutePlayerDTO.BattingOrder;
            substitutePlayer.field_position = substitutePlayerDTO.FieldPosition;
            substitutePlayer.team_id = substitutePlayerDTO.TeamId;

            return substitutePlayer;
        }
    }
}
