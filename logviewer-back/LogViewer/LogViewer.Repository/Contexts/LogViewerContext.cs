using LogViewer.Infrastructure;
using LogViewer.Repository.Entities;
using LogViewer.Repository.EntityMappers;
using Microsoft.EntityFrameworkCore;

namespace LogViewer.Repository.Contexts
{
    public class LogViewerContext : DbContext
    {
        public LogViewerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Constants.DefaultSchema);

            modelBuilder.ApplyConfiguration(new AccessLogConfigMap());
            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccessLog> AccessLogs { get; set; }

    }
}
