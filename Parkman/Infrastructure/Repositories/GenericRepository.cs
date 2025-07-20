using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Parkman.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null,
            string? search = null)
        {
            IQueryable<TEntity> query = _dbSet;

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

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
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
