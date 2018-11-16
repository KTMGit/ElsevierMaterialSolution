using ElsevierMaterials.EF.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.Common.Factory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public RepositoryFactory()
            : this(new Dictionary<Type, Func<DbContext, object>>()) { }
        public RepositoryFactory(
            IDictionary<Type, Func<DbContext, object>> factories)
        {
            _factories = factories;
        }

        public Func<DbContext, object> GetEntityRepositoryFactory<T>() where T : class
        { return GetRepositoryFactory<T>() ?? MakeEntityRepositoryFactory<T>(); }

        public Func<DbContext, object> GetRepositoryFactory<T>() where T : class
        {
            Func<DbContext, object> factory;
            _factories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        protected virtual Func<DbContext, object> MakeEntityRepositoryFactory<T>() where T : class
        { return dbContext => new BaseRepository<T>(dbContext); }

        private readonly IDictionary<Type, Func<DbContext, object>> _factories;
    }
}
