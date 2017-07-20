using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_EventData.Model;
namespace Retrosheet_Persist
{
    public class TeamPersist
    {
        public static void CreateTeam(TeamDTO teamDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var team = convertToEntity(teamDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Teams.Add(team);
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

        private static Team convertToEntity(TeamDTO teamDTO)
        {
            var team = new Team();

            team.record_id = teamDTO.RecordID;
            team.team_id = teamDTO.ID;
            team.league = teamDTO.League;
            team.city = teamDTO.City;
            team.name = teamDTO.Name;

            return team;
        }
    }
}
