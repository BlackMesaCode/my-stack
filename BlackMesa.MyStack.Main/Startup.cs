using BlackMesa.MyStack.Main;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace BlackMesa.MyStack.Main
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
