using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nigel.Core.Extensions;
using System.Text;

namespace Nigel.WebTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    //зЂВсецЪЕIP
                    .UseRealIp()
                    .UseStartup<Startup>();
                });
    }
}