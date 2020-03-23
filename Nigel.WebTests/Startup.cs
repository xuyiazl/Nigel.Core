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

            //DBContext��ע��
            #region ע��ֻд���� Write_FrontService

            services.AddDbContext<WriteEntityContext>(options =>
            {
                //ʹ��mysql������,ʹ��ָ�������ַ���
                options.UseMySql(Configuration.GetConnectionString("Nigel_WriteConnection"), opts =>
                {

                }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            //�־ò�ֿ��ע��
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IWriteEntityContext), typeof(WriteEntityContext));

            #endregion

            #region  ע��ֻ������ Read_FrontService

            services.AddDbContext<ReadEntityContext>(options =>
            {
                //ʹ��mysql������,ʹ��ָ�������ַ���
                options.UseMySql(Configuration.GetConnectionString("Nigel_ReadConnection"), opts =>
                {

                }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            });
            //�־ò�ֿ��ע��
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IReadEntityContext), typeof(ReadEntityContext));

            #endregion

            //DI ע��db�־ò�ҵ���߼�
            services.Scan(scan =>
               scan.FromAssemblyOf<IDbDependencyService>()
               .AddClasses(impl => impl.AssignableTo(typeof(IDbDependencyService)))
               .AsImplementedInterfaces()
               .WithScopedLifetime()
           );


            services.AddHttpService<HttpService>(TimeSpan.FromSeconds(6));

            services.AddRedisService();

            //ע��razor��̬HTML������
            services.AddRazorHtml();

            services.AddHttpService<HttpService>("test", "http://testmswebapi.tostar.top");

            services.AddControllersWithViews(options =>
            {
                //ȫ���쳣������
                //options.Filters.Add<ExceptionHandlerAttribute>();
            })
            .AddNewtonsoftJson(options =>
            {
                //��Ҫ����nuget
                //<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="3.1.0" />
                //EF Core��Ĭ��Ϊ�շ���ʽ���л�����key
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //ʹ��Ĭ�Ϸ�ʽ��������Ԫ���ݵ�key�Ĵ�Сд
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

            //ע��log4net��־
            loggerFactory.AddLog4Net();
            //ע����ʵIP�м��
            app.UseRealIp();
            //���þ�̬����������
            app.UseStaticHttpContext();
            //ȫ��������־�м��
            //app.UseRequestLog();
            //ȫ�ִ�����־�м��
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