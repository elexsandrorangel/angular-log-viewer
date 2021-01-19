using LogViewer.Repository.Contexts;
using LogViewer.Repository.Entities;
using LogViewer.Repository.Helpers;
using LogViewer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LogViewer.Repository.Implementations
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        protected readonly LogViewerContext Context;

        #region Constructor

        protected BaseRepository(LogViewerContext context)
        {
            Context = context;
        }

        #endregion Constructor

        #region Add

        public virtual async Task<T> AddAsync(T t)
        {
            t.CreatedAt = DateTime.UtcNow;

            var data = Context.Set<T>().Add(t);
            await SaveAsync();
            return data.Entity;
        }

        public virtual async Task<IEnumerable<T>> AddAsync(IEnumerable<T> tList)
        {
            foreach (var item in tList)
            {
                item.CreatedAt = DateTime.UtcNow;
                item.ModifiedAt = DateTime.UtcNow;
            }
            Context.Set<T>().AddRange(tList);
            await SaveAsync();
            return tList;
        }

        public virtual async Task<IEnumerable<T>> AddOrUpdateAsync(IEnumerable<T> tList)
        {
            foreach (var item in tList)
            {
                item.CreatedAt = DateTime.UtcNow;
                item.ModifiedAt = DateTime.UtcNow;
            }
            Context.Set<T>().AddOrUpdate(tList);
            await SaveAsync();
            return tList;
        }

        public virtual async Task<T> AddOrUpdateAsync(T item)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.ModifiedAt = DateTime.UtcNow;
            Context.Set<T>().AddOrUpdate(item);
            await SaveAsync();
            return item;
        }

        #endregion Add

        #region Count

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().AsNoTracking().LongCountAsync(predicate);
        }

        public virtual async Task<long> CountAsync()
        {
            return await Context.Set<T>().AsNoTracking().LongCountAsync();
        }

        #endregion Count

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id, true);
            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(T t)
        {
            Context.Entry<T>(t).State = EntityState.Detached;

            Context.Set<T>().Remove(t);
            await SaveAsync();
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().AsNoTracking().Any(predicate);
        }

        #region Get

        public virtual async Task<IEnumerable<T>> GetAsync(int page = 0, int qty = int.MaxValue, bool track = false)
        {
            if (track)
            {
                return await Context.Set<T>()
                    .OrderBy(a => a.CreatedAt).Skip(page * qty)
                    .Take(qty).ToListAsync();
            }
            return await Context.Set<T>()
                .AsNoTracking()
                .OrderBy(a => a.CreatedAt).Skip(page * qty)
                .Take(qty).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> match, 
            int page = 0, int qty = int.MaxValue, bool track = false)
        {
            if (track)
            {
                return await Context.Set<T>().Where(match)
                    .OrderBy(a => a.CreatedAt)
                    .Skip(page * qty).Take(qty).ToListAsync();
            }
            return await Context.Set<T>().Where(match)
                .AsNoTracking()
                .OrderBy(a => a.CreatedAt)
                .Skip(page * qty).Take(qty).ToListAsync();
        }

        public virtual async Task<T> GetAsync(int id, bool track = false)
        {
            return await GetSingleAsync(x => x.Id == id, track);
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> match, bool track = false)
        {
            if (track)
            {
                return await Context.Set<T>().FirstOrDefaultAsync(match);
            }
            return await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(match);
        }

        public virtual async Task<IEnumerable<T>> GetActiveAsync(int page = 0, int qty = int.MaxValue, 
            bool track = false)
        {
            return await GetAsync(a => a.Active, page, qty, track);
        }

        public virtual async Task<T> GetActiveAsync(int id, bool track = false)
        {
            return await GetSingleAsync(a => a.Id == id && a.Active, track);
        }

        public virtual async Task<IEnumerable<T>> GetInactiveAsync(bool track = false)
        {
            return await GetAsync(a => !a.Active, track: track);
        }

        #endregion Get

        public virtual async Task<int> SaveAsync()
        {

            return await Context.SaveChangesAsync();
        }

        public virtual async Task<T> SaveOrUpdateAsync(T t)
        {
            Context.Set<T>().AddOrUpdate(t);
            await SaveAsync();
            return t;
        }

        public virtual async Task<T> UpdateAsync(T updated)
        {
            return await SaveOrUpdateAsync(updated);
        }

        public virtual async Task<T> UpdateAsync(T updated, int key)
        {
            return await SaveOrUpdateAsync(updated);
        }
    }
}