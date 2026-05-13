using Microsoft.EntityFrameworkCore;
using VC_IMS.Models.Outbox;

namespace VC_IMS.Data
{
    public partial class VC_IMSIdentityDbContext
    {
        public virtual DbSet<EmailOutbox> EmailOutbox { get; set; } = default!;
        public virtual DbSet<EmailDeadLetter> EmailDeadLetters { get; set; } = default!;

        private void MapOperations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailOutbox>(b =>
            {
                b.ToTable("VC_email_outbox", schema: "dbo");
                b.HasKey(x => x.Id);

                b.Property(x => x.To).IsRequired().HasMaxLength(512);
                b.Property(x => x.Cc).HasMaxLength(1024);
                b.Property(x => x.Bcc).HasMaxLength(1024);
                b.Property(x => x.Subject).IsRequired().HasMaxLength(512);
                b.Property(x => x.HeadersJson).HasColumnType("nvarchar(max)");

                b.HasIndex(x => new { x.SentUtc, x.NextAttemptUtc });
                b.HasIndex(x => x.CreatedUtc);
            });

            modelBuilder.Entity<EmailDeadLetter>(b =>
            {
                b.ToTable("VC_email_deadletter", schema: "dbo");
                b.HasKey(x => x.Id);

                b.Property(x => x.To).IsRequired().HasMaxLength(512);
                b.Property(x => x.Subject).IsRequired().HasMaxLength(512);
                b.Property(x => x.HeadersJson).HasColumnType("nvarchar(max)");
                b.Property(x => x.Error).IsRequired().HasColumnType("nvarchar(max)");
            });
        }
    }
}
