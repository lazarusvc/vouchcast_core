using Microsoft.EntityFrameworkCore;
using VC_IMS.Models.Logging;

namespace VC_IMS.Data
{
    public partial class VC_IMSIdentityDbContext
    {
        public virtual DbSet<AuditLog> AuditLogs { get; set; } = default!;
        public virtual DbSet<SessionLog> SessionLogs { get; set; } = default!;

        private void MapLogging(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(b =>
            {
                b.ToTable("VC_audit_logs", schema: "dbo");
                b.HasKey(x => x.Id);
                b.Property(x => x.Action).IsRequired().HasMaxLength(64);
                b.Property(x => x.Entity).IsRequired().HasMaxLength(256);
                b.Property(x => x.EntityId).HasMaxLength(256);
                b.Property(x => x.Username).HasMaxLength(256);
                b.Property(x => x.Ip).HasMaxLength(64);
                b.Property(x => x.OldValuesJson).HasColumnType("nvarchar(max)");
                b.Property(x => x.NewValuesJson).HasColumnType("nvarchar(max)");

                b.HasIndex(x => new { x.Entity, x.EntityId, x.Utc });
                b.HasIndex(x => new { x.UserId, x.Utc });
            });

            modelBuilder.Entity<SessionLog>(b =>
            {
                b.ToTable("VC_session_logs", schema: "dbo");
                b.HasKey(x => x.Id);

                b.Property(x => x.Username).HasMaxLength(256);
                b.Property(x => x.SessionId).IsRequired().HasMaxLength(64);
                b.Property(x => x.Ip).HasMaxLength(64);
                b.Property(x => x.UserAgent).HasMaxLength(512);

                b.HasIndex(x => new { x.UserId, x.LoginUtc });
                b.HasIndex(x => new { x.UserId, x.SessionId }).IsUnique(false);
                b.HasIndex(x => x.LastSeenUtc);
            });
        }
    }
}
