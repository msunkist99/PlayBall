using System;
using System.Linq;
using Retrosheet_Persist;
using Retrosheet_EventData.Model;

namespace Retrosheet_RetrieveData
{
    public class RetrieveData
    {
        private DateTime dateTime;
        //constructor
        public RetrieveData()
        {

        }
        public void RetrieveAvailableSeasons()
        {
            using (var dbCtx = new retrosheetDB())
            {
                var Seasons = from season in dbCtx.Game_Information
                              orderby season.season_year ascending, season.season_game_type,
                                      season.game_date, season.game_number, season.game_number,
                                      season.game_id
                              select new
                              {
                                  season.season_year,
                                  season.season_game_type,
                                  season.game_date,
                                  season.game_number,
                                  season.game_id,
                                  season.home_team_id,
                                  season.visiting_team_id
                              };

                foreach (var s in Seasons)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}",
                                  s.season_year,
                                  s.season_game_type,
                                  s.game_date,
                                  s.game_number,
                                  s.game_id,
                                  s.home_team_id,
                                  s.visiting_team_id);
                }
            }
        }

        public void RetrieveAvailableSeasons2()
        {
            using (var dbCtx = new retrosheetDB())
            {
                var Seasons = from season2 in dbCtx.Game_Information

                              join ref_game_type in dbCtx.Reference_Data
                                    on new { A = "season_game_type", B = season2.season_game_type }
                                        equals new { A = ref_game_type.ref_data_type, B = ref_game_type.ref_data_code }

                              join ref_game_number in dbCtx.Reference_Data
                                    on new { A = "game_number", B = season2.game_number.ToString() }
                                        equals new { A = ref_game_number.ref_data_type, B = ref_game_number.ref_data_code }

                              join home_team in dbCtx.Teams on season2.home_team_id equals home_team.team_id

                              join visiting_team in dbCtx.Teams on season2.visiting_team_id equals visiting_team.team_id

                              orderby season2.season_year ascending, ref_game_type.ref_data_desc,
                                      season2.game_date, ref_game_number.ref_data_desc,
                                      season2.game_id
                              select new
                              {
                                  season2.season_year,
                                  season_game_type = ref_game_type.ref_data_desc,
                                  season2.game_date,
                                  season_game_number = ref_game_number.ref_data_desc,
                                  season2.game_id,
                                  home_team = home_team.name,
                                  visiting_team = visiting_team.name,
                              };

                foreach (var s in Seasons)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4} Home team-{5} Visiting team-{6}",
                                  s.season_year,
                                  s.season_game_type,
                                  s.game_date,
                                  s.season_game_number,
                                  s.game_id,
                                  s.home_team,
                                  s.visiting_team);
                }
            }
        }
        /*
        public void RetrieveGame(string gameID)
        {
            using (var dbCtx = new retrosheetDB())
            { 
                var completeGame = from game_information in dbCtx.Game_Information

                                   join ref_game_type in dbCtx.Reference_Data
                                   on new { A = "season_game_type", B = game_information.season_game_type }
                                   equals new { A = ref_game_type.ref_data_type, B = ref_game_type.ref_data_code }

                                   join ref_game_number in dbCtx.Reference_Data
                                         on new { A = "game_number", B = game_information.game_number.ToString() }
                                             equals new { A = ref_game_number.ref_data_type, B = ref_game_number.ref_data_code }
                                   
                                   join  

                                   join home_team in dbCtx.Teams on game_information.home_team_id equals home_team.team_id

                                   join visiting_team in dbCtx.Teams on game_information.visiting_team_id equals visiting_team.team_id

                                   join ballpark in dbCtx.Ballparks on game_information.ballpark_id equals ballpark.ballpark_id

                                   orderby game_information.season_year ascending, ref_game_type.ref_data_desc,
                                           game_information.game_date, ref_game_number.ref_data_desc,
                                           game_information.game_id
                                   where game_information.game_id == gameID

                                   select new
                                   {
                                       game_information.season_year,
                                       season_game_type = ref_game_type.ref_data_desc,
                                       game_information.game_date,
                                       season_game_number = ref_game_number.ref_data_desc,
                                       game_information.game_id,
                                       ballpark_name = ballpark.name,
                                       ballpark_city = ballpark.city,
                                       ballpark_notes = ballpark.notes,                                   
                                       home_team = home_team.name,
                                       visiting_team = visiting_team.name,
                                   };

            foreach (var game in completeGame)
            {
                Console.WriteLine("{0} {1} {2} {3} {4} Home team-{5}  Visiting team-{6}",
                              game.season_year,
                              game.season_game_type,
                              game.game_date,
                              game.season_game_number,
                              game.game_id,
                              game.ballpark_name,
                              game.ballpark_city,
                              game.ballpark_notes,
                              game.home_team,
                              game.visiting_team);

                Console.WriteLine("Ballpark-{0} in {1}",
                              game.ballpark_name,
                              game.ballpark_city);

                Console.WriteLine("Ballpark notes {0}",
                              game.ballpark_notes);
            }
        }
        */


        public void RetrieveGame(string gameID)
        {
            using (var dbCtx = new retrosheetDB())
            {
                var games = from game_information in dbCtx.Game_Information

                            join ref_game_type in dbCtx.Reference_Data
                                  on new { A = "season_game_type", B = game_information.season_game_type }
                                      equals new { A = ref_game_type.ref_data_type, B = ref_game_type.ref_data_code }

                            join ref_game_number in dbCtx.Reference_Data
                                  on new { A = "game_number", B = game_information.game_number.ToString() }
                                      equals new { A = ref_game_number.ref_data_type, B = ref_game_number.ref_data_code }

                            join ref_wind_direction in dbCtx.Reference_Data
                                 on new { A = "wind_direction", B = game_information.wind_direction }
                                      equals new { A = ref_wind_direction.ref_data_type, B = ref_wind_direction.ref_data_code }

                            join ballpark in dbCtx.Ballparks on game_information.ballpark_id equals ballpark.ballpark_id

                            join visitingTeam in dbCtx.Teams on game_information.visiting_team_id equals visitingTeam.team_id
                            join homeTeam in dbCtx.Teams on game_information.home_team_id equals homeTeam.team_id

                            join umpireHome in dbCtx.Personnels on game_information.umpire_home_id equals umpireHome.person_id
                            join umpireFirst in dbCtx.Personnels on game_information.umpire_first_base_id equals umpireFirst.person_id
                            join umpireSecond in dbCtx.Personnels on game_information.umpire_second_base_id equals umpireSecond.person_id
                            join umpireThird in dbCtx.Personnels on game_information.umpire_third_base_id equals umpireThird.person_id

                            join winningPitcher in dbCtx.Players on game_information.winning_pitcher_id equals winningPitcher.player_id
                            join losingPitcher in dbCtx.Players on game_information.losing_pitcher_id equals losingPitcher.player_id
                            join savePitcher in dbCtx.Players on game_information.save_pitcher_id equals savePitcher.player_id
                            join winningRBI in dbCtx.Players on game_information.winning_rbi_player_id equals winningRBI.player_id

                            where game_information.game_id == gameID

                            select new
                            {
                                game_information,
                                ref_game_type,
                                ref_game_number,
                                ref_wind_direction,
                                ballpark,
                                visitingTeam,
                                homeTeam,
                                umpireHome,
                                umpireFirst,
                                umpireSecond,
                                umpireThird,
                                winningPitcher,
                                losingPitcher,
                                savePitcher,
                                winningRBI


                                

                            };
           

                DataModels.GameInformation gameInformation = new DataModels.GameInformation();

                foreach (var game in games)
                {
                    gameInformation.RecordID = game.game_information.record_id;
                    gameInformation.GameID = game.game_information.game_id;
                    gameInformation.VisitingTeam_ID = game.game_information.visiting_team_id;
                    gameInformation.HomeTeam_ID = game.game_information.home_team_id;
                    gameInformation.GameDate = game.game_information.game_date;
                    gameInformation.GameNumber = game.game_information.game_number;
                    gameInformation.StartTime = game.game_information.start_time;
                    gameInformation.DayNight = game.game_information.day_night;
                    if (game.game_information.used_dh == "Y")
                    {
                        gameInformation.UsedDH = true;
                    }
                    else
                    {
                        gameInformation.UsedDH = false;
                    }
                    gameInformation.Pitches = game.game_information.pitches;
                    gameInformation.UmpireHome_ID = game.game_information.umpire_home_id;
                    gameInformation.UmpireFirstBaseID = game.game_information.umpire_first_base_id;
                    gameInformation.UmpireSecondBaseID = game.game_information.umpire_second_base_id;
                    gameInformation.UmpireThirdBaseID = game.game_information.umpire_third_base_id;
                    gameInformation.FieldCondition = game.game_information.field_condition;
                    gameInformation.Precipitation = game.game_information.precipitation;
                    gameInformation.Sky = game.game_information.sky;
                    gameInformation.Temperature = (int)game.game_information.temperature;
                    gameInformation.WindDirection = game.game_information.wind_direction;
                    gameInformation.WindSpeed = (int)game.game_information.wind_speed;
                    gameInformation.GameTimeLengthMinutes = (int)game.game_information.game_time_length_minutes;
                    gameInformation.Attendance = (int)game.game_information.attendance;
                    gameInformation.BallPark_ID = game.game_information.ballpark_id;
                    gameInformation.WinningPitcherID = game.game_information.winning_pitcher_id;
                    gameInformation.LosingPitcherID = game.game_information.losing_pitcher_id;
                    gameInformation.SavePitcherID = game.game_information.save_pitcher_id;
                    gameInformation.WinningPitcherID = game.game_information.winning_pitcher_id;
                    gameInformation.OScorer = game.game_information.oscorer;
                    gameInformation.SeasonYear = game.game_information.season_year;
                    gameInformation.SeasonGameType = game.game_information.season_game_type;
                    gameInformation.EditType = game.game_information.edit_time;
                    gameInformation.HowScored = game.game_information.how_scored;
                    gameInformation.InputProgVers = game.game_information.input_prog_vers;
                    gameInformation.Inputter = game.game_information.inputter;
                    gameInformation.InputTime = game.game_information.input_time;
                    gameInformation.Scorer = game.game_information.scorer;
                    gameInformation.Translator = game.game_information.translator;

                    gameInformation.GameNumberDesc = game.ref_game_number.ref_data_desc;
                    gameInformation.SeasonGameTypeDesc = game.ref_game_type.ref_data_desc;
                    gameInformation.WindDirection = game.ref_wind_direction.ref_data_desc;

                    gameInformation.BallPark.RecordID = game.ballpark.record_id;
                    gameInformation.BallPark.ID = game.ballpark.ballpark_id;
                    gameInformation.BallPark.Name = game.ballpark.name;
                    gameInformation.BallPark.AKA = game.ballpark.aka;
                    gameInformation.BallPark.City = game.ballpark.city;
                    gameInformation.BallPark.State = game.ballpark.state;

                    if (DateTime.TryParse(game.ballpark.start_date.ToString(), out dateTime))
                    {
                        gameInformation.BallPark.StartDate = dateTime;
                    }
                    else
                    {
                        gameInformation.BallPark.StartDate = DateTime.MaxValue; ;
                    }

                    if (DateTime.TryParse(game.ballpark.end_date.ToString(), out dateTime))
                    {
                        gameInformation.BallPark.EndDate = dateTime;
                    }
                    else
                    {
                        gameInformation.BallPark.EndDate = DateTime.MaxValue;
                    }

                    gameInformation.BallPark.League = game.ballpark.league;
                    gameInformation.BallPark.Notes = game.ballpark.notes;

                    gameInformation.VisitingTeam.RecordID = game.visitingTeam.record_id;
                    gameInformation.VisitingTeam.ID = game.visitingTeam.team_id;
                    gameInformation.VisitingTeam.League = game.visitingTeam.league;
                    gameInformation.VisitingTeam.City = game.visitingTeam.city;
                    gameInformation.VisitingTeam.Name = game.visitingTeam.name;

                    gameInformation.HomeTeam.RecordID = game.homeTeam.record_id;
                    gameInformation.HomeTeam.ID = game.homeTeam.team_id;
                    gameInformation.HomeTeam.League = game.homeTeam.league;
                    gameInformation.HomeTeam.City = game.homeTeam.city;
                    gameInformation.HomeTeam.Name = game.homeTeam.name;

                    gameInformation.UmpireHome.RecordID = game.umpireHome.record_id;
                    gameInformation.UmpireHome.PersonID = game.umpireHome.person_id;
                    gameInformation.UmpireHome.LastName = game.umpireHome.last_name;
                    gameInformation.UmpireHome.FirstName = game.umpireHome.first_name;
                    gameInformation.UmpireHome.CareerDate = game.umpireHome.career_date;
                    //gameInformation.UmpireHome.Role = game.umpireHome.role;

                    gameInformation.UmpireFirstBase.RecordID = game.umpireFirst.record_id;
                    gameInformation.UmpireFirstBase.PersonID = game.umpireFirst.person_id;
                    gameInformation.UmpireFirstBase.LastName = game.umpireFirst.last_name;
                    gameInformation.UmpireFirstBase.FirstName = game.umpireFirst.first_name;
                    gameInformation.UmpireFirstBase.CareerDate = game.umpireFirst.career_date;
                    //gameInformation.UmpireFirstBase.Role = game.umpireFirst.role;

                    gameInformation.UmpireSecondBase.RecordID = game.umpireSecond.record_id;
                    gameInformation.UmpireSecondBase.PersonID = game.umpireSecond.person_id;
                    gameInformation.UmpireSecondBase.LastName = game.umpireSecond.last_name;
                    gameInformation.UmpireSecondBase.FirstName = game.umpireSecond.first_name;
                    gameInformation.UmpireSecondBase.CareerDate = game.umpireSecond.career_date;
                    //gameInformation.UmpireSecondBase.Role = game.umpireSecond.role;

                    gameInformation.UmpireSecondBase.RecordID = game.umpireSecond.record_id;
                    gameInformation.UmpireSecondBase.PersonID = game.umpireSecond.person_id;
                    gameInformation.UmpireSecondBase.LastName = game.umpireSecond.last_name;
                    gameInformation.UmpireSecondBase.FirstName = game.umpireSecond.first_name;
                    gameInformation.UmpireSecondBase.CareerDate = game.umpireSecond.career_date;
                    //gameInformation.UmpireSecondBase.Role = game.umpireSecond.role;

                    gameInformation.WinningPitcher.RecordID = game.winningPitcher.record_id;
                    gameInformation.WinningPitcher.PlayerID = game.winningPitcher.player_id;
                    gameInformation.WinningPitcher.LastName = game.winningPitcher.last_name;
                    gameInformation.WinningPitcher.FirstName = game.winningPitcher.first_name;
                    gameInformation.WinningPitcher.Throws = game.winningPitcher.throws;
                    gameInformation.WinningPitcher.Bats = game.winningPitcher.bats;
                    //gameInformation.WinningPitcher.TeamID = game.winningPitcher.team_id;
                    //gameInformation.WinningPitcher.Position = game.winningPitcher.field_position;

                    gameInformation.LosingPitcher.RecordID = game.losingPitcher.record_id;
                    gameInformation.LosingPitcher.PlayerID = game.losingPitcher.player_id;
                    gameInformation.LosingPitcher.LastName = game.losingPitcher.last_name;
                    gameInformation.LosingPitcher.FirstName = game.losingPitcher.first_name;
                    gameInformation.LosingPitcher.Throws = game.losingPitcher.throws;
                    gameInformation.LosingPitcher.Bats = game.losingPitcher.bats;
                    //gameInformation.LosingPitcher.TeamID = game.losingPitcher.team_id;
                    //gameInformation.LosingPitcher.Position = game.losingPitcher.field_position;

                    gameInformation.SavePitcher.RecordID = game.savePitcher.record_id;
                    gameInformation.SavePitcher.PlayerID = game.savePitcher.player_id;
                    gameInformation.SavePitcher.LastName = game.savePitcher.last_name;
                    gameInformation.SavePitcher.FirstName = game.savePitcher.first_name;
                    gameInformation.SavePitcher.Throws = game.savePitcher.throws;
                    gameInformation.SavePitcher.Bats = game.savePitcher.bats;
                    //gameInformation.SavePitcher.TeamID = game.savePitcher.team_id;
                    //gameInformation.SavePitcher.Position = game.savePitcher.field_position;

                    gameInformation.WinningRBIPlayer.RecordID = game.winningRBI.record_id;
                    gameInformation.WinningRBIPlayer.PlayerID = game.winningRBI.player_id;
                    gameInformation.WinningRBIPlayer.LastName = game.winningRBI.last_name;
                    gameInformation.WinningRBIPlayer.FirstName = game.winningRBI.first_name;
                    gameInformation.WinningRBIPlayer.Throws = game.winningRBI.throws;
                    gameInformation.WinningRBIPlayer.Bats = game.winningRBI.bats;
                    //gameInformation.WinningRBIPlayer.TeamID = game.winningRBI.team_id;
                    //gameInformation.WinningRBIPlayer.Position = game.winningRBI.field_position;

                }
            }
        }

}
}
