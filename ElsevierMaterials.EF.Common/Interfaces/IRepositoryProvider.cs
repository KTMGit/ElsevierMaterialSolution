using ElsevierMaterials.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.Common.Interfaces
{
    public interface IRepositoryProvider
    {
        DbContext DbContext { get; set; }

        void BindRepository<TFrom, TTo>() where TTo : TFrom;
        T GetRepository<T>(Func<DbContext, object> factory = null) where T : class;
        T GetRepository<T>(
            DbContext dbContext, Func<DbContext, object> factory = null) where T : class;
        IRepository<T> GetEntityRepository<T>() where T : class;
        IRepository<T> GetEntityRepository<T>(DbContext dbContext) where T : class;
        void SetRepository<T>(T repository);
    }
}
