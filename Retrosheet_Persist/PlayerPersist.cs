using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_ReferenceData.Model;
using Retrosheet_EventData.Model;

namespace Retrosheet_Persist
{
    public class PlayerPersist
    {
        public static void CreatePlayer(PlayerDTO playerDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var player = convertToEntity(playerDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Players.Add(player);
            try
            {
                dbCtx.SaveChanges();
            }
            catch (Exception e)
            {
                string text;
                text = e.Message;
            }
        }

        private static Player convertToEntity(PlayerDTO playerDTO)
        {
            var player = new Player();

            player.record_id = playerDTO.RecordID;
            player.player_id = playerDTO.PlayerID;
            player.last_name = playerDTO.LastName;
            player.first_name = playerDTO.FirstName;
            player.throws = playerDTO.Throws;
            player.bats = playerDTO.Bats;
            player.team_id = playerDTO.TeamID;
            player.field_position = playerDTO.Position;

            return player;
        }
    }
}
