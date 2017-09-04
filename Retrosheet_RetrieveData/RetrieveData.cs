using System;
using System.Linq;
using Retrosheet_Persist;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Diagnostics;
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

		private string outputDelimiter = "|";

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

		public void RetrievePlayByPlay(string gameID)
		{
			using (var dbCtx = new retrosheetDB())
			{
				var plays = from play_x in dbCtx.Plays
							where play_x.game_id == gameID
							select new
							{
								play_x
							};

				foreach (var play in plays)
				{

					DataModels.PlayInformation playInformation = new DataModels.PlayInformation();

					playInformation.Play.RecordID = play.play_x.record_id;
					playInformation.Play.GameID = play.play_x.game_id;
					playInformation.Play.Inning = play.play_x.inning;
					playInformation.Play.GameTeamCode = play.play_x.game_team_code;
					playInformation.Play.Sequence = play.play_x.sequence;
					playInformation.Play.PlayerID = play.play_x.player_id;
					playInformation.Play.CountBalls = (int)play.play_x.count_balls;
					playInformation.Play.CountStrikes = (int)play.play_x.count_strikes;
					playInformation.Play.Pitches = play.play_x.pitches;
					playInformation.Play.EventSequence = play.play_x.event_sequence;
					playInformation.Play.EventModifier = play.play_x.event_modifier;
					playInformation.Play.EventRunnerAdvance = play.play_x.event_runner_advance;
					playInformation.Play.EventHitLocation = play.play_x.event_hit_location;


					char[] pitchCodes = playInformation.Play.Pitches.ToCharArray();

					int pitchCodeCount = pitchCodes.Count();
					int x = 0;

					foreach (char pitchCode in pitchCodes)
					{
						x++;
						if (x < pitchCodeCount)
						{
							playInformation.PitchDesc = playInformation.PitchDesc + RetrieveReferenceDataDesc("pitch_code", pitchCode.ToString()) + outputDelimiter;
						}
						else
						{
							playInformation.PitchDesc = playInformation.PitchDesc + RetrieveReferenceDataDesc("pitch_code", pitchCode.ToString());
						}
					}

					int event_sequence_n;
					bool isNumeric = int.TryParse(play.play_x.event_sequence, out event_sequence_n);

					if (isNumeric == true)
					{
						char[] event_sequence_codes = play.play_x.event_sequence.ToCharArray();

						int event_sequence_count = event_sequence_codes.Count();
						x = 0;

						foreach (char event_sequence_code in event_sequence_codes)
						{
							x++;
							if (x < event_sequence_count)
							{
								playInformation.EventSequenceDesc = playInformation.EventHitLocationDesc + RetrieveReferenceDataDesc("field_position", event_sequence_code.ToString()) + outputDelimiter;
							}
							else
							{
								playInformation.EventSequenceDesc = playInformation.EventHitLocationDesc + RetrieveReferenceDataDesc("field_position", event_sequence_code.ToString());
							}
						}
					}
					else
					{
						playInformation.EventSequenceDesc = RetrieveReferenceDataDesc("event_sequence", play.play_x.event_sequence);
					}

					playInformation.EventSequenceModifierDesc = RetrieveReferenceDataDesc("play_modifier", play.play_x.event_modifier);
					playInformation.EventHitLocationDesc = RetrieveReferenceDataDesc("hit_location", play.play_x.event_hit_location);
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

			//ref_data_code = ref_data_code.Trim();

			try
			{
				referenceData = referenceDataDictionary[ref_data_type + ref_data_code];
				return referenceData.ReferenceItem.ReferenceDataDescription;

			}
			catch
			{
				return null;
			}
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

        //public List<TreeViewModels.Season> RetrieveTreeViewData_Seasons()
        public Collection<TreeViewModels.Season> RetrieveTreeViewData_Seasons()
		{
            string holdSeasonYear = "";
            string sqlQuery = @"select 
distinct(game.season_year) as _seasonYear, 
game.season_game_type as _seasonGameType, 
ref.ref_data_desc as _seasonGameTypeDesc,
case game.season_game_type
	when 'R' then 0
	when 'A' then 1
	when 'C' then 2
	when '1' then 3
	when '2' then 4
	when 'L' then 5
	when 'W' then 6
end as _sortKey
from Game_Information game
join Reference_Data ref on game.season_game_type = ref.ref_data_code
					   and ref.ref_data_type = 'season_game_type'
order by game.season_year,
         _sortKey";

            // add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				IEnumerable<_SeasonGameType> results = dbCtx.ExecuteQuery<_SeasonGameType>(sqlQuery).ToList();
				Console.WriteLine("_SeasonGameType record count " + results.Count());

                Collection<TreeViewModels.Season> Seasons = new Collection<TreeViewModels.Season>();
                TreeViewModels.Season season = new TreeViewModels.Season(); 

                foreach (_SeasonGameType result in results)
                {
                    if (holdSeasonYear == "")
                    {
                        season.SeasonYear = result._seasonYear;
                        season.SeasonIcon = "c:/users/mmr/documents/retrosheet/icons/LK-MLB.gif";
                        season.SeasonGameTypes = new List<TreeViewModels.SeasonGameType>();

                        TreeViewModels.SeasonGameType seasonGameType = new TreeViewModels.SeasonGameType();
                        seasonGameType.GameType = result._seasonGameType;
                        seasonGameType.GameTypeDesc = result._seasonGameTypeDesc;
                        season.SeasonGameTypes.Add(seasonGameType);
                        holdSeasonYear = result._seasonYear;
                    }
                    else if (holdSeasonYear != result._seasonYear)
                    {
                        Seasons.Add(season);

                        season = new TreeViewModels.Season();
                        season.SeasonYear = result._seasonYear;
                        season.SeasonIcon = "c:/users/mmr/documents/retrosheet/icons/LK-MLB.gif";
                        season.SeasonGameTypes = new List<TreeViewModels.SeasonGameType>();

                        TreeViewModels.SeasonGameType seasonGameType = new TreeViewModels.SeasonGameType();
                        seasonGameType.GameType = result._seasonGameType;
                        seasonGameType.GameTypeDesc = result._seasonGameTypeDesc;
                        season.SeasonGameTypes.Add(seasonGameType);
                        holdSeasonYear = result._seasonYear;
                    }
                    else
                    {
                        TreeViewModels.SeasonGameType seasonGameType = new TreeViewModels.SeasonGameType();
                        seasonGameType.GameType = result._seasonGameType;
                        seasonGameType.GameTypeDesc = result._seasonGameTypeDesc;
                        season.SeasonGameTypes.Add(seasonGameType);
                    }
                }
                Seasons.Add(season);

                return Seasons;
            }
        }

		private class _SeasonGameType
		{
			public string _seasonYear { get; set; }
			public string _seasonGameType { get; set; }
			public string _seasonGameTypeDesc { get; set; }
			public int _sortKey { get; set; }
		}

        public Collection<TreeViewModels.Season> RetrieveTreeViewData_GameSelectionList()
        {
            string holdSeasonYear = "";
            string holdSeasonGameType = "";
            string holdDisplayUnderLeagueID = "";
            string holdDisplayUnderTeamID = "";


            string sqlQuery = @"select * from
(---- Post Season - all star and world series games
select distinct(games.season_year) _seasonYear, 
    games.season_game_type _seasonGameType, 
    refGameType.ref_data_desc _seasonGameTypeDesc,
	games.game_date _gameDate,
    games.game_number _gameNumber,
	homeTeam.league _gameHomeLeagueID, 
    refHomeTeamLeague.ref_data_desc _gameHomeLeagueName, 
    games.home_team_id _gameHomeTeamID, 
    homeTeam.name _gameHomeTeamName, 
    homeTeam.city _gameHomeTeamCity, 
	visitTeam.league _gameVisitLeagueID, 
    refVisitTeamLeague.ref_data_desc _gameVisitLeagueName, 
    games.visiting_team_id _gameVisitTeamID, 
    visitTeam.name _gameVisitTeamName, 
    visitTeam.city _gameVisitTeamCity,
	games.game_id _gameID, 

	'' _displayUnderLeagueID,
	'' _displayUnderLeagueName,
	'' _displayUnderTeamID,
	'' _displayUnderTeamName,
	 
	refIconPath.ref_data_desc _iconPath,
    refMLBIcon.ref_data_desc _MLBIcon,
	'' _leagueIcon,
	'' _teamIcon,

	case games.season_game_type
		when 'R' then 0
		when 'A' then 1
		when 'C' then 2
		when '1' then 3
		when '2' then 4
		when 'L' then 5
		when 'W' then 6
	end as _sortKey
from retrosheet.dbo.Game_Information games
join retrosheet.dbo.Reference_Data refGameType on games.season_game_type = refGameType.ref_data_code
                                              and refGameType.ref_data_type = 'season_game_type'
join retrosheet.dbo.team homeTeam on games.home_team_id = homeTeam.team_id
join retrosheet.dbo.Reference_Data refHomeTeamLeague on homeTeam.league = refHomeTeamLeague.ref_data_code
                                              and refHomeTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.team visitTeam on games.visiting_team_id = visitTeam.team_id
join retrosheet.dbo.Reference_Data refVisitTeamLeague on visitTeam.league = refVisitTeamLeague.ref_data_code
                                              and refVisitTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.Reference_Data refIconPath on refIconPath.ref_data_code = 'icon_path'
                                              and refIconPath.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_Data refLeagueIcon on homeTeam.League = refLeagueIcon.ref_data_code
                                                and refLeagueIcon.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_Data refTeamIcon on games.home_team_id = refTeamIcon.ref_data_code
                                              and refTeamIcon.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
join retrosheet.dbo.Reference_data refMLBIcon on 'MLB' = refMLBIcon.ref_data_code
                                             and refMLBIcon.ref_data_type = 'icon'

  where games.season_game_type in ('A', 'W')
union all
---- Post Season - wild card, division and league championship games
select distinct(games.season_year) _seasonYear, 
    games.season_game_type _seasonGameType, 
    refGameType.ref_data_desc _seasonGameTypeDesc,
	games.game_date _gameDate,
    games.game_number _gameNumber,
	homeTeam.league _gameHomeLeagueID, 
    refHomeTeamLeague.ref_data_desc _gameHomeLeagueName, 
    games.home_team_id _gameHomeTeamID, 
    homeTeam.name _gameHomeTeamName, 
    homeTeam.city _gameHomeTeamCity, 
	visitTeam.league _gameVisitLeagueID, 
    refVisitTeamLeague.ref_data_desc _gameVisitLeagueName, 
    games.visiting_team_id _gameVisitTeamID, 
    visitTeam.name _gameVisitTeamName, 
    visitTeam.city _gameVisitTeamCity,
	games.game_id _gameID, 

	homeTeam.league  _displayUnderLeagueID,
	refHomeTeamLeague.ref_data_desc _displayUnderLeagueName,
	'' _displayUnderTeamID,
	'' _displayUnderTeamName,
	 
	refIconPath.ref_data_desc _iconPath,
    refMLBIcon.ref_data_desc _MLBIcon,
	refLeagueIcon.ref_data_desc _leagueIcon,
	'' _teamIcon,
	case games.season_game_type
		when 'R' then 0
		when 'A' then 1
		when 'C' then 2
		when '1' then 3
		when '2' then 4
		when 'L' then 5
		when 'W' then 6
	end as _sortKey
from retrosheet.dbo.Game_Information games
join retrosheet.dbo.Reference_Data refGameType on games.season_game_type = refGameType.ref_data_code
                                              and refGameType.ref_data_type = 'season_game_type'
join retrosheet.dbo.team homeTeam on games.home_team_id = homeTeam.team_id
join retrosheet.dbo.Reference_Data refHomeTeamLeague on homeTeam.league = refHomeTeamLeague.ref_data_code
                                              and refHomeTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.team visitTeam on games.visiting_team_id = visitTeam.team_id
join retrosheet.dbo.Reference_Data refVisitTeamLeague on visitTeam.league = refVisitTeamLeague.ref_data_code
                                              and refVisitTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.Reference_Data refIconPath on refIconPath.ref_data_code = 'icon_path'
                                              and refIconPath.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_Data refLeagueIcon on homeTeam.League = refLeagueIcon.ref_data_code
                                                and refLeagueIcon.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_Data refTeamIcon on games.home_team_id = refTeamIcon.ref_data_code
                                              and refTeamIcon.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_data refMLBIcon on 'MLB' = refMLBIcon.ref_data_code
                                             and refMLBIcon.ref_data_type = 'icon'

  where games.season_game_type in ('C', '1', '2', 'L')
union all

---- regular season home games
select distinct(games.season_year) _seasonYear, 
    games.season_game_type _seasonGameType, 
    refGameType.ref_data_desc _seasonGameTypeDesc,
	games.game_date _gameDate,
    games.game_number _gameNumber,
	homeTeam.league _gameHomeLeagueID, 
    refHomeTeamLeague.ref_data_desc _gameHomeLeagueName, 
    games.home_team_id _gameHomeTeamID, 
    homeTeam.name _gameHomeTeamName, 
    homeTeam.city _gameHomeTeamCity, 
	visitTeam.league _gameVisitTeamID, 
    refVisitTeamLeague.ref_data_desc _gameVisitLeagueName, 
    games.visiting_team_id _gameVisitTeamID, 
    visitTeam.name _gameVisitTeamName,
    visitTeam.city _gameVisitTeamCity,
	games.game_id _gameID, 

	homeTeam.league _displayUnderLeagueID,   --- list under this league id 
	refHomeTeamLeague.ref_data_desc _displayUnderLeagueName,
	games.home_team_id _displayUnderTeamID,  --- list under this team id
	homeTeam.name _displayUnderTeamName,
	 
	refIconPath.ref_data_desc _iconPath,
    refMLBIcon.ref_data_desc _MLBIcon,
	refLeagueIcon.ref_data_desc _leagueIcon,
	refTeamIcon.ref_data_desc _teamIcon,

	case games.season_game_type
		when 'R' then 0
		when 'A' then 1
		when 'C' then 2
		when '1' then 3
		when '2' then 4
		when 'L' then 5
		when 'W' then 6
	end as _sortKey
from retrosheet.dbo.Game_Information games
join retrosheet.dbo.Reference_Data refGameType on games.season_game_type = refGameType.ref_data_code
                                              and refGameType.ref_data_type = 'season_game_type'
join retrosheet.dbo.team homeTeam on games.home_team_id = homeTeam.team_id
join retrosheet.dbo.Reference_Data refHomeTeamLeague on homeTeam.league = refHomeTeamLeague.ref_data_code
                                              and refHomeTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.team visitTeam on games.visiting_team_id = visitTeam.team_id
join retrosheet.dbo.Reference_Data refVisitTeamLeague on visitTeam.league = refVisitTeamLeague.ref_data_code
                                              and refVisitTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.Reference_Data refIconPath on refIconPath.ref_data_code = 'icon_path'
                                              and refIconPath.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_Data refLeagueIcon on homeTeam.League = refLeagueIcon.ref_data_code
                                                and refLeagueIcon.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_Data refTeamIcon on games.home_team_id = refTeamIcon.ref_data_code
                                              and refTeamIcon.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_data refMLBIcon on 'MLB' = refMLBIcon.ref_data_code
                                             and refMLBIcon.ref_data_type = 'icon'
 
  where games.season_game_type = 'R'
union all
---- regular season away games
select distinct(games.season_year) _seasonYear, 
    games.season_game_type _seasonGameType, 
    refGameType.ref_data_desc _seasonGameTypeDesc,
	games.game_date _gameDate,
    games.game_number _gameNumber,
	homeTeam.league _gameHomeLeagueID, 
    refHomeTeamLeague.ref_data_desc _gameHomeLeagueName, 
    games.home_team_id _gameHomeTeamID, 
    homeTeam.name _gameHomeTeamName, 
    homeTeam.city _gameHomeTeamCity, 
	visitTeam.league _gameVisitTeamID, 
    refVisitTeamLeague.ref_data_desc _gameVisitLeagueName, 
    games.visiting_team_id _gameVisitTeamID, 
    visitTeam.name _gameVisitTeamName,
    visitTeam.city _gameVisitTeamCity,
	games.game_id _gameID, 

	visitTeam.league _displayUnderLeagueID,      --- list under this league id   
	refVisitTeamLeague.ref_data_desc _displayUnderLeagueName,
	games.visiting_team_id _displayUnderTeamID,  --- list under this team id
	visitTeam.name _displayUnderTeamName,

	refIconPath.ref_data_desc _iconPath,
    refMLBIcon.ref_data_desc _MLBIcon,
	refLeagueIcon.ref_data_desc _leagueIcon,
	refTeamIcon.ref_data_desc _teamIcon,

	case games.season_game_type
		when 'R' then 0
		when 'A' then 1
		when 'C' then 2
		when '1' then 3
		when '2' then 4
		when 'L' then 5
		when 'W' then 6
	end as _sortKey
from retrosheet.dbo.Game_Information games
join retrosheet.dbo.Reference_Data refGameType on games.season_game_type = refGameType.ref_data_code
                                              and refGameType.ref_data_type = 'season_game_type'
join retrosheet.dbo.team homeTeam on games.home_team_id = homeTeam.team_id
join retrosheet.dbo.Reference_Data refHomeTeamLeague on homeTeam.league = refHomeTeamLeague.ref_data_code
                                              and refHomeTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.team visitTeam on games.visiting_team_id = visitTeam.team_id
join retrosheet.dbo.Reference_Data refVisitTeamLeague on visitTeam.league = refVisitTeamLeague.ref_data_code
                                              and refVisitTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.Reference_Data refIconPath on refIconPath.ref_data_code = 'icon_path'
                                              and refIconPath.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_Data refLeagueIcon on visitTeam.League = refLeagueIcon.ref_data_code
                                                and refLeagueIcon.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_Data refTeamIcon on games.visiting_team_id = refTeamIcon.ref_data_code
                                              and refTeamIcon.ref_data_type = 'icon'
--- use left join here because we may not want to used the licensed MLB icons
left join retrosheet.dbo.Reference_data refMLBIcon on 'MLB' = refMLBIcon.ref_data_code
                                             and refMLBIcon.ref_data_type = 'icon'
  
  where games.season_game_type = 'R') 
										unionTable
order by _seasonYear, _sortKey, _displayUnderLeagueID, _displayUnderTeamID, _gameDate, _gameID";



            // add System.Data.Linq assembly to the References
            using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
            {
                dbCtx.CommandTimeout = 120;  //2 minutes
                IEnumerable<_GameSelectionListItem> results = dbCtx.ExecuteQuery<_GameSelectionListItem>(sqlQuery).ToList();
                Console.WriteLine("_GameSelectionListItem record count " + results.Count());

                Collection<TreeViewModels.Season> Seasons = new Collection<TreeViewModels.Season>();

                TreeViewModels.Season season = new TreeViewModels.Season();
                TreeViewModels.SeasonGameType seasonGameType = new TreeViewModels.SeasonGameType();
                TreeViewModels.League league = new TreeViewModels.League();
                TreeViewModels.Team team = new TreeViewModels.Team();
                TreeViewModels.Game game = new TreeViewModels.Game();

                foreach (_GameSelectionListItem result in results)
                {
                    if (holdSeasonYear == "")
                    {
                        season = LoadTreeViewData_Season(season, result);
                        season.SeasonGameTypes = new List<TreeViewModels.SeasonGameType>();
                        holdSeasonYear = result._seasonYear;

                        seasonGameType = LoadTreeViewData_SeasonGameType(seasonGameType, result);
                        season.SeasonGameTypes.Add(seasonGameType);
                        holdSeasonGameType = result._seasonGameType;

                        league = LoadTreeViewData_League(league, result);
                        seasonGameType.Leagues = new List<TreeViewModels.League>();
                        seasonGameType.Leagues.Add(league);
                        holdDisplayUnderLeagueID = result._displayUnderLeagueID;

                        team = LoadTreeViewData_Team(team, result);
                        league.Teams = new List<TreeViewModels.Team>();
                        league.Teams.Add(team);
                        holdDisplayUnderTeamID = result._displayUnderTeamID;

                        game = LoadTreeViewData_Game(game, result);
                        team.Games = new List<TreeViewModels.Game>();
                        team.Games.Add(game);
                    }
                    else if (holdSeasonYear != result._seasonYear)
                    {
                        Seasons.Add(season);

                        season = new TreeViewModels.Season();
                        season = LoadTreeViewData_Season(season, result);
                        season.SeasonGameTypes = new List<TreeViewModels.SeasonGameType>();
                        holdSeasonYear = result._seasonYear;

                        seasonGameType = new TreeViewModels.SeasonGameType();
                        seasonGameType = LoadTreeViewData_SeasonGameType(seasonGameType, result);
                        season.SeasonGameTypes = new List<TreeViewModels.SeasonGameType>();
                        season.SeasonGameTypes.Add(seasonGameType);
                        holdSeasonGameType = result._seasonGameType;

                        league = new TreeViewModels.League();
                        league = LoadTreeViewData_League(league, result);
                        seasonGameType.Leagues = new List<TreeViewModels.League>();
                        seasonGameType.Leagues.Add(league);
                        holdDisplayUnderLeagueID = result._displayUnderLeagueID;

                        team = new TreeViewModels.Team();
                        team = LoadTreeViewData_Team(team, result);
                        league.Teams = new List<TreeViewModels.Team>();
                        league.Teams.Add(team);
                        holdDisplayUnderTeamID = result._displayUnderTeamID;

                        game = new TreeViewModels.Game();
                        game = LoadTreeViewData_Game(game, result);
                        team.Games = new List<TreeViewModels.Game>();
                        team.Games.Add(game);
                    }
                    else if (holdSeasonGameType != result._seasonGameType)
                    {
                        seasonGameType = new TreeViewModels.SeasonGameType();
                        seasonGameType = LoadTreeViewData_SeasonGameType(seasonGameType, result);
                        season.SeasonGameTypes.Add(seasonGameType);
                        holdSeasonGameType = result._seasonGameType;

                        league = new TreeViewModels.League();
                        league = LoadTreeViewData_League(league, result);
                        seasonGameType.Leagues = new List<TreeViewModels.League>();
                        seasonGameType.Leagues.Add(league);
                        holdDisplayUnderLeagueID = result._displayUnderLeagueID;

                        team = new TreeViewModels.Team();
                        team = LoadTreeViewData_Team(team, result);
                        league.Teams = new List<TreeViewModels.Team>();
                        league.Teams.Add(team);
                        holdDisplayUnderTeamID = result._displayUnderTeamID;

                        game = new TreeViewModels.Game();
                        game = LoadTreeViewData_Game(game, result);
                        team.Games = new List<TreeViewModels.Game>();
                        team.Games.Add(game);
                    }
                    else if (holdDisplayUnderLeagueID != result._displayUnderLeagueID)
                    {
                        league = new TreeViewModels.League();
                        league = LoadTreeViewData_League(league, result);
                        seasonGameType.Leagues.Add(league);
                        holdDisplayUnderLeagueID = result._displayUnderLeagueID;

                        team = new TreeViewModels.Team();
                        team = LoadTreeViewData_Team(team, result);
                        league.Teams = new List<TreeViewModels.Team>();
                        league.Teams.Add(team);
                        holdDisplayUnderTeamID = result._displayUnderTeamID;

                        game = new TreeViewModels.Game();
                        game = LoadTreeViewData_Game(game, result);
                        team.Games = new List<TreeViewModels.Game>();
                        team.Games.Add(game);
                    }
                    else if (holdDisplayUnderTeamID != result._displayUnderTeamID)
                    {
                        team = new TreeViewModels.Team();
                        team = LoadTreeViewData_Team(team, result);
                        league.Teams.Add(team);
                        holdDisplayUnderTeamID = result._displayUnderTeamID;

                        game = new TreeViewModels.Game();
                        game = LoadTreeViewData_Game(game, result);
                        team.Games = new List<TreeViewModels.Game>();
                        team.Games.Add(game);
                    }
                    else
                    {
                        game = new TreeViewModels.Game();
                        game = LoadTreeViewData_Game(game, result);
                        team.Games.Add(game);
                    }
                }
                Seasons.Add(season);

                return Seasons;
            }
        }

        private TreeViewModels.Game LoadTreeViewData_Game(TreeViewModels.Game game, _GameSelectionListItem result)
        {
            game.GameID = result._gameID;
            game.GameDate = result._gameDate;
            game.GameHomeTeamName = result._gameHomeLeagueName;
            game.GameVisitTeamName = result._gameVisitTeamName;
            if (result._gameNumber > 0)
            {
                game.GameDesc = result._gameHomeTeamName + " vs " + result._gameVisitTeamName + " @ " + result._gameHomeTeamCity + " on "
                    + result._gameDate.Date.ToShortDateString()
                    + " game " + result._gameNumber.ToString() + " of 2";
            }
            else
            {
                game.GameDesc = result._gameHomeTeamName + " vs " + result._gameVisitTeamName + " @ " + result._gameHomeTeamCity + " on " + result._gameDate.Date.ToShortDateString();
            }

            return game;
        }

        private TreeViewModels.Team LoadTreeViewData_Team(TreeViewModels.Team team, _GameSelectionListItem result)
        {
            team.TeamID = result._displayUnderTeamID;
            team.TeamName = result._displayUnderTeamName;
            team.TeamIcon = result._iconPath + result._teamIcon;

            return team;
        }

        private TreeViewModels.League LoadTreeViewData_League(TreeViewModels.League league, _GameSelectionListItem result)
        {
            league.LeagueID = result._displayUnderLeagueID;
            league.LeagueIcon = result._iconPath + result._leagueIcon;
            league.LeagueName = result._displayUnderLeagueName;

            return league;
        }

        private TreeViewModels.SeasonGameType LoadTreeViewData_SeasonGameType (TreeViewModels.SeasonGameType seasonGameType, _GameSelectionListItem result)
        {
            seasonGameType.GameType = result._seasonGameType;
            seasonGameType.GameTypeDesc = result._seasonGameTypeDesc;
            seasonGameType.GameTypeSortKey = result._sortKey.ToString();

            return seasonGameType;
        }

        private TreeViewModels.Season LoadTreeViewData_Season(TreeViewModels.Season season, _GameSelectionListItem result)
        {
            season.SeasonYear = result._seasonYear;
            season.SeasonIcon = result._iconPath + result._MLBIcon;
            return season;
        }

        private class _GameSelectionListItem
        {
            public string _seasonYear { get; set; }
            public string _seasonGameType { get; set; }
            public string _seasonGameTypeDesc { get; set; }

            public string _gameID { get; set; }
            public DateTime _gameDate { get; set; }
            public int _gameNumber { get; set; }

            public string _gameHomeLeagueID { get; set; }
            public string _gameHomeLeagueName { get; set; }
            public string _gameHomeTeamID { get; set; }
            public string _gameHomeTeamName { get; set; }
            public string _gameHomeTeamCity { get; set; }

            public string _gameVisitLeagueID { get; set; }
            public string _gameVisitLeagueName { get; set; }
            public string _gameVisitTeamID { get; set; }
            public string _gameVisitTeamName { get; set; }
            public string _gameVisitTeamCity { get; set; }

            public string _displayUnderLeagueID { get; set; }
            public string _displayUnderLeagueName { get; set; }
            public string _displayUnderTeamID { get; set; }
            public string _displayUnderTeamName { get; set; }

            public string _iconPath { get; set; }
            public string _MLBIcon { get; set; }
            public string _leagueIcon { get; set; }
            public string _teamIcon { get; set; }

            public int _sortKey { get; set; }
        }

    }
}


