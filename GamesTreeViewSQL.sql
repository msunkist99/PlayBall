use retrosheet;

select distinct(games.season_year)
from retrosheet.dbo.Game_Information games
order by games.season_year;

select distinct(games.season_year), games.season_game_type, ref.ref_data_desc
from retrosheet.dbo.Game_Information games
join retrosheet.dbo.Reference_Data ref on games.season_game_type = ref.ref_data_code
                                      and ref.ref_data_type = 'season_game_type'
where games.season_year = '2015'
order by games.season_game_type;


---- Post Season and All Star games
select distinct(games.season_year), games.season_game_type, refGameType.ref_data_desc gameTypeDesc,
	games.game_date,
	homeTeam.league homeLeague, refHomeTeamLeague.ref_data_desc homeLeagueDesc, games.home_team_id, homeTeam.name homeTeamName, homeTeam.city homeTeamCity, 
	visitTeam.league, refVisitTeamLeague.ref_data_desc, games.visiting_team_id, visitTeam.name, visitTeam.city,
	games.game_id, 
	'' home_away_league,
	'' home_away_team
from retrosheet.dbo.Game_Information games
join retrosheet.dbo.Reference_Data refGameType on games.season_game_type = refGameType.ref_data_code
                                              and refGameType.ref_data_type = 'season_game_type'
join retrosheet.dbo.team homeTeam on games.home_team_id = homeTeam.team_id
join retrosheet.dbo.Reference_Data refHomeTeamLeague on homeTeam.league = refHomeTeamLeague.ref_data_code
                                              and refHomeTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.team visitTeam on games.visiting_team_id = visitTeam.team_id
join retrosheet.dbo.Reference_Data refVisitTeamLeague on visitTeam.league = refVisitTeamLeague.ref_data_code
                                              and refVisitTeamLeague.ref_data_type = 'team_league'
where games.season_year = '2015'
  and games.season_game_type != '4'
order by games.season_game_type, homeTeam.league, games.game_date, home_team_id, games.game_id

---- regular season games
select * from
---- regular season home games
(select distinct(games.season_year), games.season_game_type, refGameType.ref_data_desc gameTypeDesc,
	games.game_date,
	homeTeam.league homeLeague, refHomeTeamLeague.ref_data_desc homeLeagueDesc, games.home_team_id, homeTeam.name homeTeamName, homeTeam.city homeTeamCity, 
	visitTeam.league, refVisitTeamLeague.ref_data_desc, games.visiting_team_id, visitTeam.name, visitTeam.city,
	games.game_id, 
	homeTeam.league home_away_league,
	games.home_team_id home_away_team
from retrosheet.dbo.Game_Information games
join retrosheet.dbo.Reference_Data refGameType on games.season_game_type = refGameType.ref_data_code
                                              and refGameType.ref_data_type = 'season_game_type'
join retrosheet.dbo.team homeTeam on games.home_team_id = homeTeam.team_id
join retrosheet.dbo.Reference_Data refHomeTeamLeague on homeTeam.league = refHomeTeamLeague.ref_data_code
                                              and refHomeTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.team visitTeam on games.visiting_team_id = visitTeam.team_id
join retrosheet.dbo.Reference_Data refVisitTeamLeague on visitTeam.league = refVisitTeamLeague.ref_data_code
                                              and refVisitTeamLeague.ref_data_type = 'team_league'
where games.season_year = '2015'
  and games.season_game_type = 'R'
union all
---- regular season away games
select distinct(games.season_year), games.season_game_type, refGameType.ref_data_desc gameTypeDesc,
	games.game_date,
	homeTeam.league homeLeague, refHomeTeamLeague.ref_data_desc homeLeagueDesc, games.home_team_id, homeTeam.name homeTeamName, homeTeam.city homeTeamCity, 
	visitTeam.league, refVisitTeamLeague.ref_data_desc, games.visiting_team_id, visitTeam.name, visitTeam.city,
	games.game_id, 
	homeTeam.league home_away_league,
	games.visiting_team_id home_away_team
from retrosheet.dbo.Game_Information games
join retrosheet.dbo.Reference_Data refGameType on games.season_game_type = refGameType.ref_data_code
                                              and refGameType.ref_data_type = 'season_game_type'
join retrosheet.dbo.team homeTeam on games.home_team_id = homeTeam.team_id
join retrosheet.dbo.Reference_Data refHomeTeamLeague on homeTeam.league = refHomeTeamLeague.ref_data_code
                                              and refHomeTeamLeague.ref_data_type = 'team_league'
join retrosheet.dbo.team visitTeam on games.visiting_team_id = visitTeam.team_id
join retrosheet.dbo.Reference_Data refVisitTeamLeague on visitTeam.league = refVisitTeamLeague.ref_data_code
                                              and refVisitTeamLeague.ref_data_type = 'team_league'
where games.season_year = '2015'
  and games.season_game_type = 'R') unionTable
order by home_away_league, home_away_team, game_date, game_id


select distinct(game.season_year), game.season_game_type, ref.ref_data_desc,
case game.season_game_type
	when 'R' then 0
	when 'A' then 1
	when 'C' then 2
	when '1' then 3
	when '2' then 4
	when 'L' then 5
	when 'W' then 6
end as sort_key
from Game_Information game
join Reference_Data ref on game.season_game_type = ref.ref_data_code
                       and ref.ref_data_type = 'season_game_type'
order by game.season_year,
         sort_key
