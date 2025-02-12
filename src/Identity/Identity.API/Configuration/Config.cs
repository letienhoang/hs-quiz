using IdentityServer8;
using IdentityServer8.Models;

namespace Identity.API.Configuration
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() {
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources() {
            return new List<ApiResource> {
                new ApiResource {
                    Name = "exam_api",
                    DisplayName = "Exam API",
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes() {
            return new List<ApiScope> {
                new ApiScope("full_access", "Full access to Exam API")
            };
        }

        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientUrls) {
            return new List<Client> {
                new Client {
                    ClientId = "exam_web_app",
                    ClientName = "Exam Web App Client",
                    ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                    ClientUri = $"{clientUrls["ExamWebApp"]}",
                    AllowedCorsOrigins = { $"{clientUrls["ExamWebApp"]}" },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new List<string> { $"{clientUrls["ExamWebApp"]}/authentication/login-callback" },
                    PostLogoutRedirectUris = new List<string> { $"{clientUrls["ExamWebApp"]}/authentication/logout-callback" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "full_access",
                    },
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours
                    IdentityTokenLifetime = 60 * 60 * 2, // 2 hours
                    RequireClientSecret = false, // !important for authorization code flow
                },
                new Client {
                    ClientId = "exam_web_admin",
                    ClientName = "Exam Web Admin Client",
                    ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                    ClientUri = $"{clientUrls["ExamWebAdmin"]}",
                    AllowedCorsOrigins = { $"{clientUrls["ExamWebAdmin"]}" },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new List<string> { $"{clientUrls["ExamWebAdmin"]}/authentication/login-callback" },
                    PostLogoutRedirectUris = new List<string> { $"{clientUrls["ExamWebAdmin"]}/authentication/logout-callback" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "full_access",
                    },
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours
                    IdentityTokenLifetime = 60 * 60 * 2, // 2 hours
                    RequireClientSecret = false, // !important for authorization code flow
                },
                new Client {
                    ClientId = "exam_api_swaggerui",
                    ClientName = "Exam API Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["ExamWebApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["ExamWebApi"]}/swagger/" },

                    AllowedScopes = { "full_access" }
                }
            };
        }
    }
}