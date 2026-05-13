// -------------------------------------------------------------------
// File:    VC_IMSIdentityDbContext.cs
// Author:  N/A
// Created: N/A
// Purpose: EF Core DbContext for VC_IMS, managing application entities and Identity tables.
// Dependencies:
//   - Microsoft.EntityFrameworkCore.DbContext
//   - Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<VC_user, SwRole, int>
//   - VC_IMS.Models.VC_user, SwRole
// -------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VC_IMS.Models;

namespace VC_IMS.Data;

/// <summary>
/// Database context for the VC_IMS application, inherits from
/// <c>IdentityDbContext&lt;VC_user, SwRole, int&gt;</c> to include the Identity schema.
/// </summary>
public partial class VC_IMSIdentityDbContext : IdentityDbContext<VC_user, VC_role, int>

{
    /// <summary>
    /// Initializes a new instance of <see cref="VC_IMSIdentityDbContext"/> with the specified options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="DbContextOptions{VC_IMSIdentityDbContext}"/> used to configure the context.
    /// </param>
    public VC_IMSIdentityDbContext(DbContextOptions<VC_IMSIdentityDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet for <see cref="VC_role"/> entities.
    /// </summary>
    public virtual DbSet<VC_role> VC_role { get; set; }

    /// <summary>
    /// DbSet for <see cref="VC_user"/> entities.
    /// </summary>
    public virtual DbSet<VC_user> VC_user { get; set; }

    /// <summary>
    /// Configures the EF Core model.
    /// </summary>
    /// <param name="modelBuilder">
    /// The <see cref="ModelBuilder"/> for constructing entity mappings.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.Entity<VC_role>(entity =>
        {
            entity.ToTable("VC_roles");

            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<VC_user>(entity =>
        {
            entity.ToTable("VC_users");

            
        });

        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("VC_user_roles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("VC_user_claims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("VC_user_logins");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("VC_role_claims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("VC_user_tokens");


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
