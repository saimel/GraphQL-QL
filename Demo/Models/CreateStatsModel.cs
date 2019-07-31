// CreateStatsModel.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/25/2019
//
//

using Newtonsoft.Json;

namespace GraphQLQLDemo.Models
{
    public class CreateStatsModel
    {
        public int PlayerId { get; set; }

        public int TeamId { get; set; }

        public int SeasonId { get; set; }

        public int GamesPlayed { get; set; }

        public int AtBat { get; set; }

        public int Hits { get; set; }

        public int HomeRuns { get; set; }

        [JsonProperty("rbis")]
        public int RBIs { get; set; }

        public CreateStatsModel(int playerId)
        {
            PlayerId = playerId;
        }
    }
}
