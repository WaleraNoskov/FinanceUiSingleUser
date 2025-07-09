using FinanceUi.Core.Repositories;
using FinanceUi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceUi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IBoardRepository, BoardRepository>()
            .AddScoped<IIncomeRepository, IncomeRepository>()
            .AddScoped<IGoalRepository, GoalRepository>()
            .AddScoped<IPaymentRepository, PaymentRepository>();
    }
    
    public static IServiceCollection AddAppSqlite(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'Default' is not configured.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        return services;
    }
}