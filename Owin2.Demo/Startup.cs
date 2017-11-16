using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;
using System.Web;
using System.IO;

[assembly: OwinStartup(typeof(Owin2.Demo.Startup))]

namespace Owin2.Demo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStage(context, "Middleware 1");
                return next.Invoke();
            });
            app.Use((context, next) =>
            {
                PrintCurrentIntegratedPipelineStage(context, "2nd MW");
                return next.Invoke();
            });
            app.UseStageMarker(PipelineStage.Authenticate);
            app.Run(context =>
            {
                PrintCurrentIntegratedPipelineStage(context, "3rd MW");
                return context.Response.WriteAsync("Hello world");
            });
            app.UseStageMarker(PipelineStage.ResolveCache);
        }

        private void PrintCurrentIntegratedPipelineStage(IOwinContext context, string msg)
        {
            var currentIntergratedpipelineStage = HttpContext.Current.CurrentNotification;
            var writer = context.Get<TextWriter>("host.TraceOutput");
            writer.WriteLine(
                $"Current IIS event: {currentIntergratedpipelineStage} Msg: {msg}");
        }
    }
}
