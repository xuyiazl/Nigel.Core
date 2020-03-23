using Microsoft.EntityFrameworkCore;
using Nigel.Data.DbService;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nigel.WebTests.Data.Repository
{
    public abstract class BaseRepositoryFactory : DBContextFactory
    {
        protected BaseRepositoryFactory(DbContextOptions options, string mappingPath) : base(options, mappingPath)
        {
            foreach (var extensions in options.Extensions)
            {
                if (extensions.GetType().FullName.Equals("Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal.MySqlOptionsExtension"))
                {
                    this.ConnectionStrings = (extensions as MySqlOptionsExtension).ConnectionString;
                }
            }
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public string ConnectionStrings { get; set; }

        /// <summary>
        /// EF依赖mappingPath，将当前项目文件夹的Entity的映射文件执行注入操作
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //扫描指定文件夹的
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !String.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractEntityTypeConfiguration<>));
            string namespce = mappingPath;
            typesToRegister = typesToRegister.Where(a => a.Namespace.Contains(namespce));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}
