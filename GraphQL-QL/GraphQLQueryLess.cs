// GraphQLQueryLess.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/26/2019
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQLQL.Abstractions;

namespace GraphQLQL
{
    public static class GraphQLQueryLess
    {
        /// <summary>
        /// Maps TInput schema into a GraphQL query
        /// </summary>
        /// <typeparam name="TInput">Represents the class mapping the result from the server</typeparam>
        /// <param name="queryName">The name of the query on the server</param>
        /// <param name="filters">Optional filters for the query</param>
        /// <returns>A string representing the query to send to the server</returns>
        public static string BuildQuery<TInput>(string queryName, Dictionary<string, object> filters = null)
        {
            string strParameters = "";
            if (filters?.Count > 0)
            {
                var parameters = new List<string>(filters.Count);

                foreach (var kvp in filters)
                {
                    parameters.Add(string.Format("{0}: {1}", kvp.Key, kvp.Value));
                }

                strParameters = string.Format(" ({0}) ", string.Join(", ", parameters));
            }

            string query = BuildQuerySchema<TInput>();

            return $"{{ {queryName} {strParameters} { query } }}";
        }

        /// <summary>
        /// Maps TResult schema into a GraphQL mutation result
        /// </summary>
        /// <typeparam name="TResult">Represents the class mapping the result from the server</typeparam>
        /// <param name="mutationName">The name of the mutation on the server</param>
        /// <param name="variables">The variables to be passed to the mutation</param>
        /// <returns>A mutation request instance</returns>
        public static MutationRequest BuildMutation<TResult>(string mutationName, IList<MutationParameters> variables)
        {
            var result = new MutationRequest();

            var paramsTypes = new List<string>();
            var paramsValues = new List<string>();

            foreach (var parameter in variables)
            {
                paramsTypes.Add($@" ${parameter.Name}: {parameter.Type} ");
                paramsValues.Add($@" {parameter.Name}: ${parameter.Name} ");

                var expandoDictionary = result.Variables as IDictionary<string, object>;
                expandoDictionary.Add(parameter.Name, parameter.Content);
            }

            string strParamsTypes = string.Join(", ", paramsTypes);
            string strParamsValues = string.Join(", ", paramsValues);
            string resultSchema = BuildQuerySchema<TResult>();

            result.Query = $@"mutation ( { strParamsTypes } ) {{ { mutationName } ( { strParamsValues } ) { resultSchema } }}";

            return result;
        }

        #region Utils

        private static string BuildQuerySchema<TInput>()
        {
            TInput fieldWillBeReplacedSoDoNotUseIt_0A0A310123624BADA4F5CE81D5CAD0A446305CCCE73C2DD11E8CE1C45CB4DC26 = default(TInput);

            var gqlQuery = new { fieldWillBeReplacedSoDoNotUseIt_0A0A310123624BADA4F5CE81D5CAD0A446305CCCE73C2DD11E8CE1C45CB4DC26 };
            var propertyInfo = gqlQuery.GetType().GetProperty("fieldWillBeReplacedSoDoNotUseIt_0A0A310123624BADA4F5CE81D5CAD0A446305CCCE73C2DD11E8CE1C45CB4DC26");

            var query = QueryBuilder(propertyInfo);
            query = query.Replace("fieldWillBeReplacedSoDoNotUseIt_0A0A310123624BADA4F5CE81D5CAD0A446305CCCE73C2DD11E8CE1C45CB4DC26", "");

            return query;
        }

        private static string QueryBuilder(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(float) ||
                type == typeof(Enum) || type == typeof(DateTime) || type == typeof(TimeSpan) || type == typeof(DateTimeOffset))
            {
                var mappingAttr = propertyInfo.CustomAttributes?.FirstOrDefault(a => a.AttributeType == typeof(GraphQLMappingAttribute));
                if (mappingAttr != null)
                {
                    return mappingAttr.ConstructorArguments[0].Value.ToString();
                }

                return $" {propertyInfo.Name.ToLowerCamelCase()} ";
            }

            if (propertyInfo.PropertyType.IsGenericType == true)
            {
                type = propertyInfo.PropertyType.GenericTypeArguments[0];
            }
            else
            {
                type = propertyInfo.PropertyType;
            }

            string query = $" { propertyInfo.Name.ToLowerCamelCase() } {{ ";
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                query = $" { query } { QueryBuilder(property) } ";
            }

            query = $" {query} }}";

            return query;
        }

        private static string ToLowerCamelCase(this string input)
        {
            if (input[0] >= 'A' && input[0] <= 'Z')
            {
                input = string.Format("{0}{1}", input[0].ToString().ToLowerInvariant(), input.Substring(1));
            }

            return input;
        }

        #endregion
    }
}
