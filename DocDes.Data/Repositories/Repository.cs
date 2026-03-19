using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using DocDes.Core.Base;
using DocDes.Core.Model;
using DocDes.Core.Repository;

namespace DocDes.Data.Repositories
{
    /// <summary>
    /// Tüm entity'ler için ortak CRUD + Include + ThenInclude + Asenkron operasyonları
    /// EF Core bağımlılığı sadece burada – Core tamamen temiz kalır
    /// </summary>
    public class Repository<T> : IRepository<T> where T : ModelBase
    {
        protected readonly DocDesDbContext _context;
        protected readonly ILogger<Repository<T>> _logger;
        public Repository(
            DocDesDbContext context,
            ILogger<Repository<T>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        // ADD

        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        {
            await _context.Set<T>().AddRangeAsync(entities, ct);
        }

        // UPDATE
        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public virtual Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        // FIND - Senkron
        public async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
        {
            return await _context.Set<T>().AnyAsync(predicate, ct);
        }
                
        public virtual IEnumerable<T> Find(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy= null,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true)
        {
            var query = ApplyQueryOptions(_context.Set<T>().AsQueryable(), predicate, orderBy, include, asNoTracking);
            return query.ToList();
        }

        public virtual IEnumerable<TResult> FindSelect<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy= null,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true)
        {
            var query = ApplyQueryOptions(_context.Set<T>().AsQueryable(), predicate, orderBy, include, asNoTracking);
            return query.Select(selector).ToList();
        }

        // FIND - Asenkron
        public virtual async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy= null,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true, CancellationToken ct = default)
        {
            var query = ApplyQueryOptions(_context.Set<T>().AsQueryable(), predicate, orderBy, include, asNoTracking);
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TResult>> FindSelectAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy= null,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true, CancellationToken ct = default)
        {
            var query = ApplyQueryOptions(_context.Set<T>().AsQueryable(), predicate, orderBy, include, asNoTracking);
            return await query.Select(selector).ToListAsync();
        }

        // FIND ONE - Senkron
        public virtual T? FindOne(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true)
        {
            var query = ApplyQueryOptions(_context.Set<T>().AsQueryable(), predicate, null, include, asNoTracking);
            return query.FirstOrDefault();
        }

        public virtual TResult? FindOneSelect<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true)
        {
            var query = ApplyQueryOptions(_context.Set<T>().AsQueryable(), predicate, null, include, asNoTracking);
            return query.Select(selector).FirstOrDefault();
        }

        // FIND ONE - Asenkron
        public virtual async Task<T?> FindOneAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true, CancellationToken ct = default)
        {
            var query = ApplyQueryOptions(_context.Set<T>().AsQueryable(), predicate, null, include, asNoTracking);
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<TResult?> FindOneSelectAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IQueryable<T>>? include= null,
            bool asNoTracking = true, CancellationToken ct = default)
        {
            var query = ApplyQueryOptions(_context.Set<T>().AsQueryable(), predicate, null, include, asNoTracking);
            return await query.Select(selector).FirstOrDefaultAsync();
        }

        // COUNT
        public virtual int Count() => _context.Set<T>().Count();
        public virtual int Count(Expression<Func<T, bool>> predicate) => _context.Set<T>().Count(predicate);


        // YARDIMCI METOD – EF Include zincirini sorunsuz çalıştırır
        private IQueryable<T> ApplyQueryOptions(
            IQueryable<T> query,
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
            Func<IQueryable<T>, IQueryable<T>>? include,
            bool asNoTracking)
        {
            if (asNoTracking) query = query.AsNoTracking();
            if (predicate != null) query = query.Where(predicate);
            if (include != null) query = include(query);           // ← Include().ThenInclude() burada çalışır
            if (orderBy != null) query = orderBy(query);
            return query;
        }
    }
}