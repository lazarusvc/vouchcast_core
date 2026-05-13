using Microsoft.EntityFrameworkCore;
using VC_IMS.Models.Security;

namespace VC_IMS.Data
{
    public partial class VC_IMSIdentityDbContext
    {
        public virtual DbSet<PublicEndpoint> PublicEndpoints { get; set; } = default!;
        public virtual DbSet<EndpointPolicyAssignment> EndpointPolicyAssignments { get; set; } = default!;


        // >>> NO OnModelCreatingPartial HERE
        // Helper invoked from the single implementation in the AuthPolicies Partial
        private void MapAccessControl(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublicEndpoint>(b =>
            {
                b.ToTable("VC_public_endpoints", "dbo");
                b.HasKey(x => x.Id);
                b.Property(x => x.MatchType).IsRequired().HasMaxLength(32);
                b.Property(x => x.Area).HasMaxLength(64);
                b.Property(x => x.Controller).HasMaxLength(128);
                b.Property(x => x.Action).HasMaxLength(128);
                b.Property(x => x.Page).HasMaxLength(256);
                b.Property(x => x.Path).HasMaxLength(512);
                b.Property(x => x.Regex).HasMaxLength(512);
                b.Property(x => x.Notes).HasMaxLength(512);
                b.Property(x => x.IsEnabled).HasDefaultValue(true);
                b.Property(x => x.Priority).HasDefaultValue(100);
                b.Property(x => x.UpdatedAt);
                // optional: speed common lookups
                b.HasIndex(x => new { x.MatchType, x.Area, x.Controller, x.Action, x.Page, x.Path, x.Regex, x.IsEnabled });
            });

            modelBuilder.Entity<EndpointPolicyAssignment>(b =>
            {
                b.ToTable("VC_endpoint_policy_assignments", "dbo");
                b.HasKey(x => x.Id);

                b.Property(x => x.MatchType).IsRequired().HasMaxLength(32);
                b.Property(x => x.Area).HasMaxLength(64);
                b.Property(x => x.Controller).HasMaxLength(128);
                b.Property(x => x.Action).HasMaxLength(128);
                b.Property(x => x.Page).HasMaxLength(256);
                b.Property(x => x.Path).HasMaxLength(512);
                b.Property(x => x.Regex).HasMaxLength(512);
                b.Property(x => x.Notes).HasMaxLength(512);
                b.Property(x => x.PolicyName).IsRequired().HasMaxLength(128);
                b.Property(x => x.IsEnabled).HasDefaultValue(true);
                b.Property(x => x.Priority).HasDefaultValue(100);
                b.Property(x => x.UpdatedAt);

                b.HasOne(x => x.Policy)
                    .WithMany()
                    .HasForeignKey(x => x.PolicyId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(x => new { x.MatchType, x.Area, x.Controller, x.Action, x.Page, x.Path, x.Regex, x.PolicyId, x.IsEnabled });
            });
        }
    }
}
