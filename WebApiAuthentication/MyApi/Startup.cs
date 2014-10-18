using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyApi.Startup))]

namespace MyApi
{
    using Microsoft.Owin.Cors;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Allowing all requests for the sample. This might get populated from a configuration.
            // app.UseCors(CorsOptions.AllowAll);
        }
    }
}
