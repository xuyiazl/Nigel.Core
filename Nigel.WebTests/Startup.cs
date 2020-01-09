using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nigel.Core.Extensions;
using Nigel.Core.Filters;
using Nigel.Core.HttpFactory;
using Nigel.Core.Logging.Log4Net;

namespace Nigel.WebTests
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpService<HttpService>(TimeSpan.FromSeconds(6));

            //注册razor静态HTML生成器
            services.AddRazorHtml();

            services.AddHttpService<HttpService>("test", "http://testmswebapi.tostar.top");

            services.AddControllersWithViews(options =>
            {
                //全局异常过滤器
                //options.Filters.Add<ExceptionHandlerAttribute>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //注册log4net日志
            loggerFactory.AddLog4Net();
            //注册真实IP中间件
            app.UseRealIp();
            //启用静态请求上下文
            app.UseStaticHttpContext();
            //全局请求日志中间件
            //app.UseRequestLog();
            //全局错误日志中间件
            //app.UseErrorLog();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
