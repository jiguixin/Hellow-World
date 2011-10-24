using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Ef.DbContextProvider
{
    /// <summary>
    /// DbContext组装接口
    /// </summary>
    public interface IDbContextBuilder<T> where T : DbContext
    {
        /// <summary>
        /// 组装DbContext
        /// </summary>
        /// <returns>DbContext</returns>
        T BuildDbContext();
    }
}
