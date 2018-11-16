using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.Common.Factory
{
    public class RepositoryProvider : IRepositoryProvider
    {
        public RepositoryProvider() :
            this(new Dictionary<Type, object>(), new RepositoryFactory()) { }
        public RepositoryProvider(
            IDictionary<Type, object> repositories,
            IRepositoryFactory repositoryFactory)
        {
            _repositories = repositories;
            _repositoryFactory = repositoryFactory;
        }

        public DbContext DbContext { get; set; }

        public void BindRepository<TFrom, TTo>() where TTo : TFrom
        { SetRepository((TFrom)Activator.CreateInstance(typeof(TTo), DbContext)); }

        public T GetRepository<T>(Func<DbContext, object> factory = null) where T : class
        {
            return GetRepository<T>(DbContext, factory);
        }

        public T GetRepository<T>(
            DbContext dbContext, Func<DbContext, object> factory = null) where T : class
        {
            object repository;
            _repositories.TryGetValue(typeof(T), out repository);

            if (repository != null)
            { return (T)repository; }

            return MakeRepository<T>(dbContext, factory);
        }

        public IRepository<T> GetEntityRepository<T>() where T : class
        {
            return GetRepository<IRepository<T>>(
                _repositoryFactory.GetEntityRepositoryFactory<T>());
        }

        public IRepository<T> GetEntityRepository<T>(DbContext dbContext) where T : class
        {
            return GetRepository<IRepository<T>>(dbContext,
                _repositoryFactory.GetEntityRepositoryFactory<T>());
        }

        public void SetRepository<T>(T repository)
        { _repositories[typeof(T)] = repository; }

        private T MakeRepository<T>(
            DbContext dbContext, Func<DbContext, object> factory) where T : class
        {
            var f = factory ?? _repositoryFactory.GetRepositoryFactory<T>();
            if (f == null)
            {
                throw new NotImplementedException(
                    "No factory for repository type " + typeof(T).FullName);
            }

            var repository = (T)f(dbContext);
            SetRepository(repository);

            return repository;
        }

        private readonly IDictionary<Type, object> _repositories;
        private IRepositoryFactory _repositoryFactory;
    }
}
