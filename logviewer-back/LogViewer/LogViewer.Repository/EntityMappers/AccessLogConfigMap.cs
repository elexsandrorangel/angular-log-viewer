using LogViewer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogViewer.Repository.EntityMappers
{
    public class AccessLogConfigMap : BaseEntityConfiguration<AccessLog>
    {
        public AccessLogConfigMap() : base("access_logs")
        {
        }

        public override void Configure(EntityTypeBuilder<AccessLog> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.RemoteAddress).HasMaxLength(256)
                .HasColumnName("remote_address")
                .IsRequired();

            builder.Property(p => p.RemoteUser).HasMaxLength(256)
                .HasColumnName("remote_user");
            builder.Property(p => p.Time).HasColumnName("request_time").IsRequired();
            builder.Property(p => p.RequestUrl)
                .HasColumnName("request_url")
                .HasMaxLength(1000).IsRequired();
            builder.Property(p => p.Status).HasColumnName("http_status").IsRequired();
            builder.Property(p => p.BytesSent).HasColumnName("bytes_sent");
            builder.Property(p => p.HttpReferer).HasColumnName("http_referer").HasMaxLength(1000);
            builder.Property(p => p.UserAgent).HasColumnName("user_agent")
                .HasMaxLength(1000).IsRequired();
            builder.Property(p => p.GzipRatio).HasColumnName("gzip_ratio");
        }
    }
}
