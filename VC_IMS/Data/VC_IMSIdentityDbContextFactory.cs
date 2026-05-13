using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VC_IMS.Data;

public sealed class VC_IMSIdentityDbContextFactory
    : IDesignTimeDbContextFactory<VC_IMSIdentityDbContext>
{
    public VC_IMSIdentityDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var cfg = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Pick the correct connection string key used by your identity db
        var cs =
            cfg.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No connection string found for VC_IMSIdentityDbContext.");

        var opts = new DbContextOptionsBuilder<VC_IMSIdentityDbContext>()
            .UseSqlServer(cs)
            .Options;

        return new VC_IMSIdentityDbContext(opts);
    }
}
