using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MinimalApis.DataDbContext;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=MinimalApis2;User ID=sa;Password=123;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True;");

        // Use a null logger (since EF tools don’t use it)
        var logger = LoggerFactory.Create(builder => { }).CreateLogger<AppDbContext>();

        return new AppDbContext(optionsBuilder.Options, logger);
    }
}