using FinanceUi.Application.Services;
using FinanceUi.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceUi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IBoardService, BoardsService>()
            .AddScoped<IPaymentService, PaymentsService>()
            .AddScoped<IGoalService, GoalsService>()
            .AddScoped<IIncomeService, IncomesService>();
    }
}