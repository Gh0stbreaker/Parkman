using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Parkman.Infrastructure.Repositories;
using Parkman.Shared;

namespace Parkman.Infrastructure.Services;

public class GenericService<TEntity> : IGenericService<TEntity>
    where TEntity : class
{
    private readonly IGenericRepository<TEntity> _repository;

    public GenericService(IGenericRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public Task<TEntity?> GetByIdAsync(object id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        int? skip = null,
        int? take = null,
        string? search = null)
    {
        return _repository.ListAsync(filter, orderBy, includeProperties, skip, take, search);
    }

    public Task<PagedResult<TEntity>> ListPagedAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        int? skip = null,
        int? take = null,
        string? search = null)
    {
        return _repository.ListPagedAsync(filter, orderBy, includeProperties, skip, take, search);
    }

    public Task<TEntity> AddAsync(TEntity entity)
    {
        return _repository.AddAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        return _repository.UpdateAsync(entity);
    }

    public Task DeleteAsync(TEntity entity)
    {
        return _repository.DeleteAsync(entity);
    }
}
