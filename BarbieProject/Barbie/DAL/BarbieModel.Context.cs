﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bARBieEntities : DbContext
    {
        public bARBieEntities()
            : base("name=bARBieEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public IDbSet<Arbs_Football_MatchWinner> Arbs_Football_MatchWinner { get; set; }
        public IDbSet<Arbs_Football_MatchWinner_Expired> Arbs_Football_MatchWinner_Expired { get; set; }
        public IDbSet<BetFairCompetitionUrls> BetFairCompetitionUrls { get; set; }
        public IDbSet<BetFairFootballFixtures> BetFairFootballFixtures { get; set; }
        public IDbSet<BetFairFootballOdds> BetFairFootballOdds { get; set; }
        public IDbSet<Bookie> Bookie { get; set; }
        public IDbSet<Countries> Countries { get; set; }
        public IDbSet<FootballCompetitions> FootballCompetitions { get; set; }
        public IDbSet<FootballFixturesMap> FootballFixturesMap { get; set; }
        public IDbSet<OddsCheckerCompetitionUrls> OddsCheckerCompetitionUrls { get; set; }
        public IDbSet<OddsCheckerFootballFixtures> OddsCheckerFootballFixtures { get; set; }
        public IDbSet<OddsCheckerFootballOdds> OddsCheckerFootballOdds { get; set; }
    }
}
