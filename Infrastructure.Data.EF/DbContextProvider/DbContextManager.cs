using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Ef.DbContextProvider
{
    public static class DbContextManager
    {
        // 线程锁
        private static object _syncLock = new object();
        // DbContext仓库
        private static IDbContextStorage _storage;
        // DbContext组装器容器
        private static Dictionary<string, IDbContextBuilder<DbContext>> _dbContextBuilders =
            new Dictionary<string, IDbContextBuilder<DbContext>>();

        /// <summary>
        /// 初始化ˉDbContext仓库
        /// </summary>
        public static void InitStorage(IDbContextStorage storage)
        {
            if (storage == null)
                throw new ArgumentNullException("storage");

            if (_storage != null && _storage != storage)
                throw new ApplicationException("应用程序中已存在DbContextStorage，请确认是否重新初始化");

            _storage = storage;
        }

        /// <summary>
        /// 初始化DbContext
        /// </summary>
        /// <param name="connectionStringName">连接字符串名称</param>
        /// <param name="mappingAssemblyPath">映射配置类所在DLL路径</param>
        /// <param name="mappingNamespace">映射配置类所在命名空间</param>
        public static void Init(string connectionStringName, string mappingAssemblyPath, string mappingNamespace)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentNullException("connectionStringName");
            if (string.IsNullOrEmpty(mappingAssemblyPath))
                throw new ArgumentNullException("mappingAssemblyPath");
            if (string.IsNullOrEmpty(mappingNamespace))
                throw new ArgumentNullException("mappingNamespace");

            lock (_syncLock)
            {
                _dbContextBuilders.Add(connectionStringName,
                    new DbContextBuilder<DbContext>(connectionStringName, mappingAssemblyPath, mappingNamespace));
            }
        }

        /// <summary>
        /// 根据KEY获取DbContext
        /// </summary> 
        internal static DbContext CurrentByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (_storage == null)
                throw new ApplicationException("DbContextStorage尚未初始化！");

            DbContext context;
            lock (_syncLock)
            {
                if (!_dbContextBuilders.ContainsKey(key))
                    throw new ApplicationException("DbContextBuilders中没有该KEY对应的组装器");

                context = _storage.GetByKey(key);
                if (context == null)
                {
                    context = _dbContextBuilders[key].BuildDbContext();
                    _storage.SetByKey(key, context);
                }
            }
            return context;
        }

        /// <summary>
        /// 关闭所有数据库连接
        /// </summary>
        internal static void CloseAllDbContexts()
        {
            foreach (var dbContext in _storage.GetAllDbContexts())
            {
                if (dbContext.Database.Connection.State == System.Data.ConnectionState.Open)
                    dbContext.Database.Connection.Close();
            }
        }
    }
}
