using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Parkman.Infrastructure.Exceptions;
using Parkman.Shared;

namespace Parkman.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> DbSet;
        private readonly ILogger<GenericRepository<TEntity>> _logger;

        public GenericRepository(
            ApplicationDbContext context,
            ILogger<GenericRepository<TEntity>> logger)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
            _logger = logger;
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null,
            string? search = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = ApplySearch(query, search);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<PagedResult<TEntity>> ListPagedAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null,
            string? search = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = ApplySearch(query, search);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var totalCount = await query.CountAsync();

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            var items = await query.AsNoTracking().ToListAsync();
            return new PagedResult<TEntity>(items, totalCount);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                await Context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding entity of type {Entity}", typeof(TEntity).Name);
                throw new RepositoryException("Error adding entity", ex);
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                DbSet.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating entity of type {Entity}", typeof(TEntity).Name);
                throw new RepositoryException("Error updating entity", ex);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            try
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    DbSet.Attach(entity);
                }
                DbSet.Remove(entity);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting entity of type {Entity}", typeof(TEntity).Name);
                throw new RepositoryException("Error deleting entity", ex);
            }
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return Context.Database.BeginTransactionAsync();
        }

        private static IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, string search)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression? predicate = null;

            var searchPattern = Expression.Constant($"%{search}%");
            var functions = Expression.Property(null, typeof(EF), nameof(EF.Functions));
            var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
                nameof(DbFunctionsExtensions.Like),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) })!;

            foreach (var property in typeof(TEntity).GetProperties().Where(p => p.PropertyType == typeof(string)))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var likeCall = Expression.Call(likeMethod, functions, propertyAccess, searchPattern);
                predicate = predicate == null ? likeCall : Expression.OrElse(predicate, likeCall);
            }

            if (predicate == null)
            {
                return query;
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);
            return query.Where(lambda);
        }
    }
}
