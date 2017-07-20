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
    public class GameInfoPersist
    {
        public static void CreateGameInfo(GameInfoDTO gameInfoDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var gameInfo = convertToEntity(gameInfoDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Game_Info.Add(gameInfo);
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

        private static Game_Info convertToEntity(GameInfoDTO gameInfoDTO)
        {
            var gameInfo = new Game_Info();

            gameInfo.record_id = gameInfoDTO.RecordID;
            gameInfo.game_id = gameInfoDTO.GameID;
            gameInfo.game_info_type = gameInfoDTO.GameInfoType;
            gameInfo.game_info_value = gameInfoDTO.GameInfoValue;

            return gameInfo;
        }

    }
}
