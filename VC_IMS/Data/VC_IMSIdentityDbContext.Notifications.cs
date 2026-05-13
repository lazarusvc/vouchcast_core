using Microsoft.EntityFrameworkCore;
using VC_IMS.Models.Notifications;

namespace VC_IMS.Data
{
    public partial class VC_IMSIdentityDbContext
    {
        public virtual DbSet<Notification> Notifications { get; set; } = default!;
        public virtual DbSet<NotificationPreference> NotificationPreferences { get; set; } = default!;

        private void MapNotifications(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(b =>
            {
                b.ToTable("VC_notifications", schema: "dbo");
                b.HasKey(x => x.Id);

                b.Property(x => x.Username).HasMaxLength(256);
                b.Property(x => x.Type).IsRequired().HasMaxLength(128);
                b.Property(x => x.PayloadJson).HasColumnType("nvarchar(max)");

                b.HasIndex(x => new { x.UserId, x.Seen, x.CreatedUtc });
                b.HasIndex(x => x.CreatedUtc);
            });

            modelBuilder.Entity<NotificationPreference>(b =>
            {
                b.ToTable("VC_notification_prefs", schema: "dbo");
                b.HasKey(x => x.Id);

                b.Property(x => x.Type).HasMaxLength(128);
                b.HasIndex(x => new { x.UserId, x.Type }).IsUnique();
            });
        }
    }
}
