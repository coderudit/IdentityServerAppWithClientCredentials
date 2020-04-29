// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        //Resources that tells about User's Identity
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "office",
                    UserClaims = { "office_number" },
                    DisplayName = "Office Details"
                }
            };

        /*API is a resource in your system that you want to protect.
          This is code as configuration approach, we can use other approach as well.eg: 
          taking these resources from DB or Configuration.*/
        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            { new ApiResource("securecoreapi", "Secure Core API") };

        //Client applications that will access our Identity and API resources
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "ClientAPI",
                    ClientName = "Client API",
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // no interactive user, use the clientid/secret for authentication
                    ClientSecrets = { new Secret("secret".Sha256()) }, // secret for authentication
                    AllowedScopes = { "securecoreapi" } // scopes that client has access to
                },
                new Client
                {
                    ClientId = "MVCApp",
                    ClientName ="MVC App",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "openid", "profile" ,"email", "office", "securecoreapi" },
                    RedirectUris = { "https://localhost:5004/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5004/signout-callback-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5004/signout-oidc",
                    AllowOfflineAccess = true
                }
            };
    }
}