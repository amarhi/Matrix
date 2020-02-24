using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Matrix.StartupOwin))]

namespace Matrix
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
