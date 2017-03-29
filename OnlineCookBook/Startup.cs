using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineCookBook.Startup))]
namespace OnlineCookBook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
