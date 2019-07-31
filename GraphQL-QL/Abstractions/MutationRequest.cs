// MutationVariables.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/29/2019
//
//

using System.Dynamic;

namespace GraphQLQL.Abstractions
{
    public class MutationRequest
    {
        public string Query { get; set; }

        public dynamic Variables { get; set; }

        public MutationRequest()
        {
            Variables = new ExpandoObject();
        }
    }
}
