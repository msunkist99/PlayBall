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
    public class StartingPlayerPersist
    {
        public static void CreateStartingPlayer(StartingPlayerDTO startingPlayerDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var startingPlayer = convertToEntity(startingPlayerDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Starting_Player.Add(startingPlayer);
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

        private static Starting_Player convertToEntity(StartingPlayerDTO startingPlayerDTO)
        {
            var startingPlayer = new Starting_Player();

            startingPlayer.record_id = startingPlayerDTO.RecordID;
            startingPlayer.game_id = startingPlayerDTO.GameID;
            startingPlayer.player_id = startingPlayerDTO.PlayerID;
            startingPlayer.game_team_code = startingPlayerDTO.GameTeamCode;
            startingPlayer.batting_order = startingPlayerDTO.BattingOrder;
            startingPlayer.field_position = startingPlayerDTO.FieldPosition;

            return startingPlayer;
        }
    }
}
