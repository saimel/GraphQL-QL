// GraphQLMappingAttribute.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/26/2019
//
//

using System;

namespace GraphQLQL.Abstractions
{
    public class GraphQLMappingAttribute : Attribute
    {
        public string MappingField { get; set; }

        public GraphQLMappingAttribute(string mappingField)
        {
            MappingField = mappingField;
        }
    }
}
