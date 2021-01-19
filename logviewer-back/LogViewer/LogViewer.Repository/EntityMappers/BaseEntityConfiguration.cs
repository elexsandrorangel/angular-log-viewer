using LogViewer.Infrastructure;
using LogViewer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogViewer.Repository.EntityMappers
{
    public abstract class BaseEntityConfiguration<TEntityType> : IEntityTypeConfiguration<TEntityType>
         where TEntityType : BaseEntity
    {
        private readonly string tableName;
        private readonly string schema;

        public BaseEntityConfiguration(string name = null, string schemaName = Constants.DefaultSchema)
        {
            tableName = name;
            schema = schemaName;
        }

        public virtual void Configure(EntityTypeBuilder<TEntityType> builder)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                builder.ToTable(tableName, schema);
                builder.HasKey(p => p.Id).HasName($"{tableName.ToLower()}_pkey");
            }
            else
            {
                builder.HasKey(p => p.Id);
            }
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(p => p.Active).HasColumnName("active");
            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.ModifiedAt).HasColumnName("modified_at");
        }
    }
}
