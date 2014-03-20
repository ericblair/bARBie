//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class OddsCheckerFootballFixtures
    {
        public OddsCheckerFootballFixtures()
        {
            this.FootballFixturesMap = new HashSet<FootballFixturesMap>();
            this.OddsCheckerFootballOdds = new HashSet<OddsCheckerFootballOdds>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public int CompetitionID { get; set; }
        public System.DateTime MatchDateTime { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string MatchWinnerOddsUrl { get; set; }
    
        public virtual Countries Countries { get; set; }
        public virtual FootballCompetitions FootballCompetitions { get; set; }
        public virtual ICollection<FootballFixturesMap> FootballFixturesMap { get; set; }
        public virtual ICollection<OddsCheckerFootballOdds> OddsCheckerFootballOdds { get; set; }
    }
}
