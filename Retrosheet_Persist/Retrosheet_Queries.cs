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
    public class Retrosheet_Queries
    {
        public static void BuildGameInformationData()
        {
            DateTime dateTime;
            int intValue;

            string sqlQuery = @"select 
  driver.game_id as _game_id
, a.game_info_value as _visiting_team_id
, b.game_info_value as _home_team_id
, c.game_info_value as _game_date
, d.game_info_value as _game_number
, e.game_info_value as _start_time
, f.game_info_value as _day_night
, g.game_info_value as _used_dh
, h.game_info_value as _pitches
, i.game_info_value as _umpire_home_id
, j.game_info_value as _umpire_first_base_id
, k.game_info_value as _umpire_second_base_id
, l.game_info_value as _umpire_third_base_id
, m.game_info_value as _field_condition
, n.game_info_value as _precipitation
, o.game_info_value as _sky
, p.game_info_value as _temperature
, q.game_info_value as _wind_direction
, r.game_info_value as _wind_speed
, s.game_info_value as _game_time_length_minutes
, t.game_info_value as _attendance
, u.game_info_value as _ballpark_id
, v.game_info_value as _winning_pitcher_id
, w.game_info_value as _losing_pitcher_id
, x.game_info_value as _save_pitcher_id
, y.game_info_value as _winning_rbi_player_id
, z.game_info_value as _oscorer
from game_info driver
left join game_info a on driver.game_id = a.game_id
                           and a.game_info_type = 'visteam'
left join game_info b on driver.game_id = b.game_id
                           and b.game_info_type = 'hometeam'
left join game_info c on driver.game_id = c.game_id
                           and c.game_info_type = 'date'
left join game_info d on driver.game_id = d.game_id
                           and d.game_info_type = 'number'
left join game_info e on driver.game_id = e.game_id
                           and e.game_info_type = 'starttime'
left join game_info f on driver.game_id = f.game_id
                           and f.game_info_type = 'daynight'
left join game_info g on driver.game_id = g.game_id
                           and g.game_info_type = 'usedh'
left join game_info h on driver.game_id = h.game_id
                           and h.game_info_type = 'pitches'
left join game_info i on driver.game_id = i.game_id
                           and i.game_info_type = 'umphome'
left join game_info j on driver.game_id = j.game_id
                           and j.game_info_type = 'ump1b'
left join game_info k on driver.game_id = k.game_id
                           and k.game_info_type = 'ump2b'
left join game_info l on driver.game_id = l.game_id
                           and l.game_info_type = 'ump3b'
left join game_info m on driver.game_id = m.game_id
                           and m.game_info_type = 'fieldcond'
left join game_info n on driver.game_id = n.game_id
                           and n.game_info_type = 'precip'
left join game_info o on driver.game_id = o.game_id
                           and o.game_info_type = 'sky'
left join game_info p on driver.game_id = p.game_id
                           and p.game_info_type = 'temp'
left join game_info q on driver.game_id = q.game_id
                           and q.game_info_type = 'winddir'
left join game_info r on driver.game_id = r.game_id
                           and r.game_info_type = 'windspeed'
left join game_info s on driver.game_id = s.game_id
                           and s.game_info_type = 'timeofgame'
left join game_info t on driver.game_id = t.game_id
                           and t.game_info_type = 'attendance'
left join game_info u on driver.game_id = u.game_id
                           and u.game_info_type = 'site'
left join game_info v on driver.game_id = v.game_id
                           and v.game_info_type = 'wp'
left join game_info w on driver.game_id = v.game_id
                           and v.game_info_type = 'lp'
left join game_info x on driver.game_id = x.game_id
                           and x.game_info_type = 'save'
left join game_info y on driver.game_id = y.game_id
                           and y.game_info_type = 'gwrbi'
left join game_info z on driver.game_id = z.game_id
                           and z.game_info_type = 'oscorer'
where driver.game_info_type = 'date'";

            

            using (RetrosheetDataContext dbCtx = new RetrosheetDataContext())
            {
                IEnumerable<_GameInformation> results = dbCtx.ExecuteQuery<_GameInformation>(sqlQuery).ToList();
                var db = new retrosheetDB();
                Console.WriteLine("Game_Information record count " + results.Count());

                foreach (_GameInformation result in results)
                {
                    Game_Information game_Information = new Game_Information();

                    game_Information.record_id = Guid.NewGuid();
                    game_Information.game_id = result._game_id;
                    game_Information.visiting_team_id = result._visiting_team_id;
                    game_Information.home_team_id = result._home_team_id;

                    if (DateTime.TryParse(result._game_date, out dateTime))
                    {
                        game_Information.game_date = dateTime;
                    }
                    else
                    {
                        game_Information.game_date = DateTime.MinValue;
                    }

                    if (int.TryParse(result._game_number, out intValue))
                    {
                        game_Information.game_number = intValue;
                    }
                    else
                    {
                        game_Information.game_number = -1;    
                    }

                    game_Information.start_time = result._start_time;
                    game_Information.day_night = result._day_night;

                    if (result._used_dh == "true")
                    {
                        game_Information.used_dh = "Y";
                    }
                    else
                    {
                        game_Information.used_dh = "N";
                    }

                    game_Information.pitches = result._pitches;
                    game_Information.umpire_home_id = result._umpire_home_id;
                    game_Information.umpire_first_base_id = result._umpire_first_base_id;
                    game_Information.umpire_second_base_id = result._umpire_second_base_id;
                    game_Information.umpire_third_base_id = result._umpire_third_base_id;
                    game_Information.field_condition = result._field_condition;
                    game_Information.precipitation = result._precipitation;
                    game_Information.sky = result._sky;

                    if (int.TryParse(result._temperature, out intValue))
                    {
                        game_Information.temperature = intValue;
                    }
                    else
                    {
                        game_Information.temperature = -1;
                    }

                    game_Information.wind_direction = result._wind_direction;

                    if (int.TryParse(result._wind_speed, out intValue))
                    {
                        game_Information.wind_speed = intValue;
                    }
                    else
                    {
                        game_Information.wind_speed = -1;
                    }

                    if (int.TryParse(result._game_time_length_minutes, out intValue))
                    {
                        game_Information.game_time_length_minutes = intValue;
                    }
                    else
                    {
                        game_Information.game_time_length_minutes = -1;
                    }


                    if (int.TryParse(result._attendance, out intValue))
                    {
                        game_Information.attendance = intValue;
                    }
                    else
                    {
                        game_Information.attendance = -1;
                    }

                    game_Information.ballpark_id = result._ballpark_id;
                    game_Information.winning_pitcher_id = result._winning_pitcher_id;
                    game_Information.losing_pitcher_id = result._losing_pitcher_id;
                    game_Information.save_pitcher_id = result._save_pitcher_id;
                    game_Information.winning_rbi_player_id = result._winning_rbi_player_id;
                    game_Information.oscorer = result._oscorer;

                    db.Game_Information.Add(game_Information);

                    try
                    {
                        db.SaveChanges();
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
            }
        }

        private class _GameInformation
        {
            public string _record_id { get; set; }
            public string _game_id { get; set; }
            public string _visiting_team_id { get; set; }
            public string _home_team_id { get; set; }
            public string _game_date { get; set; }
            public string _game_number { get; set; }
            public string _start_time { get; set; }
            public string _day_night { get; set; }
            public string _used_dh { get; set; }
            public string _pitches { get; set; }
            public string _umpire_home_id { get; set; }
            public string _umpire_first_base_id { get; set; }
            public string _umpire_second_base_id { get; set; }
            public string _umpire_third_base_id { get; set; }
            public string _field_condition { get; set; }
            public string _precipitation { get; set; }
            public string _sky { get; set; }
            public string _temperature { get; set; }
            public string _wind_direction { get; set; }
            public string _wind_speed { get; set; }
            public string _game_time_length_minutes { get; set; }
            public string _attendance { get; set; }
            public string _ballpark_id { get; set; }
            public string _winning_pitcher_id { get; set; }
            public string _losing_pitcher_id { get; set; }
            public string _save_pitcher_id { get; set; }
            public string _winning_rbi_player_id { get; set; }
            public string _oscorer { get; set; }
        }
    }
}

/*
var gameInfoQuery = from driver in dbCtx.Game_Infos
                    join a in dbCtx.Game_Infos on driver.game_id equals a.game_id 
                    join b in dbCtx.Game_Infos on driver.game_id equals b.game_id
                    join c in dbCtx.Game_Infos on driver.game_id equals c.game_id
                    join d in dbCtx.Game_Infos on driver.game_id equals d.game_id
                    join e in dbCtx.Game_Infos on driver.game_id equals e.game_id
                    join f in dbCtx.Game_Infos on driver.game_id equals f.game_id
                    join g in dbCtx.Game_Infos on driver.game_id equals g.game_id
                    join h in dbCtx.Game_Infos on driver.game_id equals h.game_id
                    join i in dbCtx.Game_Infos on driver.game_id equals i.game_id
                    join j in dbCtx.Game_Infos on driver.game_id equals j.game_id
                    join k in dbCtx.Game_Infos on driver.game_id equals k.game_id
                    join l in dbCtx.Game_Infos on driver.game_id equals l.game_id
                    join m in dbCtx.Game_Infos on driver.game_id equals m.game_id
                    join n in dbCtx.Game_Infos on driver.game_id equals n.game_id
                    join o in dbCtx.Game_Infos on driver.game_id equals o.game_id
                    join p in dbCtx.Game_Infos on driver.game_id equals p.game_id
                    join q in dbCtx.Game_Infos on driver.game_id equals q.game_id
                    join r in dbCtx.Game_Infos on driver.game_id equals r.game_id
                    join s in dbCtx.Game_Infos on driver.game_id equals s.game_id
                    join t in dbCtx.Game_Infos on driver.game_id equals t.game_id
                    join u in dbCtx.Game_Infos on driver.game_id equals u.game_id
                    join v in dbCtx.Game_Infos on driver.game_id equals v.game_id
                    join w in dbCtx.Game_Infos on driver.game_id equals w.game_id
                    join x in dbCtx.Game_Infos on driver.game_id equals x.game_id
                    join y in dbCtx.Game_Infos on driver.game_id equals y.game_id
                    join z in dbCtx.Game_Infos on driver.game_id equals z.game_id

                    where driver.game_info_type == "date"

                    && a.game_info_type == "visteam"
                    && b.game_info_type == "hometeam"
                    && c.game_info_type == "date"
                    && d.game_info_type == "number"
                    && e.game_info_type == "starttime"
                    && f.game_info_type == "daynight"
                    && g.game_info_type == "usedh"
                    && h.game_info_type == "pitches"
                    && i.game_info_type == "umphome"
                    && j.game_info_type == "ump1b"
                    && k.game_info_type == "ump2b"
                    && l.game_info_type == "ump3b"
                    && m.game_info_type == "fieldcond"
                    && n.game_info_type == "precip"
                    && o.game_info_type == "sky"
                    && p.game_info_type == "temp"
                    && q.game_info_type == "winddir"
                    && r.game_info_type == "windspeed"
                    && s.game_info_type == "timeofgame"
                    && t.game_info_type == "attendance"
                    && u.game_info_type == "site"
                    && v.game_info_type == "wp"
                    && w.game_info_type == "lp"
                    && x.game_info_type == "save"
                    && y.game_info_type == "gwrbi"
                    && z.game_info_type == "oscorer"

                    select new
                    {
                        driver.game_id,
                        visteam = a.game_info_value,
                        hometeam = b.game_info_value,
                        date = c.game_info_value,
                        number = d.game_info_value,
                        starttime = e.game_info_value,
                        daynight = f.game_info_value,
                        usedh = g.game_info_value,
                        pitches = h.game_info_value,
                        umphome = i.game_info_value,
                        ump1b = j.game_info_value,
                        ump2b = k.game_info_value,
                        ump3b = l.game_info_value,
                        fieldcond = m.game_info_value,
                        precip = n.game_info_value,
                        sky = o.game_info_value,
                        temp = p.game_info_value,
                        winddir = q.game_info_value,
                        windspeed = r.game_info_value,
                        timeofgame = s.game_info_value,
                        attendance = t.game_info_value,
                        site = u.game_info_value,
                        wp = v.game_info_value,
                        lp = w.game_info_value,
                        save = x.game_info_value,
                        gwrbi = y.game_info_value,
                        oscorer = z.game_info_value
                    };



foreach (var gameInformationRecord in gameInfoQuery)
{
    gameInformationDTO.RecordID = Guid.NewGuid();
    gameInformationDTO.GameID = gameInformationRecord.game_id;
    gameInformationDTO.VisitingTeam_ID = gameInformationRecord.visteam;
    gameInformationDTO.HomeTeam_ID = gameInformationRecord.hometeam;

    if (DateTime.TryParse(gameInformationRecord.date, out dateTime))
    {
        gameInformationDTO.GameDate = dateTime;
    }
    else
    {
        gameInformationDTO.GameDate = DateTime.MinValue;
    }

    gameInformationDTO.GameNumber = Convert.ToInt16(gameInformationRecord.number);

    if (DateTime.TryParse(gameInformationRecord.starttime, out dateTime))
    {
        gameInformationDTO.StartTime = dateTime;
    }
    else
    {
        gameInformationDTO.StartTime = DateTime.MinValue;
    }

    gameInformationDTO.DayNight = gameInformationRecord.daynight;

    if (gameInformationRecord.usedh == "true")
    {
        gameInformationDTO.UsedDH = true;
    }
    else
    {
        gameInformationDTO.UsedDH = false;
    }
    gameInformationDTO.Pitches = gameInformationRecord.pitches;
    gameInformationDTO.UmpireHome_ID = gameInformationRecord.umphome;
    gameInformationDTO.UmpireFirstBaseID = gameInformationRecord.ump1b;
    gameInformationDTO.UmpireSecondBaseID = gameInformationRecord.ump2b;
    gameInformationDTO.UmpireThirdBaseID = gameInformationRecord.ump3b;
    gameInformationDTO.FieldCondition = gameInformationRecord.fieldcond;
    gameInformationDTO.Precipitation = gameInformationRecord.precip;
    gameInformationDTO.Sky = gameInformationRecord.sky;
    gameInformationDTO.Temperature = Convert.ToInt16(gameInformationRecord.temp);
    gameInformationDTO.WindDirection = gameInformationRecord.winddir;
    gameInformationDTO.WindSpeed = Convert.ToInt16(gameInformationRecord.windspeed);
    gameInformationDTO.GameTimeLengthMinutes = Convert.ToInt16(gameInformationRecord.timeofgame);
    gameInformationDTO.Attendance = Convert.ToInt16(gameInformationRecord.attendance);
    gameInformationDTO.BallPark_ID = gameInformationRecord.site;
    gameInformationDTO.WinningPitcherID = gameInformationRecord.wp;
    gameInformationDTO.LosingPitcherID = gameInformationRecord.lp;
    gameInformationDTO.SavePitcherID = gameInformationRecord.save;
    gameInformationDTO.WinningRBIPlayerID = gameInformationRecord.gwrbi;

    gameInformationDTOs.Add(gameInformationDTO);
}
return gameInformationDTOs;

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
*/

