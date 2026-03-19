using System.Linq.Expressions;
using DocDes.Core.Base;

namespace DocDes.Core.Repository
{
    public interface IRepository<T> where T : ModelBase
    {
        void Add(T entity);
        Task AddAsync(T entity, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);

        void Update(T entity);
        Task UpdateAsync(T entity, CancellationToken ct = default);


        Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default);

        IEnumerable<T> Find(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy= null,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true            
        );

        IEnumerable<TResult> FindSelect<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy= null,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true
        );        

        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy= null,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true, CancellationToken ct = default
        );

        Task<IEnumerable<TResult>> FindSelectAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy= null,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true, CancellationToken ct = default
        );        

        T? FindOne(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true
        );

        TResult? FindOneSelect<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true
        );

        Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true, CancellationToken ct = default
        );

        Task<TResult?> FindOneSelectAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true, CancellationToken ct = default
        );

        int Count();
        int Count(Expression<Func<T, bool>> predicate);


    }

}
