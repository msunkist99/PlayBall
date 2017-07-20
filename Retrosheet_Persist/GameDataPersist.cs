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
    public class GameDataPersist
    {
        public static void CreateGameData(GameDataDTO gameDataDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var gameData = convertToEntity(gameDataDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Game_Data.Add(gameData);
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

        private static Game_Data convertToEntity(GameDataDTO gameDataDTO)
        {
            var gameData = new Game_Data();

            gameData.record_id = gameDataDTO.RecordID;
            gameData.game_id = gameDataDTO.GameID;
            gameData.data_type = gameDataDTO.DataType;
            gameData.player_id = gameDataDTO.PlayerID;
            gameData.data_value = gameDataDTO.DataValue;

            return gameData;
        }
    }
}
