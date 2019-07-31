// PlayerModel.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/25/2019
//
//

using System;
using System.Collections.Generic;

namespace GraphQLQLDemo.Models
{
    public class PlayerModel : PlayerListModel
    {
        public DateTime BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public string Height { get; set; }

        public int WeightLbs { get; set; }

        public IList<PlayerStatsModel> PlayerStats { get; set; }
    }
}
