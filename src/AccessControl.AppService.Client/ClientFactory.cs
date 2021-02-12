using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AccessControl.AppService.Contracts;

namespace AccessControl.AppService
{
    public static class ClientFactory
    {
        //public static ITokenClient CreateTokenClient(string baseUrl, HttpClient http)
        //{
        //    return new TokenClient(baseUrl, http);
        //}

        public static IItemsClient CreateItemsClient(string baseUrl, HttpClient http, Func<Task<string>> retrieveAuthorizationToken)
        {
            return new ItemsClient(baseUrl, http)
            {
                RetrieveAuthorizationToken = retrieveAuthorizationToken
            };
        }

        public static IAlarmClient CreateAlarmClient(string baseUrl, HttpClient http, Func<Task<string>> retrieveAuthorizationToken)
        {
            return new AlarmClient(baseUrl, http)
            {
                RetrieveAuthorizationToken = retrieveAuthorizationToken
            };
        }

        public static ITokenClient CreateTokenClient(string baseUrl, HttpClient http, Func<Task<string>> retrieveAuthorizationToken)
        {
            return new TokenClient(baseUrl, http)
            {
                RetrieveAuthorizationToken = retrieveAuthorizationToken
            };
        }

        public static IRegistrationClient CreateRegistrationClient(string baseUrl, HttpClient http, Func<Task<string>> retrieveAuthorizationToken)
        {
            return new RegistrationClient(baseUrl, http)
            {
                RetrieveAuthorizationToken = retrieveAuthorizationToken
            };
        }

        public static IUserClient CreateUserClient(string baseUrl, HttpClient http, Func<Task<string>> retrieveAuthorizationToken)
        {
            return new UserClient(baseUrl, http)
            {
                RetrieveAuthorizationToken = retrieveAuthorizationToken
            };
        }

        public static IAccessLogClient CreateAccessLogClient(string baseUrl, HttpClient http, Func<Task<string>> retrieveAuthorizationToken)
        {
            return new AccessLogClient(baseUrl, http)
            {
                RetrieveAuthorizationToken = retrieveAuthorizationToken
            };
        }

        public static IIdentitiesClient CreateIdentitiesClient(string baseUrl, HttpClient http, Func<Task<string>> retrieveAuthorizationToken)
        {
            return new IdentitiesClient(baseUrl, http)
            {
                RetrieveAuthorizationToken = retrieveAuthorizationToken
            };
        }

        public static IAuthorizationClient CreateAuthorizationClient(string baseUrl, HttpClient http, Func<Task<string>> retrieveAuthorizationToken)
        {
            return new AuthorizationClient(baseUrl, http)
            {
                RetrieveAuthorizationToken = retrieveAuthorizationToken
            };
        }
    }
}
