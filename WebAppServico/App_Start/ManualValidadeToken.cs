using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;

namespace WebAppServico.App_Start
{
    //https://github.com/auth0-samples/auth0-dotnet-validate-jwt/tree/master/
    public class ManualValidadeToken
    {
        private static readonly object mutex = new object();
        private static ManualValidadeToken instance = null;
        
        private static TokenValidationParameters validationParameters = null;
        private static JwtSecurityTokenHandler handler = null;

        private ManualValidadeToken()
        {
            string auth0Domain = System.Configuration.ConfigurationManager.AppSettings["auth0Domain"];
            string auth0Audience = System.Configuration.ConfigurationManager.AppSettings["ida:Audience"];
            Microsoft.IdentityModel.Protocols.IConfigurationManager<Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration> configurationManager =
                new Microsoft.IdentityModel.Protocols.ConfigurationManager<Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration>($"{auth0Domain}.well-known/openid-configuration", new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfigurationRetriever());
            Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration openIdConfig = AsyncHelper.RunSync(async () => await configurationManager.GetConfigurationAsync(CancellationToken.None));

            validationParameters = new TokenValidationParameters
            {
                ValidIssuer = auth0Domain,
                ValidAudiences = new[] { auth0Audience },
                IssuerSigningKeys = openIdConfig.SigningKeys
            };            
            handler = new JwtSecurityTokenHandler();
        }

        public static ManualValidadeToken GetInstance()
        {
            if (instance == null)
            {
                lock (mutex)
                {
                    if (instance == null)
                    {
                        instance = new ManualValidadeToken();
                    }
                }
            }

            return instance;
        }

        public bool Authorize(string token)
        {
            try
            {
                SecurityToken validatedToken;
                var user = handler.ValidateToken(token, validationParameters, out validatedToken);
                // The ValidateToken method above will return a ClaimsPrincipal. Get the user ID from the NameIdentifier claim
                // (The sub claim from the JWT will be translated to the NameIdentifier claim)
                //Console.WriteLine($"Token is validated. User Id {user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }       

        }
    }
}