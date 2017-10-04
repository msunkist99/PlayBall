using System;
using System.Linq;
using Retrosheet_Persist;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Retrosheet_RetrieveData
{
	public class RetrieveData
	{
		private Dictionary<string, DataModels.ReferenceData> referenceDataDictionary = new Dictionary<string, DataModels.ReferenceData>();
		private Dictionary<string, DataModels.PlayerInformation> playerDictionary = new Dictionary<string, DataModels.PlayerInformation>();

		public string GameId { get; private set; }
		public string SeasonYear { get; private set; }
		public string SeasonGameType { get; private set; }
		public string HomeTeamID { get; private set; }
		public string HomeTeamName { get; private set; }
		public string VisitingTeamID { get; private set; }
		public string VisitingTeamName { get; private set; }
		public string PageTitle { get; set; }
	  

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

		public void RetrieveReferenceData()
		{
				string sqlQuery = @"select 
						record_id recordID
						, ref_data_type referenceDataType
						, ref_data_code referenceDataCode
						, ref_data_desc referenceDataDesc
						from reference_data;";

				// add System.Data.Linq assembly to the References
				using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
				{
					//dbCtx.CommandTimeout = 120;  //2 minutes
					IEnumerable<DataModels.ReferenceData> results = dbCtx.ExecuteQuery<DataModels.ReferenceData>(sqlQuery).ToList();
					Console.WriteLine("DataModels.ReferenceData record count " + results.Count());

					//Collection<DataModels.GameInformation> GameInformation = new Collection<DataModels.GameInformation>();

					foreach (DataModels.ReferenceData result in results)
					{
						referenceDataDictionary.Add(result.referenceDataType + result.referenceDataCode, result);
					}
				}

		}

		public Collection<DataModels.GameInformationItem> RetrieveGameInformation(string gameID)
		{
			string sqlQuery = @"SELECT  
		gameInfo.record_id RecordID
	  , gameInfo.game_id GameID 
	  , gameInfo.visiting_team_id VisitingTeamID 
	  , gameInfo.home_team_id HomeTeamID 
	  , gameInfo.game_date GameDate 
	  , gameInfo.game_number GameNumber 
	  , gameInfo.start_time StartTime 
	  , gameInfo.day_night DayNight 
	  , gameInfo.used_dh UsedDH 
	  , gameInfo.pitches Pitches 
	  , gameInfo.umpire_home_id UmpireHomeID 
	  , gameInfo.umpire_first_base_id UmpireFirstBaseID 
	  , gameInfo.umpire_second_base_id UmpireSecondBaseID 
	  , gameInfo.umpire_third_base_id UmpireThirdBaseID 
	  , gameInfo.field_condition FieldCondition 
	  , gameInfo.precipitation Precipitation 
	  , gameInfo.sky Sky 
	  , gameInfo.temperature Temperature 
	  , gameInfo.wind_direction WindDirection 
	  , gameInfo.wind_speed WindSpeed 
	  , gameInfo.game_time_length_minutes GameTimeLengthMinutes 
	  , gameInfo.attendance Attendance 
	  , gameInfo.ballpark_id BallparkID 
	  , gameInfo.winning_pitcher_id WinningPitcherID 
	  , gameInfo.losing_pitcher_id LosingPitcherID 
	  , gameInfo.save_pitcher_id SavePitcherID 
	  , gameInfo.winning_rbi_player_id WinningRBIPlayerID 
	  , gameInfo.oscorer Oscorer 
	  , gameInfo.season_year SeasonYear 
	  , gameInfo.season_game_type SeasonGameType 
	  , gameInfo.edit_time EditTime 
	  , gameInfo.how_scored HowScored 
	  , gameInfo.input_prog_vers InputProgVers 
	  , gameInfo.inputter Inputter 
	  , gameInfo.input_time InputTime 
	  , gameInfo.scorer Scorer 
	  , gameInfo.translator Translator 
	  , homeTeam.league HomeTeamLeague 
	  , homeTeam.name HomeTeamName 
	  , homeTeam.city HomeTeamCity 
	  , visitTeam.league VisitTeamLeague 
	  , visitTeam.name VisitTeamName 
	  , visitTeam.city VisitTeamCity   
	  , ballpark.aka BallparkAKA 
	  , ballpark.city BallparkCity 
	  , ballpark.name BallparkName 
	  , ballpark.notes BallparkNotes 
	  , firstBaseUmpire.last_name UmpireFirstBaseLastName 
	  , firstBaseUmpire.first_name UmpireFirstBaseFirstName 
	  , secondBaseUmpire.last_name UmpireSecondBaseLastName 
	  , secondBaseUmpire.first_name UmpireSecondBaseFirstName 
	  , thirdBaseUmpire.last_name UmpireThirdBaseLastName 
	  , thirdBaseUmpire.first_name UmpireThirdBaseFirstName 
	  , homeUmpire.last_name UmpireHomeLastName 
	  , homeUmpire.first_name UmpireHomeFirstName 
  FROM Game_Information gameInfo
  join Team homeTeam on gameInfo.home_team_id = homeTeam.team_id
								and gameInfo.season_year = homeTeam.season_year
								and gameInfo.season_game_type = homeTeam.season_game_type
  join Team visitTeam on gameInfo.visiting_team_id = visitTeam.team_id
								and gameInfo.season_year = visitTeam.season_year
								and gameInfo.season_game_type = visitTeam.season_game_type
  join Ballpark ballpark on gameInfo.ballpark_id = ballpark.ballpark_id
  left join Personnel firstBaseUmpire on gameInfo.umpire_first_base_id = firstBaseUmpire.person_id
								and firstBaseUmpire.role = 'U'
  left join Personnel secondBaseUmpire on gameInfo.umpire_second_base_id = secondBaseUmpire.person_id
								and secondBaseUmpire.role = 'U'
  left join Personnel thirdBaseUmpire on gameInfo.umpire_third_base_id = thirdBaseUmpire.person_id
								and thirdBaseUmpire.role = 'U'
  left join Personnel homeUmpire on gameInfo.umpire_home_id = homeUmpire.person_id
								and homeUmpire.role = 'U'
  where game_id = 'x_game_id'";

			sqlQuery = sqlQuery.Replace("x_game_id", gameID);


			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.GameInformation> results = dbCtx.ExecuteQuery<DataModels.GameInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.GameInformation record count " + results.Count());

				Collection<DataModels.GameInformation> GameInformation = new Collection<DataModels.GameInformation>();

				// only one GameInformation record will return from the database
				DataModels.GameInformation result = results.First<DataModels.GameInformation>();

				GameId = result.GameID;
				SeasonYear = result.SeasonYear;
				SeasonGameType = result.SeasonGameType;
				HomeTeamID = result.HomeTeamID;
				VisitingTeamID = result.VisitingTeamID;

				HomeTeamName = result.HomeTeamName;
				VisitingTeamName = result.VisitTeamName;


				if (result.GameNumber > 0)
				{
					PageTitle = result.HomeTeamName + " vs " + result.VisitTeamName + " @ " + result.HomeTeamCity + " on "
								+ result.GameDate.ToShortDateString()
								+ " game " + result.GameNumber.ToString() + " of 2";
				}
				else
				{
					PageTitle = result.HomeTeamName + " vs " + result.VisitTeamName + " @ " + result.HomeTeamCity + " on "
								+ result.GameDate.ToShortDateString();
				}

				if (result.GameNumber > 0)
				{
					result.GameNumberDesc = "game " + result.GameNumber + " of doubleheader";
				}
				else
				{
					result.GameNumberDesc = null;
				}

				
				result.VisitTeamLeagueName = RetrieveReferenceDataDesc("team_league", result.VisitTeamLeague);
				result.HomeTeamLeagueName = RetrieveReferenceDataDesc("team_league", result.HomeTeamLeague);
				result.SeasonGameTypeDesc = RetrieveReferenceDataDesc("season_game_type", result.SeasonGameType);
				result.WindDirectionDesc = RetrieveReferenceDataDesc("wind_direction", result.WindDirection);

				//build the collection of formatted individual game information items
				Collection<DataModels.GameInformationItem> gameInformationItems = new Collection<DataModels.GameInformationItem>();

				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "", GameItemValue = result.SeasonYear + " " + result.SeasonGameTypeDesc + " game" });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Visiting Team", GameItemValue = result.VisitTeamLeagueName + " League " + result.VisitTeamCity + " " + result.VisitTeamName });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Home Team", GameItemValue = result.HomeTeamLeagueName + " League " + result.HomeTeamCity + " " + result.HomeTeamName });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Game Date/Time", GameItemValue = result.GameDate.ToShortDateString() + "  " + result.StartTime + " " + result.DayNight + " game" });

				if (result.GameNumberDesc != null)
				{
					gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "", GameItemValue = result.GameNumberDesc });

				}


				if (result.UsedDH == "Y")
				{
					gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "", GameItemValue = "Designated hitter for pitcher" });
				}

				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Field Condition", GameItemValue = result.FieldCondition });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Precipitation", GameItemValue = result.Precipitation });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Sky", GameItemValue = result.Sky });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Tempurature", GameItemValue = result.Temperature.ToString() + " degrees fahrenheit" });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Wind Direction", GameItemValue = result.WindDirectionDesc });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Wind Speed", GameItemValue = result.WindSpeed.ToString() + " mph" });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Game Length", GameItemValue = result.GameTimeLengthMinutes.ToString("N0") + " minutes" });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Attendance", GameItemValue = result.Attendance.ToString("N0") });
				if (result.BallparkAKA != "")
				{
					gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Ballpark", GameItemValue = result.BallparkName + " aka " + result.BallparkAKA });
				}
				else
				{
					gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Ballpark", GameItemValue = result.BallparkName });

				}
				if (result.BallparkNotes != "")
				{
					gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "", GameItemValue = result.BallparkNotes });
				}

				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Umpire Home Plate", GameItemValue = string.Concat(result.UmpireHomeLastName, ", ", result.UmpireHomeFirstName) });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Umpire First Base", GameItemValue = string.Concat(result.UmpireFirstBaseLastName, ", ", result.UmpireFirstBaseFirstName) });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Umpire Second Base", GameItemValue = string.Concat(result.UmpireSecondBaseLastName, ", ", result.UmpireSecondBaseFirstName) });
				gameInformationItems.Add(new DataModels.GameInformationItem() { GameItemDesc = "Umpire Third Base", GameItemValue = string.Concat(result.UmpireThirdBaseLastName, ", ", result.UmpireThirdBaseFirstName) });

				return gameInformationItems;
			}
		}

		public Collection<DataModels.StartingPlayerInformation> RetrieveStartingPlayers(string seasonYear, string seasonGameType, string gameID)
		{
			string sqlQuery = @"select
  starting_player.record_id RecordID
, starting_player.game_id GameID
, starting_player.player_id PlayerID
, player.last_name PlayerLastName
, player.first_name PlayerFirstName
, starting_player.game_team_code GameTeamCode
, starting_player.batting_order BattingOrder
, starting_player.field_position FieldPosition
, player.throws Throws
, player.bats Bats
, starting_player.team_id TeamID
, team.name TeamName 
from starting_player
join team on starting_player.team_id = team.team_id
		   and team.season_year = 'x_season_year'
		   and team.season_game_type = 'x_season_game_type'
join player on starting_player.player_id = player.player_id
		   and player.team_id = Starting_Player.team_id
		   and player.season_year = 'x_season_year'
		   and player.season_game_type = 'x_season_game_type'
where game_id = 'x_game_id'";

			sqlQuery = sqlQuery.Replace("x_season_year", seasonYear);
			sqlQuery = sqlQuery.Replace("x_season_game_type", seasonGameType);
			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.StartingPlayerInformation> StartingPlayerInformation = new Collection<DataModels.StartingPlayerInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.StartingPlayerInformation> results = dbCtx.ExecuteQuery<DataModels.StartingPlayerInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.StartingPlayerInformation record count " + results.Count());

				foreach (DataModels.StartingPlayerInformation result in results)
				{
					result.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
					result.FieldPositionDesc = RetrieveReferenceDataDesc("field_position", result.FieldPosition.ToString());

					result.ThrowsDesc = RetrieveReferenceDataDesc("throws", result.Throws);
					result.BatsDesc = RetrieveReferenceDataDesc("bats", result.Bats);
					if (result.BattingOrder == 0)
					{
						result.BattingOrderDesc = "";
					}
					else
					{
						result.BattingOrderDesc = result.BattingOrder.ToString();
					}

					StartingPlayerInformation.Add(result);

					DataModels.PlayerInformation playerInformation = new DataModels.PlayerInformation();
					playerInformation.RecordID = result.RecordID;
					playerInformation.PlayerID = result.PlayerID;
					playerInformation.PlayerLastName = result.PlayerLastName;
					playerInformation.PlayerFirstName = result.PlayerFirstName;
					playerInformation.PlayerThrows = result.Throws;
					playerInformation.PlayerThrowsDesc = result.ThrowsDesc;
					playerInformation.PlayerBats = result.Bats;
					playerInformation.PlayerBatsDesc = result.BatsDesc;
					playerInformation.PlayerTeamID = result.TeamID;
					playerInformation.PlayerTeamName = result.TeamName;
					playerInformation.PlayerFieldPosition = result.FieldPosition.ToString();
					playerInformation.PlayerFieldPositionDesc = result.FieldPositionDesc;
					playerInformation.SeasonYear = seasonYear;
					playerInformation.SeasonGameType = seasonGameType;
					playerDictionary.Add(playerInformation.PlayerID, playerInformation);
				}
			}
			return StartingPlayerInformation;
		}

		public Collection<DataModels.PlayInformation> RetrievePlay(string seasonYear,
																   string seasonGameType,
																   string homeTeamID,
																   string visitingTeamID,
																   string gameID)
		{
			string sqlQuery = @"select   
play.record_id RecordID 
, play.game_id GameID 
, play.inning Inning 
, play.game_team_code GameTeamCode 
, play.sequence Sequence 
, play.player_id PlayerID 
, player.last_name PlayerLastName 
, player.first_name PlayerFirstName 
, play.count_balls CountBalls 
, play.count_strikes CountStrikes 
, play.pitches Pitches 
, play.event_sequence EventSequence 
, play.event_modifier EventModifier 
, play.event_runner_advance EventRunnerAdvance 
, play.event_hit_location EventHitLocation 
, team.team_id TeamID 
, team.name TeamName
, play.event_fielded_by EventFieldedBy 
, play.event_play_on_runner EventPlayOnRunner 
, play.event_type EventType 
, play.event_columnSix EventColumnSix 
from play  
join team on team.season_year = 'x_season_year'  
		and team.season_game_type = 'x_season_game_type'              
		and team.team_id = (          
		case play.game_team_code        
			when 0 then 'x_visiting_team_id'        
			else 'x_home_team_id'         
		end)     
join player on player.player_id = play.player_id             
		and player.season_year = 'x_season_year'             
		and player.season_game_type = 'x_season_game_type'             
		and player.team_id = (            
		case play.game_team_code         
			when 0 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)             
where play.game_id = 'x_game_id'
	order by play.inning,
	play.game_team_code,
	play.sequence";

			sqlQuery = sqlQuery.Replace("x_season_year", seasonYear);
			sqlQuery = sqlQuery.Replace("x_season_game_type", seasonGameType);
			sqlQuery = sqlQuery.Replace("x_visiting_team_id", visitingTeamID);
			sqlQuery = sqlQuery.Replace("x_home_team_id", homeTeamID);
			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.PlayInformation> PlayInformation = new Collection<DataModels.PlayInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.PlayInformation> results = dbCtx.ExecuteQuery<DataModels.PlayInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.PlayInformation record count " + results.Count());

				foreach (DataModels.PlayInformation result in results)
				{
					var pitchCodes = result.Pitches.ToCharArray();
					int pitchCodeCount = pitchCodes.Count();
					int x = 0;
					foreach (char pitchCode in pitchCodes)
					{
						x++;
						if (x < pitchCodeCount)
						{
							result.PitchDesc = result.PitchDesc + RetrieveReferenceDataDesc("pitch_code", pitchCode.ToString()) + ", ";
						}
						else
						{
							result.PitchDesc = result.PitchDesc + RetrieveReferenceDataDesc("pitch_code", pitchCode.ToString());
						}
					}

					result.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());

					string[] event_modifiers = result.EventModifier.Split('/');
					int event_modifier_count = event_modifiers.Count();
					x = 0;
					foreach (string event_modifier in event_modifiers)
					{
						x++;
						if (x < event_modifier_count)
						{
							result.EventModifierDesc = result.EventModifierDesc + RetrieveReferenceDataDesc("play_modifier", event_modifier) + ", ";
						}
						else
						{
							result.EventModifierDesc = result.EventModifierDesc + RetrieveReferenceDataDesc("play_modifier", event_modifier);
						}
					}

					char[] event_fielded_by_values = result.EventFieldedBy.ToCharArray();
					int event_fielded_by_count = event_fielded_by_values.Count();
					x = 0;
					foreach (char event_field_by_value in event_fielded_by_values)
					{
						x++;
						if (x < event_fielded_by_count)
						{
							result.EventFieldedByDesc = result.EventFieldedByDesc + RetrieveReferenceDataDesc("field_position", event_field_by_value.ToString()) + ", ";
						}
						else
						{
							result.EventFieldedByDesc = result.EventFieldedByDesc + RetrieveReferenceDataDesc("field_position", event_field_by_value.ToString()) ;
						}
					}

					result.EventTypeDesc = RetrieveReferenceDataDesc("event_type", result.EventType);
					result.EventModifierDesc = RetrieveReferenceDataDesc("play_modifier", result.EventModifier);

					if((result.EventModifierDesc != "") && (result.EventTypeDesc !=""))
					{
						result.EventModifierDesc = result.EventModifierDesc + ", " + result.EventTypeDesc;
					}
					else if (result.EventTypeDesc != "")
					{
						result.EventModifierDesc = result.EventTypeDesc;
					}
					result.EventHitLocationDesc = RetrieveReferenceDataDesc("hit_location", result.EventHitLocation);
					if (result.EventHitLocation != "")
					{
						result.EventHitLocationDiagram = RetrieveReferenceDataDesc("hit_location", "diagram_path") + RetrieveReferenceDataDesc("hit_diagram", result.EventHitLocation);
					}
					else
					{
						result.EventHitLocationDiagram = RetrieveReferenceDataDesc("hit_location", "diagram_path") + "blank.jpg";
					}

					result.PlayerName = string.Concat(result.PlayerLastName, ", ", result.PlayerFirstName);
					result.Count = string.Concat(result.CountBalls, " / ", result.CountStrikes);
					PlayInformation.Add(result);
				}
			}
				return PlayInformation;
		}

		public Collection<DataModels.PlayBeventInformation> RetrievePlayBevent(string seasonYear,
																			   string seasonGameType,
																			   string homeTeamID,
																			   string visitingTeamID,
																			   string gameID)
		{
			int visitingTeamHitCount = 0;
			int visitingTeamErrorCount = 0;

			int homeTeamHitCount = 0;
			int homeTeamErrorCount = 0;

			string sqlQuery = @"select   
  play_bevent.record_id RecordID 
, play_bevent.game_id GameID 
, play_bevent.inning Inning 
, play_bevent.game_team_code GameTeamCode 
, play_bevent.event_num EventNum 
, play_bevent.batter_player_id BatterPlayerID 
, play_bevent.batter_defensive_position BatterDefPosition
, player.last_name BatterLastName 
, player.first_name BatterFirstName 
, play_bevent.batter_lineup_position BatterLineUpPosition
, Play_Bevent.outs CountOuts
, play_bevent.count_balls CountBalls 
, play_bevent.count_strikes CountStrikes 
, play_bevent.pitch_sequence Pitches 
, play_bevent.batted_ball_type BattedBallType
, play_bevent.batter_dest DestBatter
, Play_Bevent.first_runner_dest DestFirstRunner
, Play_Bevent.second_runner_dest DestSecondRunner
, Play_Bevent.third_runner_dest DestThirdRunner
, play_bevent.hit_location HitLocation
, team.team_id TeamID 
, team.name TeamName
, play_bevent.fielded_by FieldedBy 
, Play_Bevent.play_on_batter PlayOnBatter
, Play_Bevent.play_on_first_runner PlayOnFirstRunner
, Play_Bevent.play_on_second_runner PlayOnSecondRunner
, Play_Bevent.play_on_third_runner PlayOnThirdRunner
, play_bevent.event_type EventType
, catcher.player_id CatcherPlayerID 
, catcher.last_name CatcherLastName
, catcher.first_name CatcherFirstName
, catcher.bats CatcherBats
, catcher.throws CatcherThrows
, firstBase.player_id FirstBasePlayerID
, firstBase.last_name FirstBaseLastName
, firstBase.first_name FirstBaseFirstName
, firstBase.bats FirstBaseBats
, firstBase.throws FirstBaseThrows
, secondBase.player_id SecondBasePlayerID
, secondBase.last_name SecondBaseLastName
, secondBase.first_name SecondBaseFirstName
, secondBase.bats SecondBaseBats
, secondBase.throws SecondBaseThrows
, thirdbase.player_id ThirdBasePlayerID
, thirdBase.last_name ThirdBaseLastName
, thirdBase.first_name ThirdBaseFirstName
, thirdBase.bats ThirdBaseBats
, thirdBase.throws ThirdBaseThrows
, shortstop.player_id ShortStopPlayerID
, shortStop.last_name ShortStopLastName
, shortStop.first_name ShortStopFirstName
, shortStop.bats ShortStopBats
, shortStop.throws ShortStopThrows
, leftField.player_id LeftFieldPlayerID
, leftField.last_name LeftFieldLastName
, leftField.first_name LeftFieldFirstName
, leftField.throws LeftFieldThrows
, centerField.player_id CenterFieldPlayerID
, centerField.last_name CenterFieldLastName
, centerField.first_name CenterFieldFirstName
, centerField.bats CenterFieldBats
, centerField.throws CenterFieldThrows
, rightField.player_id RightFieldPlayerID
, rightField.last_name RightFieldLastName
, rightField.first_name RightFieldFirstName
, rightField.bats RightFieldBats
, rightField.throws RightFieldThrows
, pitcher.player_id PitcherPlayerID
, pitcher.last_name PitcherLastName
, pitcher.bats PitcherBats
, play_bevent.first_error_position	FirstErrorPosition
, play_bevent.first_error_type		FirstErrorType
, play_bevent.second_error_position	SecondErrorPosition
, play_bevent.second_error_type		SecondErrorType
, play_bevent.third_error_position	ThirdErrorPosition
, play_bevent.third_error_type		ThirdErrorType

, play_bevent.runner_first_removed_pinch_player_id  RunnerFirstRemovedForPinchPlayerID
, runnerFirstRemovedPinch.last_name                 RunnerFirstRemovedForPinchLastName
, runnerFirstRemovedPinch.first_name                RunnerFirstRemovedForPinchFirstName
, runnerFirstRemovedPinch.bats                      RunnerFirstRemovedForPinchBats
, runnerFirstRemovedPinch.throws                    RunnerFirstRemovedForPinchThrows

, play_bevent.runner_second_removed_pinch_player_id RunnerSecondRemovedForPinchPlayerID
, runnerSecondRemovedPinch.last_name                RunnerSecondRemovedForPinchLastName
, runnerSecondRemovedPinch.first_name               RunnerSecondRemovedForPinchFirstName
, runnerSecondRemovedPinch.bats                     RunnerSecondRemovedForPinchBats
, runnerSecondRemovedPinch.throws                   RunnerSecondRemovedForPinchThrows

, play_bevent.runner_third_removed_pinch_player_id  RunnerThirdRemovedForPinchPlayerID
, runnerThirdRemovedPinch.last_name                 RunnerThirdRemovedForPinchLastName
, runnerThirdRemovedPinch.first_name                RunnerThirdRemovedForPinchFirstName
, runnerThirdRemovedPinch.bats                      RunnerThirdRemovedForPinchBats
, runnerThirdRemovedPinch.throws                    RunnerThirdRemovedForPinchThrows

, play_bevent.batter_removed_pinch_player_id        BatterRemovedforPinchPlayerID
, batterRemovedPinch.last_name                      BatterRemovedforPinchLastName
, batterRemovedPinch.first_name                     BatterRemovedforPinchFirstName
, batterRemovedPinch.bats                           BatterRemovedforPinchBaseBats
, batterRemovedPinch.throws                         BatterRemovedforPinchBaseThrows

, play_bevent.batter_removed_position_pinch         BatterRemovedForPinchFieldPosition

, play_bevent.visiting_score                        VisitingTeamScore
, play_bevent.home_score                            HomeTeamScore
, play_bevent.hit_value                             HitValue
, play_bevent.num_errors                            NumErrors
from play_bevent  
join team on team.season_year = 'x_season_year'  
		and team.season_game_type = 'x_season_game_type'              
		and team.team_id = (          
		case play_bevent.game_team_code        
			when 0 then 'x_visiting_team_id'        
			else 'x_home_team_id'         
		end)     
join player on player.player_id = play_bevent.batter_player_id             
		and player.season_year = 'x_season_year'             
		and player.season_game_type = 'x_season_game_type'             
		and player.team_id = (            
		case play_bevent.game_team_code         
			when 0 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)    
join player catcher on catcher.player_id = play_bevent.catcher_player_id             
		and catcher.season_year = 'x_season_year'             
		and catcher.season_game_type = 'x_season_game_type'             
		and catcher.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)           
join player firstBase on firstBase.player_id = play_bevent.first_base_player_id             
		and firstBase.season_year = 'x_season_year'             
		and firstBase.season_game_type = 'x_season_game_type'             
		and firstBase.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)           
join player secondBase on secondBase.player_id = play_bevent.second_base_player_id             
		and secondBase.season_year = 'x_season_year'             
		and secondBase.season_game_type = 'x_season_game_type'             
		and secondBase.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)           
join player thirdBase on thirdBase.player_id = play_bevent.third_base_player_id             
		and thirdBase.season_year = 'x_season_year'             
		and thirdBase.season_game_type = 'x_season_game_type'             
		and thirdBase.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)           
join player shortstop on shortstop.player_id = play_bevent.shortstop_player_id             
		and shortstop.season_year = 'x_season_year'             
		and shortstop.season_game_type = 'x_season_game_type'             
		and shortstop.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)           
join player leftField on leftField.player_id = play_bevent.left_field_player_id             
		and leftField.season_year = 'x_season_year'             
		and leftField.season_game_type = 'x_season_game_type'             
		and leftField.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)           
join player centerField on centerField.player_id = play_bevent.center_field_player_id             
		and centerField.season_year = 'x_season_year'             
		and centerField.season_game_type = 'x_season_game_type'             
		and centerField.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)           
join player rightField on rightField.player_id = play_bevent.right_field_player_id             
		and rightField.season_year = 'x_season_year'             
		and rightField.season_game_type = 'x_season_game_type'             
		and rightField.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)           
join player pitcher on pitcher.player_id = play_bevent.pitcher_player_id             
		and pitcher.season_year = 'x_season_year'             
		and pitcher.season_game_type = 'x_season_game_type'             
		and pitcher.team_id = (            
		case play_bevent.game_team_code         
			when 1 then 'x_visiting_team_id'        
			else 'x_home_team_id'       
		end)

left join player runnerFirstRemovedPinch on runnerFirstRemovedPinch.player_id = play_bevent.runner_first_removed_pinch_player_id
		and runnerFirstRemovedPinch.season_year = 'x_season_year'
		and runnerFirstRemovedPinch.season_game_type = 'x_season_game_type'
		and runnerFirstRemovedPinch.team_id = (
		case play_bevent.game_team_code
			when 1 then 'x_home_team_id'
			else 'x_visiting_team_id'
		end)

left join player runnerSecondRemovedPinch on runnerSecondRemovedPinch.player_id = play_bevent.runner_second_removed_pinch_player_id
		and runnerSecondRemovedPinch.season_year = 'x_season_year'
		and runnerSecondRemovedPinch.season_game_type = 'x_season_game_type'
		and runnerSecondRemovedPinch.team_id = (
		case play_bevent.game_team_code
			when 1 then 'x_home_team_id'
			else 'x_visiting_team_id'
		end)
	 
left join player runnerThirdRemovedPinch on runnerThirdRemovedPinch.player_id = play_bevent.runner_third_removed_pinch_player_id
		and runnerThirdRemovedPinch.season_year = 'x_season_year'
		and runnerThirdRemovedPinch.season_game_type = 'x_season_game_type'
		and runnerThirdRemovedPinch.team_id = (
		case play_bevent.game_team_code
			when 1 then 'x_home_team_id'
			else 'x_visiting_team_id'
		end)

left join player batterRemovedPinch on batterRemovedPinch.player_id = play_bevent.batter_removed_pinch_player_id
		and batterRemovedPinch.season_year = 'x_season_year'
		and batterRemovedPinch.season_game_type = 'x_season_game_type'
		and batterRemovedPinch.team_id = (
		case play_bevent.game_team_code
			when 1 then 'x_home_team_id'
			else 'x_visiting_team_id'
		end)
	 
where play_bevent.game_id = 'x_game_id'
	order by play_bevent.inning,
	play_bevent.event_num";

			sqlQuery = sqlQuery.Replace("x_season_year", seasonYear);
			sqlQuery = sqlQuery.Replace("x_season_game_type", seasonGameType);
			sqlQuery = sqlQuery.Replace("x_visiting_team_id", visitingTeamID);
			sqlQuery = sqlQuery.Replace("x_home_team_id", homeTeamID);
			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.PlayBeventInformation> PlayBeventInformation = new Collection<DataModels.PlayBeventInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.PlayBeventInformation> results = dbCtx.ExecuteQuery<DataModels.PlayBeventInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.PlayBeventInformation record count " + results.Count());

				foreach (DataModels.PlayBeventInformation result in results)
				{
					var pitchCodes = result.Pitches.ToCharArray();
					int pitchCodeCount = pitchCodes.Count();
					int x = 0;
					foreach (char pitchCode in pitchCodes)
					{
						x++;
						if (x < pitchCodeCount)
						{
							result.PitchDesc = result.PitchDesc + RetrieveReferenceDataDesc("pitch_code", pitchCode.ToString()) + ", ";
						}
						else
						{
							result.PitchDesc = result.PitchDesc + RetrieveReferenceDataDesc("pitch_code", pitchCode.ToString());
						}
					}

					result.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
					if (result.GameTeamCode == 0)
					{
						result.TopBottom = "⌃";
					}
					else
					{
						result.TopBottom = "⌄";
					}

					result.EventTypeDesc = RetrieveReferenceDataDesc("event_type", result.EventType);

					result.BatterName = string.Concat(result.BatterLastName, ", ", result.BatterFirstName);
					result.BatterDefPositionDesc = RetrieveReferenceDataDesc("field_position", result.BatterDefPosition.ToString());
					result.BattedBallTypeDesc = RetrieveReferenceDataDesc("batted_ball_type", result.BattedBallType);

					result.DestBatterDesc = RetrieveReferenceDataDesc("runner_destination", result.DestBatter.ToString());
					result.DestFirstRunnerDesc = RetrieveReferenceDataDesc("runner_destination", result.DestFirstRunner.ToString());
					result.DestSecondRunnerDesc = RetrieveReferenceDataDesc("runner_destination", result.DestSecondRunner.ToString());
					result.DestThirdRunnerDesc = RetrieveReferenceDataDesc("runner_destination", result.DestThirdRunner.ToString());

					result.CatcherName = string.Concat(result.CatcherLastName, ", ", result.CatcherFirstName);
					result.CatcherBatsDesc = RetrieveReferenceDataDesc("bats", result.CatcherBats);
					result.CatcherThrowsDesc = RetrieveReferenceDataDesc("throws", result.CatcherThrows);

					result.FirstBaseName = string.Concat(result.FirstBaseLastName, ", ", result.FirstBaseFirstName);
					result.FirstBaseBatsDesc = RetrieveReferenceDataDesc("bats", result.FirstBaseBats);
					result.FirstBaseThrowsDesc = RetrieveReferenceDataDesc("throws", result.FirstBaseThrows);

					result.SecondBaseName = string.Concat(result.SecondBaseLastName, ", ", result.SecondBaseFirstName);
					result.SecondBaseBatsDesc = RetrieveReferenceDataDesc("bats", result.SecondBaseBats);
					result.SecondBaseThrowsDesc = RetrieveReferenceDataDesc("throws", result.SecondBaseThrows);

					result.ThirdBaseName = string.Concat(result.ThirdBaseLastName, ", ", result.ThirdBaseFirstName);
					result.ThirdBaseBatsDesc = RetrieveReferenceDataDesc("bats", result.ThirdBaseBats);
					result.ThirdBaseThrowsDesc = RetrieveReferenceDataDesc("throws", result.ThirdBaseThrows);

					result.ShortStopName = string.Concat(result.ShortStopLastName, ", ", result.ShortStopFirstName);
					result.ShortStopBatsDesc = RetrieveReferenceDataDesc("bats", result.ShortStopBats);
					result.ShortStopThrowsDesc = RetrieveReferenceDataDesc("throws", result.ShortStopThrows);

					result.LeftFieldName = string.Concat(result.LeftFieldLastName, ", ", result.LeftFieldFirstName);
					result.LeftFieldBatsDesc = RetrieveReferenceDataDesc("bats", result.LeftFieldBats);
					result.LeftFieldThrowsDesc = RetrieveReferenceDataDesc("throws", result.LeftFieldThrows);

					result.CenterFieldName = string.Concat(result.CenterFieldLastName, ", ", result.CenterFieldFirstName);
					result.CenterFieldBatsDesc = RetrieveReferenceDataDesc("bats", result.CenterFieldBats);
					result.CenterFieldThrowsDesc = RetrieveReferenceDataDesc("throws", result.CatcherThrows);

					result.RightFieldName = string.Concat(result.RightFieldLastName, ", ", result.RightFieldFirstName);
					result.RightFieldBatsDesc = RetrieveReferenceDataDesc("bats", result.RightFieldBats);
					result.RightFieldThrowsDesc = RetrieveReferenceDataDesc("throws", result.CatcherThrows);

					result.PitcherName = string.Concat(result.PitcherLastName, ", ", result.PitcherFirstName);
					result.PitcherBatsDesc = RetrieveReferenceDataDesc("bats", result.PitcherBats);
					result.PitcherThrowsDesc = RetrieveReferenceDataDesc("throws", result.PitcherThrows);

					if (result.PlayOnBatter != null)
						{
						x = 0;
						var playOn = result.PlayOnBatter.ToCharArray();
						int playOnCount = playOn.Count();
						foreach (char play in playOn)
						{
							x++;
							if (x < playOnCount)
							{
								result.PlayOnBatterDesc = result.PlayOnBatterDesc + RetrieveReferenceDataDesc("field_position", play.ToString()) + ", ";
							}
							else
							{
								result.PlayOnBatterDesc = result.PlayOnBatterDesc + RetrieveReferenceDataDesc("field_position", play.ToString());
							}
						}
					}

					if (result.PlayOnFirstRunner != null)
					{
						x = 0;
						var playOn = result.PlayOnFirstRunner.ToCharArray();
						int playOnCount = playOn.Count();
						foreach (char play in playOn)
						{
							x++;
							if (x < playOnCount)
							{
								result.PlayOnFirstRunnerDesc = result.PlayOnFirstRunnerDesc + RetrieveReferenceDataDesc("field_position", play.ToString()) + ", ";
							}
							else
							{
								result.PlayOnFirstRunnerDesc = result.PlayOnFirstRunnerDesc + RetrieveReferenceDataDesc("field_position", play.ToString());
							}
						}
					}

					if(result.PlayOnSecondRunner != null)
					{
						x = 0;
						var playOn = result.PlayOnSecondRunner.ToCharArray();
						int playOnCount = playOn.Count();
						foreach (char play in playOn)
						{
							x++;
							if (x < playOnCount)
							{
								result.PlayOnSecondRunnerDesc = result.PlayOnSecondRunnerDesc + RetrieveReferenceDataDesc("field_position", play.ToString()) + ", ";
							}
							else
							{
								result.PlayOnSecondRunnerDesc = result.PlayOnSecondRunnerDesc + RetrieveReferenceDataDesc("field_position", play.ToString());
							}
						}
					}

					if (result.PlayOnThirdRunner != null)
					{
						x = 0;
						var playOn = result.PlayOnThirdRunner.ToCharArray();
						int playOnCount = playOn.Count();
						foreach (char play in playOn)
						{
							x++;
							if (x < playOnCount)
							{
								result.PlayOnThirdRunnerDesc = result.PlayOnThirdRunnerDesc + RetrieveReferenceDataDesc("field_position", play.ToString()) + ", ";
							}
							else
							{
								result.PlayOnThirdRunnerDesc = result.PlayOnThirdRunnerDesc + RetrieveReferenceDataDesc("field_position", play.ToString());
							}
						}
					}

					if (result.PlayOnBatterDesc != null)
					{
						result.PlayOnRunnerDetails = "Batter out - fielded by " + result.PlayOnBatterDesc + Environment.NewLine;
					}

					if (result.PlayOnFirstRunnerDesc != null)
					{
						result.PlayOnRunnerDetails = "First base runner out - fielded by " + result.PlayOnFirstRunnerDesc + Environment.NewLine;
					}

					if (result.PlayOnSecondRunnerDesc != null)
					{
						result.PlayOnRunnerDetails = "Second base runner out - fielded by " + result.PlayOnSecondRunnerDesc + Environment.NewLine;
					}

					if (result.PlayOnThirdRunnerDesc != null)
					{
						result.PlayOnRunnerDetails = "Third base runner out - fielded by " + result.PlayOnThirdRunnerDesc + Environment.NewLine;
					}

					result.FieldedByDesc = RetrieveReferenceDataDesc("field_position", result.FieldedBy.ToString());
					result.CountDesc = string.Concat(result.CountBalls, " / ", result.CountStrikes);

					if (result.EventTypeDesc != "")
					{
						result.PlayDetails = result.EventTypeDesc + Environment.NewLine;
					}

					if (result.BattedBallTypeDesc != "")
					{
						result.PlayDetails = result.PlayDetails + result.BattedBallTypeDesc;
						if (result.HitLocation != null)
						{
							result.PlayDetails = result.PlayDetails + " to " + RetrieveReferenceDataDesc("hit_location", result.HitLocation) + Environment.NewLine;
						}
						else
						{
							result.PlayDetails = result.PlayDetails + Environment.NewLine;
						}
					}


					if (result.FieldedByDesc != "")
					{
						result.PlayDetails = result.PlayDetails + "Fielded by " + result.FieldedByDesc;
					}

					string _runnerOnFirst = "0";
					string _runnerOnSecond = "0";
					string _runnerOnThird = "0";

					if (result.DestBatterDesc !="")
					{
						result.DestinationDetails = "Batter goes to " + result.DestBatterDesc + Environment.NewLine;
					}

					if (result.DestBatter == 1)
					{
						_runnerOnFirst = "1";
					}
					else if (result.DestBatter == 2)
					{
						_runnerOnSecond = "2";
					}
					else if (result.DestBatter == 3)
					{
						_runnerOnThird = "3";
					}

					int runCount = 0;
					if (result.DestBatter > 3) // runner has made it home
					{
						runCount++;
					}

					if ((result.DestFirstRunnerDesc != "") && (result.DestFirstRunnerDesc != "first base"))
					{
						result.DestinationDetails = result.DestinationDetails + "Runner on first goes to " + result.DestFirstRunnerDesc + Environment.NewLine;
					}

					if (result.DestFirstRunner == 1)
					{
						_runnerOnFirst = "1";
					}
					else if (result.DestFirstRunner == 2)
					{
						_runnerOnSecond = "2";
					}
					else if (result.DestFirstRunner == 3)
					{
						_runnerOnThird = "3";
					}

					if (result.DestFirstRunner > 3) // runner has made it home
					{
						runCount++;
					}

					if ((result.DestSecondRunnerDesc != "") && (result.DestSecondRunnerDesc != "second base"))
					{
						result.DestinationDetails = result.DestinationDetails + "Runner on second goes to " + result.DestSecondRunnerDesc + Environment.NewLine;
					}

					if (result.DestSecondRunner == 1)
					{
						_runnerOnFirst = "1";
					}
					else if (result.DestSecondRunner == 2)
					{
						_runnerOnSecond = "2";
					}
					else if (result.DestSecondRunner == 3)
					{
						_runnerOnThird = "3";
					}

					if (result.DestSecondRunner > 3) // runner has made it home
					{
						runCount++;
					}

					if ((result.DestThirdRunnerDesc != "") && (result.DestThirdRunnerDesc != "third base"))
					{
						result.DestinationDetails = result.DestinationDetails + "Runner on third goes to " + result.DestThirdRunnerDesc + Environment.NewLine;
					}

					if (result.DestThirdRunner == 1)
					{
						_runnerOnFirst = "1";
					}
					else if (result.DestThirdRunner == 2)
					{
						_runnerOnSecond = "2";
					}
					else if (result.DestThirdRunner == 3)
					{
						_runnerOnThird = "3";
					}

					if (result.DestThirdRunner > 3) // runner has made it home
					{
						runCount++;
					}

					result.DestinationRunnersOnBaseDiagram = RetrieveReferenceDataDesc("runners_diagram", "diagram_path") + _runnerOnFirst + _runnerOnSecond + _runnerOnThird + ".jpg";

					if (result.HitLocation != null)
					{
						result.HitLocationDiagram = RetrieveReferenceDataDesc("hit_diagram", "diagram_path") + RetrieveReferenceDataDesc("hit_diagram", result.HitLocation);
					}
					else
					{
						result.HitLocationDiagram = RetrieveReferenceDataDesc("hit_diagram", "diagram_path") + "blank.jpg";
					}

					result.FirstErrorPositionDesc = RetrieveReferenceDataDesc("field_position", result.FirstErrorPosition.ToString());
					result.FirstErrorTypeDesc = RetrieveReferenceDataDesc("error_type", result.FirstErrorType);

					result.SecondErrorPositionDesc = RetrieveReferenceDataDesc("field_position", result.SecondErrorPosition.ToString());
					result.SecondErrorTypeDesc = RetrieveReferenceDataDesc("error_type", result.SecondErrorType);

					result.ThirdErrorPositionDesc = RetrieveReferenceDataDesc("field_position", result.ThirdErrorPosition.ToString());
					result.ThirdErrorTypeDesc = RetrieveReferenceDataDesc("error_type", result.ThirdErrorType);

					if ((result.FirstErrorTypeDesc != null) && (result.FirstErrorTypeDesc !="no error"))
					{
						result.ErrorDetails = "Error on - " + result.FirstErrorPositionDesc + " - " + result.FirstErrorTypeDesc + Environment.NewLine;
					}

					if ((result.SecondErrorTypeDesc != null) && (result.SecondErrorTypeDesc != "no error"))
					{
						result.ErrorDetails = result.ErrorDetails + "Error on - " + result.SecondErrorPositionDesc + " - " + result.SecondErrorTypeDesc + Environment.NewLine;
					}

					if ((result.ThirdErrorTypeDesc != null) && (result.ThirdErrorTypeDesc != "no error"))
					{
						result.ErrorDetails = result.ErrorDetails + "Error on - " + result.ThirdErrorPositionDesc + " - " + result.ThirdErrorTypeDesc + Environment.NewLine;
					}

					result.BatterRemovedForPinchFieldPositionDesc = RetrieveReferenceDataDesc("field_position", result.BatterRemovedForPinchFieldPosition.ToString());
					PlayBeventInformation.Add(result);

					if (result.GameTeamCode == 0)  //visiting team
					{
						if (result.HitValue > 0)
						{
							visitingTeamHitCount = visitingTeamHitCount + 1;
						}

						visitingTeamErrorCount = visitingTeamErrorCount + result.NumErrors;
						/// note - VisitingTeamScore is the score BEFORE this play - so add the number of runs resulting from this play
						result.VisitingTeamScore = result.VisitingTeamScore + runCount;
					}
					else                         // home team  
					{
						if (result.HitValue > 0)
						{
							homeTeamHitCount = homeTeamHitCount + 1;
						}

						homeTeamErrorCount = homeTeamErrorCount + result.NumErrors;
						/// note - HomeTeamScore is the score BEFORE this play - so add the number of runs resulting from this play
						result.HomeTeamScore = result.HomeTeamScore + runCount;
					}

					result.VistingTeamHitCount = visitingTeamHitCount;
					result.VisitingTeamErrorCount = visitingTeamErrorCount;

					result.HomeTeamHitCount = homeTeamHitCount;
					result.HomeTeamErrorCount = homeTeamErrorCount;
				}

				return PlayBeventInformation;
			}
		}

		/*
		public Collection<DataModels.PlayerInformation>RetrievePlayers(string seasonYear, string seasonGameType, string homeTeamID, string visitTeamID)
		{
			string sqlQuery = @"select
	  Player.record_id RecordID
	, Player.player_id PlayerID
	, Player.last_name PlayerLastName
	, Player.first_name PlayerFirstName
	, Player.throws PlayerThrows
	, Player.bats PlayerBats
	, Player.team_id PlayerTeamID
	, Team.name PlayerTeamName
	, Player.field_position PlayerPosition
	, Player.season_year SeasonYear
	, Player.season_game_type SeasonGameType
from player
	join team on player.team_id = team.team_id
			 and team.season_year = 'x_season_year'
			 and team.season_game_type = 'x_season_game_type'
where Player.team_id in ('x_home_team_id', 'x_visit_team_id')
  and Player.season_year = 'x_season_year'
  and Player.season_game_type = 'x_season_game_type'";

			sqlQuery = sqlQuery.Replace("x_season_year", seasonYear);
			sqlQuery = sqlQuery.Replace("x_season_game_type", seasonGameType);
			sqlQuery = sqlQuery.Replace("x_home_team_id", homeTeamID);
			sqlQuery = sqlQuery.Replace("x_visit_team_id", visitTeamID);

			Collection<DataModels.PlayerInformation> PlayerInformation = new Collection<DataModels.PlayerInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.PlayerInformation> results = dbCtx.ExecuteQuery<DataModels.PlayerInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.PlayerInformation record count " + results.Count());

				foreach (DataModels.PlayerInformation result in results)
				{
					result.PlayerThrowsDesc = RetrieveReferenceDataDesc("throws", result.PlayerThrows);
					result.PlayerBatsDesc = RetrieveReferenceDataDesc("bats", result.PlayerBats);
					result.PlayerFieldPositionDesc = RetrieveReferenceDataDesc("field_position_x", result.PlayerFieldPosition);
					PlayerInformation.Add(result);
				}
			}

			return PlayerInformation;
		}
		*/

		private string RetrieveReferenceDataDesc(string ref_data_type, string ref_data_code)
		{
			DataModels.ReferenceData referenceData = new DataModels.ReferenceData();

			try
			{
				referenceData = referenceDataDictionary[ref_data_type + ref_data_code];
				return referenceData.referenceDataDesc;

			}
			catch
			{
				return "";
			}
		}

		private DataModels.PlayerInformation RetrievePlayerInformation(string playerId)
		{
			DataModels.PlayerInformation playerInformation = new DataModels.PlayerInformation();

			try
			{
				playerInformation = playerDictionary[playerId];
				return playerInformation;
			}
			catch
			{
				return playerInformation;
			}
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
	'' _displayUnderTeamCity, 
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

---  where games.season_game_type in ('A', 'W')
	 where games.season_game_type in ('W')
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
	'' _displayUnderTeamCity, 
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
	homeTeam.city _displayUnderTeamCity, 
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
	visitTeam.city _displayUnderTeamCity,
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
				//dbCtx.CommandTimeout = 120;  //2 minutes
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

		public Collection<DataModels.BatterAdjustmentInformation> RetrieveBatterAdjustment(string seasonYear,
																						   string seasonGameType,
																						   string homeTeamID,
																						   string visitingTeamID,
																						   string gameID)
		{
			string sqlQuery = @"select 
  batter_adjustment.record_id RecordID
, batter_adjustment.game_id GameID
, batter_adjustment.inning  Inning
, batter_adjustment.game_team_code GameTeamCode
, batter_adjustment.sequence Sequence
, batter_adjustment.player_id PlayerID
, player.last_name PlayerLastName
, player.first_name PlayerFirstName
, batter_adjustment.bats PlayerBats
, team.team_id TeamID
, team.name TeamName
from batter_adjustment
left join starting_player on batter_adjustment.player_id = starting_player.player_id
						 and starting_player.game_id = 'x_game_id' 
left join substitute_player on batter_adjustment.player_id = substitute_player.player_id
						   and substitute_player.game_id = 'x_game_id'
left join team on team.team_id = (
						case 
							when starting_player.team_id != 'NULL' then starting_player.team_id
							else substitute_player.team_id
						end)
left join player on player.player_id = (
						case  
							when starting_player.player_id != 'NULL' then starting_player.player_id
							else substitute_player.player_id
						end)
				 and player.team_id = team.team_id
where batter_adjustment.game_id =  'x_game_id'";

			sqlQuery = sqlQuery.Replace("x_season_year", seasonYear);
			sqlQuery = sqlQuery.Replace("x_season_game_type", seasonGameType);
			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.BatterAdjustmentInformation> BatterAdjustmentInformation = new Collection<DataModels.BatterAdjustmentInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.BatterAdjustmentInformation> results = dbCtx.ExecuteQuery<DataModels.BatterAdjustmentInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.BatterAdjustmentInformation record count " + results.Count());

				foreach (DataModels.BatterAdjustmentInformation result in results)
				{
					result.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
					result.BatsDesc = RetrieveReferenceDataDesc("bats", result.PlayerBats);
					BatterAdjustmentInformation.Add(result);
				}
			}
			return BatterAdjustmentInformation;
		}

		public Collection<DataModels.PlayerEjectionInformation> RetrievePlayerEjection(string gameID)
		{
			string sqlQuery = @"SELECT 
		ejection.record_id RecordID
	  , ejection.game_id GameID
	  , ejection.inning Inning
	  , ejection.game_team_code GameTeamCode
	  , ejection.sequence Sequence
	  , ejection.comment_sequence CommentSequence
	  , ejection.player_id PlayerID
	  , case manager.person_id 
			when null 
				then null
			else
				manager.last_name
			end PlayerLastName
	  , case manager.person_id
			when null
				then  null
			else
				manager.first_name
			end PlayerFirstName
	  , ejection.job_code JobCode
	  , ejection.umpire_id UmpireCode
	  , umpire.last_name UmpireLastName
	  , umpire.first_name UmpireFirstName
	  , umpire.role UmpireRoll
	  , ejection.reason Reason
  FROM ejection
	  join personnel umpire on umpire.person_id = ejection.umpire_id
	  left join personnel manager on manager.person_id = ejection.player_id
						 and manager.role = ejection.job_code
  where ejection.game_id = 'x_game_id'
  order by   ejection.game_id
		   , ejection.inning
		   , ejection.game_team_code
		   , ejection.sequence
		   , ejection.comment_sequence";

			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.PlayerEjectionInformation> PlayerEjectionInformation = new Collection<DataModels.PlayerEjectionInformation>();
			DataModels.PlayerEjectionInformation x_playerEjectionInformation = new DataModels.PlayerEjectionInformation();

			string x_playerEjectionInformationReason = null;

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.PlayerEjectionInformation> results = dbCtx.ExecuteQuery<DataModels.PlayerEjectionInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.PlayerEjectionInformation record count " + results.Count());

				foreach (DataModels.PlayerEjectionInformation result in results)
				{
					if (x_playerEjectionInformation.GameID == null)
					{
						x_playerEjectionInformation = result;

						x_playerEjectionInformation.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
						x_playerEjectionInformation.JobCodeDesc = RetrieveReferenceDataDesc("personnel", result.JobCode);
						x_playerEjectionInformation.UmpireRole = RetrieveReferenceDataDesc("personnel", result.UmpireRole);

						if (x_playerEjectionInformation.PlayerLastName == null)
						{

							x_playerEjectionInformation.PlayerLastName = RetrievePlayerInformation(result.PlayerID).PlayerLastName;
							x_playerEjectionInformation.PlayerFirstName = RetrievePlayerInformation(result.PlayerID).PlayerFirstName;
						}

					}
					else if ((result.Inning != x_playerEjectionInformation.Inning)
						|| (result.GameTeamCode != x_playerEjectionInformation.GameTeamCode)
						|| (result.Sequence != x_playerEjectionInformation.Sequence)
						|| (result.PlayerID != x_playerEjectionInformation.PlayerID))
					{
						x_playerEjectionInformation.Reason = x_playerEjectionInformationReason;
						x_playerEjectionInformationReason = null;
						PlayerEjectionInformation.Add(x_playerEjectionInformation);

						x_playerEjectionInformation = result;

						x_playerEjectionInformation.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
						x_playerEjectionInformation.JobCodeDesc = RetrieveReferenceDataDesc("personnel", result.JobCode);
						x_playerEjectionInformation.UmpireRole = RetrieveReferenceDataDesc("personnel", result.UmpireRole);

						if (x_playerEjectionInformation.PlayerLastName == null)
						{
							x_playerEjectionInformation.PlayerLastName = RetrievePlayerInformation(result.PlayerID).PlayerLastName;
							x_playerEjectionInformation.PlayerFirstName = RetrievePlayerInformation(result.PlayerID).PlayerFirstName;
						}
					}

					x_playerEjectionInformationReason = x_playerEjectionInformationReason + " " + result.Reason;
				}

				if (x_playerEjectionInformation.GameID != null)
				{
					x_playerEjectionInformation.Reason = x_playerEjectionInformationReason;
					PlayerEjectionInformation.Add(x_playerEjectionInformation);
				}
			}

			return PlayerEjectionInformation;
		}

		public Collection<DataModels.SubstitutePlayerInformation> RetrieveSubstitutePlayer(string seasonYear,
																						   string seasonGameType,
																						   string gameID)
		{
			string sqlQuery = @"SELECT 
		substitute_player.record_id RecordID
	  , substitute_player.game_id GameID
	  , substitute_player.inning Inning
	  , substitute_player.game_team_code GameTeamCode
	  , substitute_player.sequence Sequence
	  , substitute_player.player_id PlayerID
	  , player.last_name PlayerLastName
	  , player.first_name PlayerFirstName
	  , player.throws PlayerThrows
	  , player.bats PlayerBats
	  , substitute_player.batting_order BattingOrder
	  , substitute_player.field_position FieldPosition
	  , substitute_player.team_id TeamID
	  , team.name TeamName
  FROM substitute_player 
  join team on team.season_year = 'x_season_year'
		   and team.season_game_type = 'x_season_game_type'
		   and substitute_player.team_id = team.team_id
  join player on substitute_player.player_id = player.player_id
			 and player.season_year = 'x_season_year'
			 and player.season_game_type = 'x_season_game_type'
			 and player.team_id = substitute_player.team_id
  where substitute_player.game_id = 'x_game_id'";

			sqlQuery = sqlQuery.Replace("x_season_year", seasonYear);
			sqlQuery = sqlQuery.Replace("x_season_game_type", seasonGameType);
			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.SubstitutePlayerInformation> SubstitutePlayerInformation = new Collection<DataModels.SubstitutePlayerInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.SubstitutePlayerInformation> results = dbCtx.ExecuteQuery<DataModels.SubstitutePlayerInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.SubstitutePlayerInformation record count " + results.Count());

				foreach (DataModels.SubstitutePlayerInformation result in results)
				{
					result.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
					result.FieldPositionDesc = RetrieveReferenceDataDesc("field_position", result.FieldPosition.ToString());
					result.PlayerBatsDesc = RetrieveReferenceDataDesc( "bats", result.PlayerBats);
					result.PlayerThrowsDesc = RetrieveReferenceDataDesc("throws", result.PlayerThrows);
						
					SubstitutePlayerInformation.Add(result);

					if (!playerDictionary.ContainsKey(result.PlayerID))
					{
						DataModels.PlayerInformation playerInformation = new DataModels.PlayerInformation();
						playerInformation.RecordID = result.RecordID;
						playerInformation.PlayerID = result.PlayerID;
						playerInformation.PlayerLastName = result.PlayerLastName;
						playerInformation.PlayerFirstName = result.PlayerFirstName;
						playerInformation.PlayerThrows = result.PlayerThrows;
						playerInformation.PlayerThrowsDesc = result.PlayerThrowsDesc;
						playerInformation.PlayerBats = result.PlayerBats;
						playerInformation.PlayerBatsDesc = result.PlayerBatsDesc;
						playerInformation.PlayerTeamID = result.TeamID;
						playerInformation.PlayerTeamName = result.TeamName;
						playerInformation.PlayerFieldPosition = result.FieldPosition.ToString();
						playerInformation.PlayerFieldPositionDesc = result.FieldPositionDesc;
						playerInformation.SeasonYear = seasonYear;
						playerInformation.SeasonGameType = seasonGameType;
						playerDictionary.Add(playerInformation.PlayerID, playerInformation);
					}
				}
			}
			return SubstitutePlayerInformation;
		}


		public Collection<DataModels.GameCommentInformation> RetrieveGameComment(string gameID)
		{
			string sqlQuery = @"SELECT 
		game_comment.record_id RecordID
	  , game_comment.game_id GameID
	  , game_comment.inning Inning
	  , game_comment.game_team_code GameTeamCode
	  , game_comment.sequence Sequence
	  , game_comment.comment_sequence CommentSequence
	  , game_comment.comment Comment
  FROM game_comment
  where game_comment.game_id = 'x_game_id'
  order by 	  
		game_comment.game_id
	  , game_comment.inning
	  , game_comment.game_team_code
	  , game_comment.sequence
	  , game_comment.comment_sequence";

			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.GameCommentInformation> GameCommentInformation = new Collection<DataModels.GameCommentInformation>();
			DataModels.GameCommentInformation x_gameCommentInformation = new DataModels.GameCommentInformation();

			string x_gameCommentInformationComment = null;

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.GameCommentInformation> results = dbCtx.ExecuteQuery<DataModels.GameCommentInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.GameCommentInformation record count " + results.Count());

				foreach (DataModels.GameCommentInformation result in results)
				{
					if (x_gameCommentInformation.GameID == null)
					{
						x_gameCommentInformation = result;
						x_gameCommentInformation.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
					}
					else if ((result.Inning != x_gameCommentInformation.Inning)
						|| (result.GameTeamCode != x_gameCommentInformation.GameTeamCode)
						|| (result.Sequence != x_gameCommentInformation.Sequence))
					{
						x_gameCommentInformation.Comment = x_gameCommentInformationComment.Replace(" $","");
						x_gameCommentInformationComment = null;
						GameCommentInformation.Add(x_gameCommentInformation);

						x_gameCommentInformation = result;

						x_gameCommentInformation.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
					}

					x_gameCommentInformationComment = x_gameCommentInformationComment + " " + result.Comment;
				}

				if (x_gameCommentInformation.GameID != null)
				{
					x_gameCommentInformation.Comment = x_gameCommentInformationComment.Replace(" $", "");
					GameCommentInformation.Add(x_gameCommentInformation);
				}
			}

			return GameCommentInformation;
		}


		public Collection<DataModels.ReplayInformation> RetrieveReplay(string seasonYear,
																	   string seasonGameType,
																	   string gameID)
		{
			string sqlQuery = @"SELECT  
		replay.record_id RecordID
	  , replay.game_id GameID
	  , replay.inning Inning
	  , replay.game_team_code GameTeamCode
	  , replay.sequence Sequence
	  , replay.comment_sequence CommentSequence
	  , replay.player_id PlayerID
	  , replay.team_id PlayerTeamID
	  , team.name PlayerTeamName
	  , replay.umpire_id UmpireID
	  , personnel.last_name UmpireLastName
	  , personnel.first_name UmpireFirstName
	  , replay.ballpark_id BallparkID
	  , ballpark.name BallparkName
	  , ballpark.aka BallparkAKA
	  , replay.reason Reason
	  , replay.reversed Reversed
	  , replay.initiated Initiator
	  , replay.replay_challenge_team_id InitiatorTeamID
	  , initiateTeam.name InitiatorTeamName
	  , replay.type Type
  FROM  replay 
  join team on replay.team_id = team.team_id
				and team.season_year = 'x_season_year'
				and team.season_game_type = 'x_season_game_type'
  left join personnel on replay.umpire_id = personnel.person_id
				and personnel.role = 'U'
  join ballpark on replay.ballpark_id = ballpark.ballpark_id
  join team initiateTeam on replay.replay_challenge_team_id = initiateTeam.team_id
				and initiateTeam.season_year = 'x_season_year'
				and initiateTeam.season_game_type = 'x_season_game_type'
  where replay.game_id = 'x_game_id'
  order by
		replay.game_id 
	  , replay.inning 
	  , replay.game_team_code 
	  , replay.sequence 
	  , replay.comment_sequence 
	  , replay.player_id 
	  , replay.team_id 
	  , replay.umpire_id ";

			sqlQuery = sqlQuery.Replace("x_season_year", seasonYear);
			sqlQuery = sqlQuery.Replace("x_season_game_type", seasonGameType);
			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.ReplayInformation> ReplayInformation = new Collection<DataModels.ReplayInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.ReplayInformation> results = dbCtx.ExecuteQuery<DataModels.ReplayInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.ReplayInformation record count " + results.Count());

				foreach (DataModels.ReplayInformation result in results)
				{
					result.GameTeamCodeDesc = RetrieveReferenceDataDesc("game_team", result.GameTeamCode.ToString());
					result.PlayerLastName = RetrievePlayerInformation(result.PlayerID).PlayerLastName;
					result.PlayerFirstName = RetrievePlayerInformation(result.PlayerID).PlayerFirstName;
					result.ReasonDesc = RetrieveReferenceDataDesc("instant_replay_reason", result.Reason);
					result.InitiatorDesc = RetrieveReferenceDataDesc("instant_replay_initiator", result.Initiator);
					result.TypeDesc = RetrieveReferenceDataDesc("instant_replay_type", result.Type);
					result.UmpireName = string.Concat(result.UmpireLastName, ", ", result.UmpireFirstName);

					if(result.Reversed == "Y")
					{
						result.ReversedDesc = "ruling on play is overturned";
					}
					else
					{
						result.ReversedDesc = "ruling on play upheld";
					}

					ReplayInformation.Add(result);
				}
				return ReplayInformation;
			}
		}

		public Collection<DataModels.GameDataInformation> RetrieveGameData(string gameID)
		{
			string sqlQuery = @"SELECT game_data.record_id RecordID
	  ,game_data.game_id GameID
	  ,game_data.data_type DataType
	  ,game_data.player_id PlayerID
	  ,game_data.data_value DataValue
  FROM game_data
  where game_data.game_id = 'x_game_id' ";

			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.GameDataInformation> GameDataInformation = new Collection<DataModels.GameDataInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.GameDataInformation> results = dbCtx.ExecuteQuery<DataModels.GameDataInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.GameDataInformation record count " + results.Count());

				foreach (DataModels.GameDataInformation result in results)
				{
					result.PlayerLastName = RetrievePlayerInformation(result.PlayerID).PlayerLastName;
					result.PlayerFirstName = RetrievePlayerInformation(result.PlayerID).PlayerFirstName;
					result.DataTypeDesc = RetrieveReferenceDataDesc("game_data_type", result.DataType);
					GameDataInformation.Add(result);
				}

				return GameDataInformation;
			}
		}


		public Collection<DataModels.PitcherAdjustmentInformation> RetrievePitcherAdjustmentInformation(string seasonYear,
																								string seasonGameType,
																								string gameID)
		{
			string sqlQuery = @"select 
	   pitcher_adjustment.record_id RecordID
	  ,pitcher_adjustment.game_id GameID
	  ,pitcher_adjustment.inning Inning
	  ,pitcher_adjustment.game_team_code GameTeamCode
	  ,pitcher_adjustment.sequence Sequence
	  ,pitcher_adjustment.player_id PlayerID
	  ,player.last_name PlayerLastName
	  ,player.first_name PlayerFirstName
	  ,pitcher_adjustment.pitcher_hand PlayerHand
	  ,team.team_id TeamID
	  ,team.name TeamName
  FROM pitcher_adjustment
  left join starting_player on pitcher_adjustment.player_id = starting_player.player_id
						 and starting_player.game_id = 'x_game_id' 
  left join substitute_player on pitcher_adjustment.player_id = substitute_player.player_id
						   and substitute_player.game_id = 'x_game_id'
  left join team on team.team_id = (
						case 
							when starting_player.team_id != 'NULL' then starting_player.team_id
							else substitute_player.team_id
						end)
  left join player on player.player_id = (
						case  
							when starting_player.player_id != 'NULL' then starting_player.player_id
							else substitute_player.player_id
						end)
				 and player.team_id = team.team_id  WHERE pitcher_adjustment.game_id = 'x_game_id'";

			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.PitcherAdjustmentInformation> PitcherAdjustmentInformation = new Collection<DataModels.PitcherAdjustmentInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.PitcherAdjustmentInformation> results = dbCtx.ExecuteQuery<DataModels.PitcherAdjustmentInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.PitcherAdjustmentInformation record count " + results.Count());

				foreach (DataModels.PitcherAdjustmentInformation result in results)
				{
					result.PlayerLastName = RetrievePlayerInformation(result.PlayerID).PlayerLastName;
					result.PlayerFirstName = RetrievePlayerInformation(result.PlayerID).PlayerFirstName;
					result.PlayerHandDesc = RetrieveReferenceDataDesc("throws", result.PlayerHand);
					PitcherAdjustmentInformation.Add(result);
				}
				return PitcherAdjustmentInformation;
			}
		}

		public Collection<DataModels.SubstituteUmpireInformation> RetrieveSubstituteUmpireInformation(string gameID)
		{
			string sqlQuery = @"SELECT 
	   substitute_umpire.record_id RecordID
	  ,substitute_umpire.game_id GameID
	  ,substitute_umpire.inning Inning
	  ,substitute_umpire.sequence Sequence
	  ,substitute_umpire.comment_sequence CommentSequence
	  ,substitute_umpire.field_position FieldPosition
	  ,substitute_umpire.umpire_id UmpireID
	  ,personnel.last_name UmpireLastName
	  ,personnel.first_name UmpireFirstName
  FROM substitute_umpire
  left join personnel on substitute_umpire.umpire_id = personnel.person_id
					 and personnel.role = 'U'
  where substitute_umpire.game_id = 'x_game_id'";
			sqlQuery = sqlQuery.Replace("x_game_id", gameID);

			Collection<DataModels.SubstituteUmpireInformation> SubstituteUmpireInformation = new Collection<DataModels.SubstituteUmpireInformation>();

			// add System.Data.Linq assembly to the References
			using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
			{
				//dbCtx.CommandTimeout = 120;  //2 minutes
				IEnumerable<DataModels.SubstituteUmpireInformation> results = dbCtx.ExecuteQuery<DataModels.SubstituteUmpireInformation>(sqlQuery).ToList();
				Console.WriteLine("DataModels.SubstituteUmpireInformation record count " + results.Count());

				foreach (DataModels.SubstituteUmpireInformation result in results)
				{
					result.FieldPositionDesc = RetrieveReferenceDataDesc("ump_position", result.FieldPosition);
					SubstituteUmpireInformation.Add(result);
				}

				return SubstituteUmpireInformation;
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
			// non Regular Season games.
			if (result._seasonGameType != "R")
			{
				team.TeamIcon = "";
				team.TeamName = result._gameHomeTeamName + " vs " + result._gameVisitTeamName;
			}
			else
			{
				team.TeamIcon = result._iconPath + result._teamIcon;
				team.TeamName = result._displayUnderTeamCity + " " + result._displayUnderTeamName;
			}
			return team;
		}

		private TreeViewModels.League LoadTreeViewData_League(TreeViewModels.League league, _GameSelectionListItem result)
		{
			league.LeagueID = result._displayUnderLeagueID;
			// All-Star or World Series
			if ((result._seasonGameType == "A") || (result._seasonGameType == "W"))
			{
				league.LeagueIcon = "";
				league.LeagueName = "American League vs National League";
			}
			else
			{
				league.LeagueIcon = result._iconPath + result._leagueIcon;
				league.LeagueName = result._displayUnderLeagueName + " League";
			}
			return league;
		}

		private TreeViewModels.SeasonGameType LoadTreeViewData_SeasonGameType(TreeViewModels.SeasonGameType seasonGameType, _GameSelectionListItem result)
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
			public string _displayUnderTeamCity { get; set; }
			public string _displayUnderTeamName { get; set; }

			public string _iconPath { get; set; }
			public string _MLBIcon { get; set; }
			public string _leagueIcon { get; set; }
			public string _teamIcon { get; set; }

			public int _sortKey { get; set; }
		}

		private class _GameInformation
		{
			public Guid _recordID { get; set; }
			public string _gameID { get; set; }
			public string _visitingTeamID { get; set; }
			public string _homeTeamID { get; set; }
			public string _gameDate { get; set; }
			public string _gameNumber { get; set; }
			public string _startTime { get; set; }
			public string _dayNight { get; set; }
			public string _usedDH { get; set; }
			public string _pitches { get; set; }
			public string _umpireHomeID { get; set; }
			public string _umpireFirstBaseID { get; set; }
			public string _umpireSecondBaseID { get; set; }
			public string _umpireThirdBaseID { get; set; }
			public string _fieldCondition { get; set; }
			public string _precipitation { get; set; }
			public string _sky { get; set; }
			public string _temperature { get; set; }
			public string _windDirection { get; set; }
			public string _windSpeed { get; set; }
			public string _gameTimeLengthMinutes { get; set; }
			public string _attendance { get; set; }
			public string _ballparkID { get; set; }
			public string _winningPitcherID { get; set; }
			public string _losingPitcherID { get; set; }
			public string _savePitcherID { get; set; }
			public string _winningRBIPlayerID { get; set; }
			public string _oscorer { get; set; }
			public string _seasonYear { get; set; }
			public string _seasonGameType { get; set; }
			public string _editTime { get; set; }
			public string _howScored { get; set; }
			public string _inputProgVers { get; set; }
			public string _inputter { get; set; }
			public string _inputTime { get; set; }
			public string _scorer { get; set; }
			public string _translator { get; set; }
			public string _homeTeamLeague { get; set; }
			public string _homeTeamName { get; set; }
			public string _homeTeamCity { get; set; }
			public string _visitTeamLeague { get; set; }
			public string _visitTeamName { get; set; }
			public string _visitTeamCity { get; set; }
			public string _ballparkAKA { get; set; }
			public string _ballparkCity { get; set; }
			public string _ballparkName { get; set; }
			public string _ballparkNotes { get; set; }
			public string _umpireFirstBaseLastName { get; set; }
			public string _umpireFirstBaseFirstName { get; set; }
			public string _umpireSecondBaseLastName { get; set; }
			public string _umpireSecondBaseFirstName { get; set; }
			public string _umpireThirdBaseLastName { get; set; }
			public string _umpireThirdBaseFirstName { get; set; }
			public string _umpireHomeLastName { get; set; }
			public string _umpireHomeFirstName { get; set; }
		}
	}
}



