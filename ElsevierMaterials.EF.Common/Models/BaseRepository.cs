using ElsevierMaterials.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.Common.Models
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        public BaseRepository(DbContext dbContext)
        {
            if (dbContext == null)
            { throw new ArgumentNullException("DbContext is missing."); }

            DataContext = dbContext;
        }

        public virtual IQueryable<T> All
        { get { return DataSet; } }
        public virtual ObservableCollection<T> AllLocal
        { get { return DataSet.Local; } }
        public virtual IQueryable<T> AllAsNoTracking
        { get { return DataSet.AsNoTracking(); } }

        public virtual IQueryable<T> GetAllIncluding(
            params Expression<Func<T, object>>[] includeProperties)
        {
            var dataSetQuery = GetAllIncluding(false, includeProperties);

            return dataSetQuery;
        }

        public virtual IQueryable<T> GetAllIncluding(
            bool asNoTracking,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var dataSetQuery = All;
            if (asNoTracking)
            { dataSetQuery = AllAsNoTracking; }

            foreach (var includeProperty in includeProperties)
            { dataSetQuery = dataSetQuery.Include(includeProperty); }

            return dataSetQuery;
        }

        public virtual T Find(params object[] keyValues)
        { return DataSet.Find(keyValues); }

        public virtual T Find(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var entityQuery = DataSet
              .Where<T>(predicate);

            foreach (var includeProperty in includeProperties)
            { entityQuery = entityQuery.Include(includeProperty); }

            return entityQuery.SingleOrDefault();
        }

        public virtual void Add(T entity, bool onlyChangeState = false)
        {
            if (entity == null)
            { return; }

            if (onlyChangeState)
            { EntityEntry(entity).State = EntityState.Added; }
            else
            { DataSet.Add(entity); }
        }

        public virtual void Update(T entity, bool onlyChangeState = false)
        {
            if (entity == null)
            { return; }

            EntityEntry(entity).State = EntityState.Modified;

            if (!onlyChangeState)
            {
                var databaseValues = EntityEntry(entity).GetDatabaseValues();
                if (databaseValues != null)
                {
                    DataSet.Attach(entity);
                    EntityEntry(entity).OriginalValues.SetValues(databaseValues);
                }
            }
        }

        public virtual void Remove(T entity, bool onlyChangeState = false)
        {
            if (entity == null)
            { return; }

            if (onlyChangeState)
            { EntityEntry(entity).State = EntityState.Deleted; }
            else
            { DataSet.Remove(entity); }
        }

        public virtual void Remove(
            Expression<Func<T, bool>> predicate,
            bool onlyChangeState = false,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var entity = Find(predicate, includeProperties);
            Remove(entity, onlyChangeState);
        }

        public virtual void Remove(params object[] keyValues)
        {
            var entity = Find(keyValues);
            Remove(entity);
        }

        public virtual void Remove(
            object[] keyValues,
            bool onlyChangeState = false)
        {
            var entity = Find(keyValues);
            Remove(entity, onlyChangeState);
        }

        public virtual void Attach(T entity)
        { DataSet.Attach(entity); }

        public virtual IEnumerable<T> SqlQuery(
            string commandText,
            bool withTracking = false,
            params object[] parameters)
        {
            var datasetSqlQuery = DataSet.SqlQuery(commandText, parameters);
            if (!withTracking)
            { datasetSqlQuery = datasetSqlQuery.AsNoTracking(); }

            return datasetSqlQuery;
        }

        public virtual IEnumerable<TType> SqlQuery<TType>(
            string commandText,
            params object[] parameters)
        { return DataContext.Database.SqlQuery<TType>(commandText, parameters); }

        public IUowCommandResult ExecuteSqlCommand(
            string commandText,
            params object[] parameters)
        {
            var uowCommandResult = new UowCommandResult();

            try
            {
                uowCommandResult.RecordsAffected =
                    DataContext.Database.ExecuteSqlCommand(commandText, parameters);
                uowCommandResult.HasError = uowCommandResult.RecordsAffected < 1;
                uowCommandResult.Description = String.Format(
                    "{0} rows affected.",
                    uowCommandResult.RecordsAffected);
            }
            catch (Exception exception)
            {
                uowCommandResult.Description = "UnexpectedException";
                uowCommandResult.Exception = exception;
            }

            return uowCommandResult;
        }

        protected DbEntityEntry<T> EntityEntry(T entity)
        { return DataContext.Entry<T>(entity); }

        protected DbCollectionEntry<T, TElement> ChildrenEntry<TElement>(
            T entity, Expression<Func<T, ICollection<TElement>>> navigationProperty) where TElement : class
        { return EntityEntry(entity).Collection<TElement>(navigationProperty); }

        protected DbContext DataContext { get; private set; }
        protected DbSet<T> DataSet { get { return DataContext.Set<T>(); } }
    }
}
