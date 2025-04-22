using Microsoft.Owin.Security.OAuth;
using MVCAPIWithBearerToken.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace MVCAPIWithBearerToken.Provider
{
    public class AppAuthorizationServerProvider:OAuthAuthorizationServerProvider
    {
        public async override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserRepo repo=new UserRepo())
            {
                var user = repo.ValidateUser(context.UserName,context.Password);
                //if (user != null)
                //{
                //    context.SetError("Invalid_grant", "User or password is incorrect");
                //    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                //    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                //    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                //    foreach (var item in user.Roles.Split(','))
                //    {
                //        identity.AddClaim(new Claim(ClaimTypes.Role, item.Trim()));
                //    }
                //    context.Validated(identity);
                //}

                if (user == null)
                {
                    context.SetError("Invalid_grant", "User or password is incorrect");
                }
                else
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    foreach (var item in user.Roles.Split(','))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, item.Trim()));
                    }
                    context.Validated(identity);
                }
                //if (user == null)
                //    context.SetError("Invalid_grant", "User or password is incorrect");
                //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                //identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                //foreach (var item in user.Roles.Split(','))
                //{
                //    identity.AddClaim(new Claim(ClaimTypes.Role, item.Trim()));
                //}
                //context.Validated(identity);

            }
        }
        public async override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
           context.Validated();
        }
    }
}