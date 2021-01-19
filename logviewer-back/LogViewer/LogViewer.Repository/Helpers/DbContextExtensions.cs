using LogViewer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LogViewer.Repository.Helpers
{
    public static class DbContextExtensions
    {
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T record)
               where T : BaseEntity
        {
            var exists = dbSet.AsNoTracking().Any(x => x.Id == record.Id);
            if (exists)
            {
                dbSet.Update(record);
            }
            else
            {
                dbSet.Add(record);
            }
        }

        public static void AddOrUpdate<T>(this DbSet<T> dbSet, IEnumerable<T> records)
            where T : BaseEntity
        {
            foreach (var data in records)
            {
                var exists = dbSet.AsNoTracking().Any(x => x.Id == data.Id);
                if (exists)
                {
                    dbSet.Update(data);
                    continue;
                }
                dbSet.Add(data);
            }
        }
    }
}
