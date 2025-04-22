using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using MVCAPIWithBearerToken.Provider;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(MVCAPIWithBearerToken.Startup))]
//eta startup class a thke. ebong OwinStartup diye kaj kore.

namespace MVCAPIWithBearerToken
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan=TimeSpan.FromMinutes(60),
                Provider=new AppAuthorizationServerProvider()
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
           
        }
    }
}
