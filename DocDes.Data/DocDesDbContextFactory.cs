using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DocDes.Data;
public class DocDesDbContextFactory : IDesignTimeDbContextFactory<DocDesDbContext>
{
    public DocDesDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string not found.");

        var optionsBuilder = new DbContextOptionsBuilder<DocDesDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DocDesDbContext(optionsBuilder.Options);
    }
}
