using System;
using System.Linq;
using Retrosheet_Persist;
using System.Collections.Generic;
using Retrosheet_EventData.Model;

namespace Retrosheet_RetrieveData
{
    public class RetrieveData
    {
        private DateTime dateTime;
        //private DataModels.ReferenceData referenceData = new DataModels.ReferenceData();

        private Dictionary<string, DataModels.ReferenceData> referenceDataDictionary = new Dictionary<string, DataModels.ReferenceData>();
        private Dictionary<string, DataModels.StartingPlayerInformation> startingPlayerDictionary = new Dictionary<string, DataModels.StartingPlayerInformation>();
        private Dictionary<string, DataModels.PlayerInformation> playerDictionary = new Dictionary<string, DataModels.PlayerInformation>();
        private Dictionary<string, DataModels.TeamInformation> teamDictionary = new Dictionary<string, DataModels.TeamInformation>();

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


        public void RetrieveReferenceData()
        {

            using (var dbCtx = new retrosheetDB())
            {
                var reference_items = from reference_data in dbCtx.Reference_Data
                                     select new
                                     {
                                         reference_data
                                     };

                foreach (var reference_data_item in reference_items)
                {
                    DataModels.ReferenceData referenceData = new DataModels.ReferenceData();

                    referenceData.ReferenceItem.RecordID = reference_data_item.reference_data.record_id;
                    referenceData.ReferenceItem.ReferenceDataType = reference_data_item.reference_data.ref_data_type;
                    referenceData.ReferenceItem.ReferenceDataCode = reference_data_item.reference_data.ref_data_code;
                    referenceData.ReferenceItem.ReferenceDataDescription = reference_data_item.reference_data.ref_data_desc;

                    referenceDataDictionary.Add(referenceData.ReferenceItem.ReferenceDataType + referenceData.ReferenceItem.ReferenceDataCode, referenceData);
                }
            }
        }

        public void RetrieveGame(string gameID)
        {
            using (var dbCtx = new retrosheetDB())
            {
                var games = from game_information in dbCtx.Game_Information

                            //join ref_game_type in dbCtx.Reference_Data
                            //      on new { A = "season_game_type", B = game_information.season_game_type }
                            //          equals new { A = ref_game_type.ref_data_type, B = ref_game_type.ref_data_code }

                            //join ref_game_number in dbCtx.Reference_Data
                            //      on new { A = "game_number", B = game_information.game_number.ToString() }
                            //          equals new { A = ref_game_number.ref_data_type.ToString(), B = ref_game_number.ref_data_code }

                            //join ref_wind_direction in dbCtx.Reference_Data
                            //     on new { A = "wind_direction", B = game_information.wind_direction}
                            //          equals new { A = ref_wind_direction.ref_data_type, B = ref_wind_direction.ref_data_code }

                            join ballpark in dbCtx.Ballparks on game_information.ballpark_id equals ballpark.ballpark_id

                            join visitingTeam in dbCtx.Teams on game_information.visiting_team_id equals visitingTeam.team_id
                            join homeTeam in dbCtx.Teams on game_information.home_team_id equals homeTeam.team_id

                            join umpireHome in dbCtx.Personnels on game_information.umpire_home_id equals umpireHome.person_id
                            join umpireFirst in dbCtx.Personnels on game_information.umpire_first_base_id equals umpireFirst.person_id
                            join umpireSecond in dbCtx.Personnels on game_information.umpire_second_base_id equals umpireSecond.person_id
                            join umpireThird in dbCtx.Personnels on game_information.umpire_third_base_id equals umpireThird.person_id

                            /*
                             * join umpireHomeRole in dbCtx.Reference_Data
                                on new { A = "personnel", B = umpireHome.role }
                                     equals new { A = umpireHomeRole.ref_data_type, B = umpireHomeRole.ref_data_code }

                            join umpireFirstRole in dbCtx.Reference_Data
                                on new { A = "personnel", B = umpireFirst.role }
                                     equals new { A = umpireFirstRole.ref_data_type, B = umpireFirstRole.ref_data_code }

                            join umpireSecondRole in dbCtx.Reference_Data
                                on new { A = "personnel", B = umpireHome.role }
                                     equals new { A = umpireSecondRole.ref_data_type, B = umpireSecondRole.ref_data_code }

                            join umpireThirdRole in dbCtx.Reference_Data
                                on new { A = "personnel", B = umpireHome.role }
                                     equals new { A = umpireThirdRole.ref_data_type, B = umpireThirdRole.ref_data_code }
                            */

                            //join winningPitcher in dbCtx.Players on game_information.winning_pitcher_id equals winningPitcher.player_id
                            //join losingPitcher in dbCtx.Players on game_information.losing_pitcher_id equals losingPitcher.player_id

                            // this is to prove the left join stuff works when there is data on the left side
                            //join xSavePitcher in dbCtx.Players on game_information.save_pitcher_id equals xSavePitcher.player_id
                            //     into savePitcherJoin from savePitcher in savePitcherJoin.DefaultIfEmpty()

                            //join xWinningRBI in dbCtx.Players on game_information.winning_rbi_player_id equals xWinningRBI.player_id
                            //    into winningRBIJoin from winningRBI  in winningRBIJoin.DefaultIfEmpty()

                            //join winningPitcherTeam in dbCtx.Teams on winningPitcher.team_id equals winningPitcherTeam.team_id
                            //join losingPitcherTeam in dbCtx.Teams on losingPitcher.team_id equals losingPitcherTeam.team_id
                            //join savePitcherTeam in dbCtx.Teams on savePitcher.team_id equals savePitcherTeam.team_id
                            //join xWinningRBITeam in dbCtx.Teams on winningRBI.team_id equals xWinningRBITeam.team_id
                            //    into winningRBITeamJoin from winningRBITeam in winningRBITeamJoin.DefaultIfEmpty(null)
                            //join xWinningRBIFieldPosition in dbCtx.Reference_Data
                            //                                on new { A = "field_position_x", B = winningRBI.field_position }
                            //   equals new { A = xWinningRBIFieldPosition.ref_data_type, B = xWinningRBIFieldPosition.ref_data_code }
                            //    into winningRBIFieldPositionJoin from winningRBIFieldPosition in winningRBIFieldPositionJoin.DefaultIfEmpty()


                            where game_information.game_id == gameID

                            select new
                            {
                                game_information,
                                //ref_game_type,
                                //ref_game_number,
                                //ref_wind_direction,
                                ballpark,
                                visitingTeam,
                                homeTeam,
                                umpireHome,
                                umpireFirst,
                                umpireSecond,
                                umpireThird
                                //umpireHomeRole,
                                //umpireFirstRole,
                                //umpireSecondRole,
                                //umpireThirdRole,
                                //winningPitcher,
                                //losingPitcher,
                                //savePitcher,
                                //winningRBI,
                                //winningPitcherTeam,
                                //losingPitcherTeam,
                                //savePitcherTeam,
                                //winningRBITeam,
                                //winningRBIFieldPosition
                            };

                DataModels.GameInformation gameInformation = new DataModels.GameInformation();

                foreach (var game in games)
                {
                    gameInformation.RecordID = game.game_information.record_id;
                    gameInformation.GameID = game.game_information.game_id;
                    gameInformation.VisitingTeam_ID = game.game_information.visiting_team_id;
                    gameInformation.HomeTeam_ID = game.game_information.home_team_id;

                    RetrieveTeams(game.game_information.season_year, game.game_information.season_game_type, game.game_information.home_team_id, game.game_information.visiting_team_id);
                    RetrievePlayers(game.game_information.season_year, game.game_information.season_game_type, game.game_information.home_team_id, game.game_information.visiting_team_id);

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

                    gameInformation.GameNumberDesc = RetrieveReferenceDataDesc("game_number", game.game_information.game_number.ToString());
                    gameInformation.SeasonGameTypeDesc = RetrieveReferenceDataDesc("season_game_type", game.game_information.season_game_type);
                    gameInformation.WindDirectionDesc = RetrieveReferenceDataDesc("wind_direction", game.game_information.wind_direction);

                    gameInformation.Ballpark.Ballpark.RecordID = game.ballpark.record_id;
                    gameInformation.Ballpark.Ballpark.ID = game.ballpark.ballpark_id;
                    gameInformation.Ballpark.Ballpark.Name = game.ballpark.name;
                    gameInformation.Ballpark.Ballpark.AKA = game.ballpark.aka;
                    gameInformation.Ballpark.Ballpark.City = game.ballpark.city;
                    gameInformation.Ballpark.Ballpark.State = game.ballpark.state;

                    if (DateTime.TryParse(game.ballpark.start_date.ToString(), out dateTime))
                    {
                        gameInformation.Ballpark.Ballpark.StartDate = dateTime;
                    }
                    else
                    {
                        gameInformation.Ballpark.Ballpark.StartDate = DateTime.MaxValue; ;
                    }

                    if (DateTime.TryParse(game.ballpark.end_date.ToString(), out dateTime))
                    {
                        gameInformation.Ballpark.Ballpark.EndDate = dateTime;
                    }
                    else
                    {
                        gameInformation.Ballpark.Ballpark.EndDate = DateTime.MaxValue;
                    }

                    gameInformation.Ballpark.Ballpark.League = game.ballpark.league;
                    gameInformation.Ballpark.BallparkLeagueDesc = RetrieveReferenceDataDesc("ballpark_league", game.ballpark.league);
                    gameInformation.Ballpark.Ballpark.Notes = game.ballpark.notes;

                    gameInformation.VisitingTeam.Team.RecordID = game.visitingTeam.record_id;
                    gameInformation.VisitingTeam.Team.TeamID = game.visitingTeam.team_id;
                    gameInformation.VisitingTeam.Team.League = game.visitingTeam.league;
                    gameInformation.VisitingTeam.TeamLeagueDesc = RetrieveReferenceDataDesc("team_league", game.visitingTeam.league) ;
                    gameInformation.VisitingTeam.Team.City = game.visitingTeam.city;
                    gameInformation.VisitingTeam.Team.Name = game.visitingTeam.name;

                    gameInformation.HomeTeam.Team.RecordID = game.homeTeam.record_id;
                    gameInformation.HomeTeam.Team.TeamID = game.homeTeam.team_id;
                    gameInformation.HomeTeam.Team.League = game.homeTeam.league;
                    gameInformation.HomeTeam.TeamLeagueDesc = RetrieveReferenceDataDesc("team_league", game.homeTeam.league);
                    gameInformation.HomeTeam.Team.City = game.homeTeam.city;
                    gameInformation.HomeTeam.Team.Name = game.homeTeam.name;

                    gameInformation.UmpireHome.UmpirePersonel.RecordID = game.umpireHome.record_id;
                    gameInformation.UmpireHome.UmpirePersonel.PersonID = game.umpireHome.person_id;
                    gameInformation.UmpireHome.UmpirePersonel.LastName = game.umpireHome.last_name;
                    gameInformation.UmpireHome.UmpirePersonel.FirstName = game.umpireHome.first_name;
                    gameInformation.UmpireHome.UmpirePersonel.CareerDate = game.umpireHome.career_date;
                    gameInformation.UmpireHome.UmpireRole = RetrieveReferenceDataDesc("personnel", game.umpireHome.role);

                    gameInformation.UmpireFirst.UmpirePersonel.RecordID = game.umpireFirst.record_id;
                    gameInformation.UmpireFirst.UmpirePersonel.PersonID = game.umpireFirst.person_id;
                    gameInformation.UmpireFirst.UmpirePersonel.LastName = game.umpireFirst.last_name;
                    gameInformation.UmpireFirst.UmpirePersonel.FirstName = game.umpireFirst.first_name;
                    gameInformation.UmpireFirst.UmpirePersonel.CareerDate = game.umpireFirst.career_date;
                    gameInformation.UmpireFirst.UmpireRole = RetrieveReferenceDataDesc("personnel", game.umpireFirst.role);

                    gameInformation.UmpireSecond.UmpirePersonel.RecordID = game.umpireSecond.record_id;
                    gameInformation.UmpireSecond.UmpirePersonel.PersonID = game.umpireSecond.person_id;
                    gameInformation.UmpireSecond.UmpirePersonel.LastName = game.umpireSecond.last_name;
                    gameInformation.UmpireSecond.UmpirePersonel.FirstName = game.umpireSecond.first_name;
                    gameInformation.UmpireSecond.UmpirePersonel.CareerDate = game.umpireSecond.career_date;
                    gameInformation.UmpireSecond.UmpireRole = RetrieveReferenceDataDesc("personnel", game.umpireSecond.role);

                    gameInformation.UmpireThird.UmpirePersonel.RecordID = game.umpireThird.record_id;
                    gameInformation.UmpireThird.UmpirePersonel.PersonID = game.umpireThird.person_id;
                    gameInformation.UmpireThird.UmpirePersonel.LastName = game.umpireThird.last_name;
                    gameInformation.UmpireThird.UmpirePersonel.FirstName = game.umpireThird.first_name;
                    gameInformation.UmpireThird.UmpirePersonel.CareerDate = game.umpireThird.career_date;
                    gameInformation.UmpireThird.UmpireRole = RetrieveReferenceDataDesc("personnel", game.umpireThird.role);

                    // cannot look up in Player because we don't know the teamID for the winningPitcher, losingPitcher, savePitcher, winningRBIPlayer
                    // going to have to figure out the which team was the winner and loser before looking up this information

                    //DataModels.PlayerInformation playerInformation = playerDictionary[game.winningPitcher.player_id];

                    //DataModels.TeamInformation teamInformation = teamDictionary[playerInformation.Player.TeamID];

                    //gameInformation.WinningPitcher.Player.RecordID = playerInformation.Player.RecordID;
                    //gameInformation.WinningPitcher.Player.PlayerID = playerInformation.Player.PlayerID;
                    //gameInformation.WinningPitcher.Player.LastName = playerInformation.Player.LastName;
                    //gameInformation.WinningPitcher.Player.FirstName = playerInformation.Player.FirstName;
                    //gameInformation.WinningPitcher.Player.Throws = RetrieveReferenceDataDesc("throws", playerInformation.Player.Throws);
                    //gameInformation.WinningPitcher.Player.Bats = RetrieveReferenceDataDesc("bats", playerInformation.Player.Bats);
                    //gameInformation.WinningPitcher.PlayerTeam = teamInformation.Team.Name;
                    //gameInformation.WinningPitcher.Player.Position = RetrieveReferenceDataDesc("field_position_x", playerInformation.Player.Position);


                    //gameInformation.WinningPitcher.Player.RecordID = game.winningPitcher.record_id;
                    //gameInformation.WinningPitcher.Player.PlayerID = game.winningPitcher.player_id;
                    //gameInformation.WinningPitcher.Player.LastName = game.winningPitcher.last_name;
                    //gameInformation.WinningPitcher.Player.FirstName = game.winningPitcher.first_name;
                    //gameInformation.WinningPitcher.Player.Throws = game.winningPitcher.throws;
                    //gameInformation.WinningPitcher.Player.Bats = game.winningPitcher.bats;
                    //gameInformation.WinningPitcher.PlayerTeam = game.winningPitcherTeam.name;
                    //gameInformation.WinningPitcher.Player.Position = "pitcher";

                    //gameInformation.LosingPitcher.Player.RecordID = game.losingPitcher.record_id;
                    //gameInformation.LosingPitcher.Player.PlayerID = game.losingPitcher.player_id;
                    //gameInformation.LosingPitcher.Player.LastName = game.losingPitcher.last_name;
                    //gameInformation.LosingPitcher.Player.FirstName = game.losingPitcher.first_name;
                    //gameInformation.LosingPitcher.Player.Throws = game.losingPitcher.throws;
                    //gameInformation.LosingPitcher.Player.Bats = game.losingPitcher.bats;
                    //gameInformation.LosingPitcher.PlayerTeam = game.losingPitcherTeam.name;
                    //gameInformation.LosingPitcher.Player.Position= "pitcher";

                    //gameInformation.SavePitcher.Player.RecordID = game.savePitcher.record_id;
                    //gameInformation.SavePitcher.Player.PlayerID = game.savePitcher.player_id;
                    //gameInformation.SavePitcher.Player.LastName = game.savePitcher.last_name;
                    //gameInformation.SavePitcher.Player.FirstName = game.savePitcher.first_name;
                    //gameInformation.SavePitcher.Player.Throws = game.savePitcher.throws;
                    //gameInformation.SavePitcher.Player.Bats = game.savePitcher.bats;
                    //gameInformation.SavePitcher.PlayerTeam = game.savePitcherTeam.name;
                    //gameInformation.SavePitcher.Player.Position = referenceData.ReferenceItem.ReferenceDataDescription;

                    /*
                    if (game.winningRBI != null )
                    {
                        gameInformation.WinningRBIPlayer.Player.RecordID = game.winningRBI.record_id;
                        gameInformation.WinningRBIPlayer.Player.PlayerID = game.winningRBI.player_id;
                        gameInformation.WinningRBIPlayer.Player.LastName = game.winningRBI.last_name;
                        gameInformation.WinningRBIPlayer.Player.FirstName = game.winningRBI.first_name;
                        gameInformation.WinningRBIPlayer.Player.Throws = game.winningRBI.throws;
                        gameInformation.WinningRBIPlayer.Player.Bats = game.winningRBI.bats;
                        gameInformation.WinningRBIPlayer.PlayerTeam = game.winningRBI.team_id;
                    }

                    if (game.winningRBIFieldPosition != null)
                    {
                        gameInformation.WinningRBIPlayer.Player.Position = game.winningRBIFieldPosition.ref_data_desc;
                    }
                    */
                }
            }
        }

        public void RetrieveStartingPlayers(string gameID)
        {
            using (var dbCtx = new retrosheetDB())
            {
                var startingPlayers = from starting_players in dbCtx.Starting_Player

                                      where starting_players.game_id == gameID

                                      select new
                                      {
                                          starting_players
                                      };


                
                foreach (var startingPlayer in startingPlayers)
                {
                    DataModels.StartingPlayerInformation startingPlayerInformation = new DataModels.StartingPlayerInformation();

                    startingPlayerInformation.StartingPlayer.RecordID = startingPlayer.starting_players.record_id;
                    startingPlayerInformation.StartingPlayer.GameID = startingPlayer.starting_players.game_id;
                    startingPlayerInformation.StartingPlayer.PlayerID = startingPlayer.starting_players.player_id;
                    startingPlayerInformation.StartingPlayer.GameTeamCode = startingPlayer.starting_players.game_team_code;
                    startingPlayerInformation.StartingPlayer.BattingOrder = startingPlayer.starting_players.batting_order;
                    startingPlayerInformation.StartingPlayer.FieldPosition = startingPlayer.starting_players.field_position;
                   
                    startingPlayerInformation.StartingPlayer.TeamId = startingPlayer.starting_players.team_id;

                    DataModels.TeamInformation teamInformation = RetrieveTeamInformation(startingPlayer.starting_players.team_id);
                    startingPlayerInformation.TeamName = teamInformation.Team.Name;

                    DataModels.PlayerInformation playerInformation = RetrievePlayerInformation(startingPlayerInformation.StartingPlayer.PlayerID);

                    startingPlayerInformation.LastName = playerInformation.Player.LastName;
                    startingPlayerInformation.FirstName = playerInformation.Player.FirstName;
                    startingPlayerInformation.Bats = playerInformation.Player.Bats;
                    startingPlayerInformation.Throws = playerInformation.Player.Throws;
                    startingPlayerInformation.FieldPostionDesc =playerInformation.PlayerPositionDesc;

                    startingPlayerDictionary.Add(startingPlayerInformation.StartingPlayer.PlayerID, startingPlayerInformation);
                }
            }
        }

        private void RetrievePlayers(string seasonYear, string SeasonGameType, string homeTeamId, string visitingTeamId)
        {
            // get the home team players
            using (var dbCtx = new retrosheetDB())
            {
                var players = from player_x in dbCtx.Players
                              where (player_x.team_id == homeTeamId) && (player_x.season_year == seasonYear) && (player_x.season_game_type == SeasonGameType)
                              select new
                                        {
                                            player_x
                                        };


                foreach (var player in players)
                {
                    DataModels.PlayerInformation playerInformation = new DataModels.PlayerInformation();

                    playerInformation.Player.RecordID = player.player_x.record_id;
                    playerInformation.Player.PlayerID = player.player_x.player_id;
                    playerInformation.Player.LastName = player.player_x.last_name;
                    playerInformation.Player.FirstName = player.player_x.first_name;
                    playerInformation.Player.Throws = RetrieveReferenceDataDesc("throws", player.player_x.throws);
                    playerInformation.Player.Bats = RetrieveReferenceDataDesc("bats", player.player_x.bats);
                    playerInformation.Player.TeamID = player.player_x.team_id;

                    DataModels.TeamInformation teamInformation = RetrieveTeamInformation(player.player_x.team_id);
                    playerInformation.PlayerTeamName = teamInformation.Team.Name;

                    playerInformation.Player.Position = player.player_x.field_position;

                    playerInformation.PlayerPositionDesc = RetrieveReferenceDataDesc("field_position_x", playerInformation.Player.Position);

                    playerDictionary.Add(playerInformation.Player.PlayerID, playerInformation);
                }
            }

            // get the visiting team players
            using (var dbCtx = new retrosheetDB())
            {
                var players = from player_x in dbCtx.Players
                                  //where teamIdList.Contains(player_x.team_id)
                              where (player_x.team_id == visitingTeamId) && (player_x.season_year == seasonYear) && (player_x.season_game_type == SeasonGameType)
                              select new
                              {
                                  player_x
                              };


                foreach (var player in players)
                {
                    DataModels.PlayerInformation playerInformation = new DataModels.PlayerInformation();

                    playerInformation.Player.RecordID = player.player_x.record_id;
                    playerInformation.Player.PlayerID = player.player_x.player_id;
                    playerInformation.Player.LastName = player.player_x.last_name;
                    playerInformation.Player.FirstName = player.player_x.first_name;
                    playerInformation.Player.Throws = RetrieveReferenceDataDesc("throws", player.player_x.throws);
                    playerInformation.Player.Bats = RetrieveReferenceDataDesc("bats", player.player_x.bats);
                    playerInformation.Player.TeamID = player.player_x.team_id;

                    DataModels.TeamInformation teamInformation = RetrieveTeamInformation(player.player_x.team_id);
                    playerInformation.PlayerTeamName = teamInformation.Team.Name;

                    playerInformation.Player.Position = player.player_x.field_position;

                    playerInformation.PlayerPositionDesc = RetrieveReferenceDataDesc("field_position_x", playerInformation.Player.Position);

                    playerDictionary.Add(playerInformation.Player.PlayerID, playerInformation);
                }
            }
        }

        private void RetrieveTeams(string seasonYear, string SeasonGameType, string homeTeamId, string visitingTeamId)
        {
            var teamIdList = new string[] { homeTeamId, visitingTeamId };

            //  get the home team
            using (var dbCtx = new retrosheetDB())
            {
                var teams = from team_x in dbCtx.Teams
                            where (team_x.team_id == homeTeamId) && (team_x.season_year == seasonYear) && (team_x.season_game_type == SeasonGameType)

                          select new
                          {
                              team_x
                          };

                foreach (var team in teams)
                {
                    DataModels.TeamInformation teamInformation = new DataModels.TeamInformation();

                    teamInformation.Team.RecordID = team.team_x.record_id;
                    teamInformation.Team.TeamID = team.team_x.team_id;
                    teamInformation.Team.League = team.team_x.league;
                    teamInformation.Team.City = team.team_x.city;
                    teamInformation.Team.Name = team.team_x.name;
                    teamInformation.Team.SeasonYear = seasonYear;
                    teamInformation.Team.SeasonGameType = SeasonGameType;
                    teamInformation.TeamLeagueDesc = RetrieveReferenceDataDesc("team_league", teamInformation.Team.League);

                    teamDictionary.Add(teamInformation.Team.TeamID, teamInformation);
                }
            }

            //  get the visiting team
            using (var dbCtx = new retrosheetDB())
            {
                var teams = from team_x in dbCtx.Teams
                            where (team_x.team_id == visitingTeamId) && (team_x.season_year == seasonYear) && (team_x.season_game_type == SeasonGameType)

                            select new
                            {
                                team_x
                            };

                foreach (var team in teams)
                {
                    DataModels.TeamInformation teamInformation = new DataModels.TeamInformation();

                    teamInformation.Team.RecordID = team.team_x.record_id;
                    teamInformation.Team.TeamID = team.team_x.team_id;
                    teamInformation.Team.League = team.team_x.league;
                    teamInformation.Team.City = team.team_x.city;
                    teamInformation.Team.Name = team.team_x.name;

                    teamInformation.TeamLeagueDesc = RetrieveReferenceDataDesc("team_league", teamInformation.Team.League);

                    teamDictionary.Add(teamInformation.Team.TeamID, teamInformation);
                }
            }

        }

        private string RetrieveReferenceDataDesc(string ref_data_type, string ref_data_code)
        {
            DataModels.ReferenceData referenceData = new DataModels.ReferenceData();

            ref_data_code = ref_data_code.Trim();

            referenceData = referenceDataDictionary[ref_data_type + ref_data_code];
            return referenceData.ReferenceItem.ReferenceDataDescription;
        }

        private DataModels.PlayerInformation RetrievePlayerInformation (string playerId)
        {
            DataModels.PlayerInformation playerInformation = new DataModels.PlayerInformation();

            playerInformation = playerDictionary[playerId];
            return playerInformation;
        }

        private DataModels.TeamInformation RetrieveTeamInformation (string teamID)
        {
            DataModels.TeamInformation teamInformation = new DataModels.TeamInformation();

            teamInformation = teamDictionary[teamID];
            return teamInformation;
        }
    }
}


