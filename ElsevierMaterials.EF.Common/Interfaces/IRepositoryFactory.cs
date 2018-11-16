using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.Common.Interfaces
{
    public interface IRepositoryFactory
    {
        Func<DbContext, object> GetRepositoryFactory<T>() where T : class;
        Func<DbContext, object> GetEntityRepositoryFactory<T>() where T : class;
    }
}
