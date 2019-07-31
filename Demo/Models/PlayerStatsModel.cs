// PlayerStatsModel.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/25/2019
//
//

using GraphQLQL.Abstractions;

namespace GraphQLQLDemo.Models
{
    public class PlayerStatsModel : BaseModel
    {
        public int Average { get; set; }

        public string Season { get; set; }

        public string TeamAbbreviation { get; set; }

        public string LeagueAbbreviation { get; set; }

        public int GamesPlayed { get; set; }

        public int AtBat { get; set; }

        public int Hits { get; set; }

        [GraphQLMapping("rbis")]
        public int RBIs { get; set; }

        public int HomeRuns { get; set; }
    }
}
