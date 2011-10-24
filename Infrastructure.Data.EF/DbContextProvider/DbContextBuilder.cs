using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Data.Ef.DbContextProvider
{
    public class DbContextBuilder<T> : DbModelBuilder, IDbContextBuilder<T> where T : DbContext
    {
        // 数据库客户端ProviderFactory
        private readonly DbProviderFactory _factory;
        // 连接字符串配置
        private readonly ConnectionStringSettings _cnStringSettings;
        // 如果数据库不存在是否创建
        private readonly bool _recreateDatabaseIfExists;
        // 是否LazyLoad
        private readonly bool _lazyLoadingEnabled;

        public DbContextBuilder(string connectionStringName, string mappingAssemblyPath, string mappingNamespace, bool recreateDatabaseIfExists = false, bool lazyLoadingEnabled = true)
        {
            this.Conventions.Remove<IncludeMetadataConvention>();

            _cnStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            _factory = DbProviderFactories.GetFactory(_cnStringSettings.ProviderName);
            _recreateDatabaseIfExists = recreateDatabaseIfExists;
            _lazyLoadingEnabled = lazyLoadingEnabled;

            AddConfigurations(mappingAssemblyPath, mappingNamespace);
        }

        private void AddConfigurations(string mappingAssemblyPath, string mappingNamespace)
        {
            if (string.IsNullOrEmpty(mappingAssemblyPath))
                throw new ArgumentNullException("mappingAssemblyPath");

            if (string.IsNullOrEmpty(mappingNamespace))
                throw new ArgumentNullException("mappingNamespace");

            var hashMapping = false;
            // 关键代码
            var asm = Assembly.LoadFrom(GetAssemblyPath(mappingAssemblyPath));
            foreach (var type in asm.GetTypes().Where(c =>
                !c.IsAbstract
                && c.BaseType != null
                && c.BaseType.IsGenericType
                && c.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>))
                )
            {
                hashMapping = true;
                dynamic configurationInstance = Activator.CreateInstance(type);
                Configurations.Add(configurationInstance);
            }

            if (!hashMapping)
                throw new ApplicationException("DbContext组装时，没有发现映射配置类");
        }

        private static string GetAssemblyPath(string assemblyName)
        {
            return (assemblyName.IndexOf(".dll") == -1)
                ? assemblyName.Trim() + ".dll"
                : assemblyName.Trim();
        }

        #region Implementation of IDbContextBuilder<T>

        /// <summary>
        /// 组装DbContext
        /// </summary>
        /// <returns>DbContext</returns>
        public T BuildDbContext()
        {
            // 关键代码
            var cn = _factory.CreateConnection();
            cn.ConnectionString = _cnStringSettings.ConnectionString;

            var dbModel = this.Build(cn);

            var ctx = dbModel.Compile().CreateObjectContext<ObjectContext>(cn);
            ctx.ContextOptions.LazyLoadingEnabled = this._lazyLoadingEnabled;

            if (!ctx.DatabaseExists())
            {
                ctx.CreateDatabase();
            }
            else if (_recreateDatabaseIfExists)
            {
                ctx.DeleteDatabase();
                ctx.CreateDatabase();
            }

            return (T)new DbContext(ctx, false);
        }

        #endregion
    }
}
