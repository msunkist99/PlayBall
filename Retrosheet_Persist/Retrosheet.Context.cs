﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class retrosheetDB : DbContext
    {
        public retrosheetDB()
            : base("name=retrosheetDB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Ballpark> Ballparks { get; set; }
        public virtual DbSet<Batter_Adjustment> Batter_Adjustment { get; set; }
        public virtual DbSet<Game_Comment> Game_Comment { get; set; }
        public virtual DbSet<Game_Data> Game_Data { get; set; }
        public virtual DbSet<Game_Info> Game_Info { get; set; }
        public virtual DbSet<Pitcher_Adjustment> Pitcher_Adjustment { get; set; }
        public virtual DbSet<Starting_Player> Starting_Player { get; set; }
        public virtual DbSet<Substitute_Player> Substitute_Player { get; set; }
        public virtual DbSet<Replay> Replays { get; set; }
        public virtual DbSet<Ejection> Ejections { get; set; }
        public virtual DbSet<Admin_Info> Admin_Info { get; set; }
        public virtual DbSet<Substitute_Umpire> Substitute_Umpire { get; set; }
        public virtual DbSet<Personnel> Personnels { get; set; }
        public virtual DbSet<Game_Information> Game_Information { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Reference_Data> Reference_Data { get; set; }
        public virtual DbSet<Play> Plays { get; set; }
    }
}
