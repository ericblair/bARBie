//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class FootballFixturesMap
    {
        public FootballFixturesMap()
        {
            this.Arbs_Football_MatchWinner = new HashSet<Arbs_Football_MatchWinner>();
        }
    
        public int ID { get; set; }
        public int BetFairFixtureID { get; set; }
        public int OddsCheckerFixtureID { get; set; }
    
        public virtual BetFairFootballFixtures BetFairFootballFixtures { get; set; }
        public virtual OddsCheckerFootballFixtures OddsCheckerFootballFixtures { get; set; }
        public virtual ICollection<Arbs_Football_MatchWinner> Arbs_Football_MatchWinner { get; set; }
    }
}
