using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nigel.Core.Extensions;
using Nigel.Core.HttpFactory;
using Nigel.Core.Logging.Log4Net;
using Nigel.Core.Redis;
using Nigel.WebTests.Data.DbService;
using Nigel.WebTests.Data.Repository.ReadRepository;
using Nigel.WebTests.Data.Repository.WriteRepository;
using System;

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

            //DBContext的注入
            #region 注入只写操作 Write_FrontService

            services.AddDbContext<WriteEntityContext>(options =>
            {
                //使用mysql的连接,使用指定连接字符串
                options.UseMySql(Configuration.GetConnectionString("Nigel_WriteConnection"), opts =>
                {

                }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            //持久层仓库的注入
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IWriteEntityContext), typeof(WriteEntityContext));

            #endregion

            #region  注入只读操作 Read_FrontService

            services.AddDbContext<ReadEntityContext>(options =>
            {
                //使用mysql的连接,使用指定连接字符串
                options.UseMySql(Configuration.GetConnectionString("Nigel_ReadConnection"), opts =>
                {

                }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            });
            //持久层仓库的注入
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IReadEntityContext), typeof(ReadEntityContext));

            #endregion

            //DI 注入db持久层业务逻辑
            services.Scan(scan =>
               scan.FromAssemblyOf<IDbDependencyService>()
               .AddClasses(impl => impl.AssignableTo(typeof(IDbDependencyService)))
               .AsImplementedInterfaces()
               .WithScopedLifetime()
           );


            services.AddHttpService<HttpService>(TimeSpan.FromSeconds(6));

            services.AddRedisService();

            //注册razor静态HTML生成器
            services.AddRazorHtml();

            services.AddHttpService<HttpService>("test", "http://testmswebapi.tostar.top");

            services.AddControllersWithViews(options =>
            {
                //全局异常过滤器
                //options.Filters.Add<ExceptionHandlerAttribute>();
            })
            .AddNewtonsoftJson(options =>
            {
                //需要引入nuget
                //<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="3.1.0" />
                //EF Core中默认为驼峰样式序列化处理key
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //使用默认方式，不更改元数据的key的大小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();//new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddUploadService();
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