using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MinimalApis.DataDbContext;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private readonly IConfiguration _configuration;

    public AppDbContextFactory(IConfiguration config)
    {
        _configuration = config;
    }

    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        // Use a null logger (since EF tools don’t use it)
        var logger = LoggerFactory.Create(builder => { }).CreateLogger<AppDbContext>();

        return new AppDbContext(optionsBuilder.Options, logger);
    }
}