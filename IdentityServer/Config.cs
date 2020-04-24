// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        //API is a resource in your system that you want to protect.
        //This is code as configuration approach, we can use other approach as well.eg: taking these
        //resources from DB or Configuration.
        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            { new ApiResource("SecureCoreAPI", "Secure Core API") };

        //Client applications that we will use to access our API resources
        public static IEnumerable<Client> Clients =>
            new List<Client>
    {
        new Client
        {
            ClientId = "ClientAPI",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "SecureCoreAPI" }
        }
    };

    }
}