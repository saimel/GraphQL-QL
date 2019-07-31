// MutationVariables.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/29/2019
//
//

namespace GraphQLQL.Abstractions
{
    public class MutationParameters
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public object Content { get; set; }
    }
}
