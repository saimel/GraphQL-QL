// GraphQLService.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/25/2019
//
//

using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Exceptions;
using GraphQL.Common.Request;
using GraphQLQL.Abstractions;

namespace GraphQLQL
{
    public static class GraphQLService
    {
        public static async Task<T> GetAsync<T>(string endpoint, string query, string section, CancellationToken? token = null)
        {
            var gqlClient = new GraphQLClient(endpoint, new GraphQLClientOptions { MediaType = new MediaTypeHeaderValue("application/json") });

            try
            {
                var gqlResponse = await gqlClient.GetQueryAsync(query, token ?? CancellationToken.None);

                if (gqlResponse.Errors?.Length > 0)
                {
                    throw new GraphQLException(gqlResponse.Errors[0]);
                }

                var result = gqlResponse.GetDataFieldAs<T>(section);
                return result;
            }
            finally
            {
                gqlClient.Dispose();
            }
        }

        public static async Task<T> PostQueryAsync<T>(string endpoint, string query, string section, CancellationToken? token = null)
        {
            var gqlClient = new GraphQLClient(endpoint, new GraphQLClientOptions { MediaType = new MediaTypeHeaderValue("application/json") });

            try
            {
                var gqlResponse = await gqlClient.PostQueryAsync(query, token ?? CancellationToken.None);

                if (gqlResponse.Errors?.Length > 0)
                {
                    throw new GraphQLException(gqlResponse.Errors[0]);
                }

                var result = gqlResponse.GetDataFieldAs<T>(section);
                return result;
            }
            finally
            {
                gqlClient.Dispose();
            }
        }

        public static async Task<T> PostAsync<T>(string endpoint, MutationRequest request, string section, CancellationToken? token = null)
        {
            var gqlClient = new GraphQLClient(endpoint, new GraphQLClientOptions { MediaType = new MediaTypeHeaderValue("application/json") });

            try
            {
                var gqlRequest = new GraphQLRequest
                {
                    Query = request.Query,
                    Variables = request.Variables
                };

                var gqlResponse = await gqlClient.PostAsync(gqlRequest, token ?? CancellationToken.None);

                if (gqlResponse.Errors?.Length > 0)
                {
                    throw new GraphQLException(gqlResponse.Errors[0]);
                }

                var result = gqlResponse.GetDataFieldAs<T>(section);
                return result;
            }
            finally
            {
                gqlClient.Dispose();
            }
        }
    }
}
