using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MinimalApis.DataDbContext;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=.;Database=MinimalApis;User ID=sa;Password=123;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}