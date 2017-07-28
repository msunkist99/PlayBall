//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Retrosheet_Persist
{
    using System;
    using System.Collections.Generic;
    
    public partial class Game_Information
    {
        public System.Guid record_id { get; set; }
        public string game_id { get; set; }
        public string visiting_team_id { get; set; }
        public string home_team_id { get; set; }
        public System.DateTime game_date { get; set; }
        public int game_number { get; set; }
        public string start_time { get; set; }
        public string day_night { get; set; }
        public string used_dh { get; set; }
        public string pitches { get; set; }
        public string umpire_home_id { get; set; }
        public string umpire_first_base_id { get; set; }
        public string umpire_second_base_id { get; set; }
        public string umpire_third_base_id { get; set; }
        public string field_condition { get; set; }
        public string precipitation { get; set; }
        public string sky { get; set; }
        public Nullable<int> temperature { get; set; }
        public string wind_direction { get; set; }
        public Nullable<int> wind_speed { get; set; }
        public Nullable<int> game_time_length_minutes { get; set; }
        public Nullable<int> attendance { get; set; }
        public string ballpark_id { get; set; }
        public string winning_pitcher_id { get; set; }
        public string losing_pitcher_id { get; set; }
        public string save_pitcher_id { get; set; }
        public string winning_rbi_player_id { get; set; }
        public string oscorer { get; set; }
        public string season_year { get; set; }
        public string season_game_type { get; set; }
        public string edit_time { get; set; }
        public string how_scored { get; set; }
        public string input_prog_vers { get; set; }
        public string inputter { get; set; }
        public string input_time { get; set; }
        public string scorer { get; set; }
        public string translator { get; set; }
    }
}
