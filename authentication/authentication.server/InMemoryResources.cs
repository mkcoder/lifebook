using System;
using System.Collections.Generic;
using authentication.server.ConfigConstants;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace authentication.server
{
    public static class InMemoryResources
    {
        public static IEnumerable<Client> GetInmemoryClients()
            => new List<Client>()
            {
                new Client
                {
                    ClientId = "admin.ui",
                    ClientName = "admin",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {"" },
                    AllowedScopes = { Constants.Scopes.OPEN_ID, Constants.Scopes.EMAIL },
                }
            };


        public static IEnumerable<IdentityResource> GetInMemoryIdentityResources()
            => new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> GetInMemoryApiResources()
            => new List<ApiResource>
            {
                new ApiResource
                {
                     Name = "admin.ui",
                     DisplayName = "internal application",
                     UserClaims = {},
                     Scopes = {}
                }
            };

        public static List<TestUser> GetTestUsers()
            => new List<TestUser>
            {
                new TestUser()
                {
                    Username ="tester",
                    Password="password",
                    SubjectId="tester"

                }
            };
    }
}