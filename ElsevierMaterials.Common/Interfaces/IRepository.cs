using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace ElsevierMaterials.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All { get; }
        ObservableCollection<T> AllLocal { get; }
        IQueryable<T> AllAsNoTracking { get; }

        IQueryable<T> GetAllIncluding(
            bool asNoTracking,
            params Expression<Func<T, object>>[] includeProperties);
        
        IQueryable<T> GetAllIncluding(
            params Expression<Func<T, object>>[] includeProperties);
        
        T Find(params object[] keyValues);
        T Find(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity, bool onlyChangeState = false);
        void Update(T entity, bool onlyChangeState = false);
        void Remove(T entity, bool onlyChangeState = false);
        void Remove(
            Expression<Func<T, bool>> predicate,
            bool onlyChangeState = false,
            params Expression<Func<T, object>>[] includeProperties);
        void Remove(params object[] keyValues);
        void Remove(object[] keyValues, bool onlyChangeState = false);
        void Attach(T entity);
        IEnumerable<T> SqlQuery(
            string commandText,
            bool withTracking = false,
            params object[] parameters);
        IEnumerable<TType> SqlQuery<TType>(
            string commandText,
            params object[] parameters);
        
        IUowCommandResult ExecuteSqlCommand(
            string commandText,
            params object[] parameters);
    }
}
