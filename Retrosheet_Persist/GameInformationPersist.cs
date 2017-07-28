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
    public class GameInformationPersist
    {
        public static void CreateGameInformation(GameInformationDTO gameInformationDTO)
        {
            // ballpark instance of Player class in Retrosheet_Persist.Retrosheet
            var gameInformation = convertToEntity(gameInformationDTO);

            // entity data model
            var dbCtx = new retrosheetDB();

            dbCtx.Game_Information.Add(gameInformation);
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

        private static Game_Information convertToEntity(GameInformationDTO gameInformationDTO)
        {
            var gameInformation = new Game_Information();

            gameInformation.record_id = gameInformationDTO.RecordID;
            gameInformation.game_id = gameInformationDTO.GameID;
            gameInformation.visiting_team_id = gameInformationDTO.VisitingTeam_ID;
            gameInformation.home_team_id = gameInformationDTO.HomeTeam_ID;
            gameInformation.game_date = gameInformationDTO.GameDate;
            gameInformation.game_number = gameInformationDTO.GameNumber;
            gameInformation.start_time = gameInformationDTO.StartTime;
            gameInformation.day_night = gameInformationDTO.DayNight;

            if (gameInformationDTO.UsedDH == true)
            {
                gameInformation.used_dh = "true";
            }
            else
            {
                gameInformation.used_dh = "false";
            }

            gameInformation.pitches = gameInformationDTO.Pitches;
            gameInformation.umpire_home_id = gameInformationDTO.UmpireHome_ID;
            gameInformation.umpire_first_base_id = gameInformationDTO.UmpireFirstBaseID;
            gameInformation.umpire_second_base_id = gameInformationDTO.UmpireSecondBaseID;
            gameInformation.umpire_third_base_id = gameInformationDTO.UmpireThirdBaseID;
            gameInformation.field_condition = gameInformationDTO.FieldCondition;
            gameInformation.precipitation = gameInformationDTO.Precipitation;
            gameInformation.sky = gameInformationDTO.Sky;
            gameInformation.temperature = gameInformationDTO.Temperature;
            gameInformation.wind_direction = gameInformationDTO.WindDirection;
            gameInformation.wind_speed = gameInformationDTO.WindSpeed;
            gameInformation.game_time_length_minutes = gameInformationDTO.GameTimeLengthMinutes;
            gameInformation.attendance = gameInformationDTO.Attendance;
            gameInformation.ballpark_id = gameInformationDTO.BallPark_ID;
            gameInformation.winning_pitcher_id = gameInformationDTO.WinningPitcherID;
            gameInformation.losing_pitcher_id = gameInformationDTO.LosingPitcherID;
            gameInformation.save_pitcher_id = gameInformationDTO.SavePitcherID;
            gameInformation.winning_rbi_player_id = gameInformationDTO.WinningPitcherID;
            gameInformation.oscorer = gameInformationDTO.OScorer;
            gameInformation.season_year = gameInformationDTO.SeasonYear;
            gameInformation.season_game_type = gameInformationDTO.SeasonGameType;
            gameInformation.edit_time = gameInformationDTO.EditType;
            gameInformation.how_scored = gameInformationDTO.HowScored;
            gameInformation.input_prog_vers = gameInformationDTO.InputProgVers;
            gameInformation.inputter = gameInformationDTO.Inputter;
            gameInformation.scorer = gameInformationDTO.Scorer;
            gameInformation.translator = gameInformationDTO.Translator;

            return gameInformation;
        }
    }
}
