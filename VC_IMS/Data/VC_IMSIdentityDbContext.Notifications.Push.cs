using Microsoft.EntityFrameworkCore;
using VC_IMS.Models.Notifications;

namespace VC_IMS.Data
{
    public partial class VC_IMSIdentityDbContext
    {
        public virtual DbSet<UserPushSubscription> PushSubscriptions { get; set; } = default!;

        private void MapPush(ModelBuilder b)
        {
            b.Entity<UserPushSubscription>(e =>
            {
                e.ToTable("VC_push_subscriptions", schema: "dbo");
                e.HasKey(x => x.Id);

                e.Property(x => x.Endpoint).HasMaxLength(1000).IsRequired();
                e.Property(x => x.P256dh).HasMaxLength(256).IsRequired();
                e.Property(x => x.Auth).HasMaxLength(256).IsRequired();
                e.Property(x => x.UserAgent).HasMaxLength(512);

                e.HasIndex(x => x.UserId);
                e.HasIndex(x => x.Endpoint).IsUnique();
            });
        }
    }
}
