using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FinanceUi.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Укажи путь к appsettings.json
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

		var rawConnectionString = configuration["ConnectionStrings:Default"];
		var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		var resolvedConnectionString = rawConnectionString.Replace("%LOCALAPPDATA%", localAppData);

		if (resolvedConnectionString == null)
            throw new NullReferenceException();

		Console.WriteLine($"aboba {resolvedConnectionString}");
		optionsBuilder.UseSqlite(resolvedConnectionString); // Или другой провайдер

        return new AppDbContext(optionsBuilder.Options);
    }
}