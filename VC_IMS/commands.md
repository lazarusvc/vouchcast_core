/// Migrations commands

# Adding Migrations
# --------------------------------------
## Add-Migration "Identity_Init" -Context VC_IMSIdentityDbContext -Output Migrations/Identity
## Add-Migration "DbMore_Init" -Context VC_IMSDb_moreContext -Output Migrations/DbMore

# Updating Database
# --------------------------------------
## Update-Database -Context VC_IMSIdentityDbContext
## Update-Database -Context VC_IMSDb_moreContext
