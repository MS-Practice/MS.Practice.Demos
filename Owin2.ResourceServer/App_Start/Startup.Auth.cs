using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Owin2.ResourceServer
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
