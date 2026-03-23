using System.Linq.Expressions;
using DocDes.Core.Base;

namespace DocDes.Core.Repository;

public interface IRepository<T, TKey> where T : ModelBase<TKey> {
    // ── Create ────────────────────────────────────────────────────────────
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);

    // ── Update ────────────────────────────────────────────────────────────
    void Update(T entity);
    Task UpdateAsync(T entity, CancellationToken ct = default);

    // ── Delete ────────────────────────────────────────────────────────────
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task RemoveAsync(T entity, CancellationToken ct = default);

    // ── Get by Id ─────────────────────────────────────────────────────────
    Task<T?> GetByIdAsync(TKey id, CancellationToken ct = default);

    // ── Exists ────────────────────────────────────────────────────────────
    Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default);

    // ── Count ─────────────────────────────────────────────────────────────
    int Count();
    int Count(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    // ── Find (sync) ───────────────────────────────────────────────────────
    IReadOnlyList<T> Find(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true
    );

    IReadOnlyList<TResult> FindSelect<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true
    );

    // ── Find (async) ──────────────────────────────────────────────────────
    Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true,
        CancellationToken ct = default
    );

    Task<IReadOnlyList<TResult>> FindSelectAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true,
        CancellationToken ct = default
    );

    // ── Find Paged (async) ────────────────────────────────────────────────
    Task<IReadOnlyList<T>> FindPagedAsync(
        Expression<Func<T, bool>> predicate,
        int pageNumber,
        int pageSize,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        CancellationToken ct = default
    );

    // ── FindOne (sync) ────────────────────────────────────────────────────
    T? FindOne(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true
    );

    TResult? FindOneSelect<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true
    );

    // ── FindOne (async) ───────────────────────────────────────────────────
    Task<T?> FindOneAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true,
        CancellationToken ct = default
    );

    Task<TResult?> FindOneSelectAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true,
        CancellationToken ct = default
    );
}