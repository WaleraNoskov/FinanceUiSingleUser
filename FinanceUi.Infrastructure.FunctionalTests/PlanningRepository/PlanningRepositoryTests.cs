using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Planning;
using FinanceUi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.Infrastructure.FunctionalTests.PlanningRepository;

public class PlanningRepositoryTests
{
    private AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetColumn_ShouldReturnCorrectAggregatedData()
    {
        await using var context = GetContext();
        var mapper = DependencyInjection.GetMapper(); // предполагается, что у тебя уже есть этот метод

        var board = new Board { Title = "Test Board" };
        context.Boards.Add(board);

        var period = new Period
        {
            StartDate = new DateOnly(2025, 8, 1),
            EndDate = new DateOnly(2025, 8, 31)
        };

        context.Incomes.AddRange(new[]
        {
            new Income { Name = "Job", Amount = 1000, Date = new DateOnly(2025, 8, 5), Board = board },
            new Income { Name = "Gift", Amount = 500, Date = new DateOnly(2025, 8, 15), Board = board },
            new Income { Name = "Skip", Amount = 300, Date = new DateOnly(2025, 7, 31), Board = board } // вне периода
        });

        context.Payments.AddRange(new[]
        {
            new Payment { Name = "Rent", Amount = 400, Date = new DateOnly(2025, 8, 10), Board = board }, // без цели
            new Payment { Name = "Food", Amount = 200, Date = new DateOnly(2025, 8, 12), Board = board, GoalId = null }, // без цели
            new Payment { Name = "Savings", Amount = 300, Date = new DateOnly(2025, 8, 20), Board = board, GoalId = Guid.NewGuid() },
            new Payment { Name = "Skip", Amount = 100, Date = new DateOnly(2025, 9, 1), Board = board } // вне периода
        });

        await context.SaveChangesAsync();

        var repo = new FinanceUi.Infrastructure.Repositories.PlanningRepository(context, mapper);
        var dto = new GetColumnDto
        {
            BoardId = board.Id,
            Period = period
        };

        var column = await repo.GetColumn(dto);

        Assert.Equal(period, column.Period);
        Assert.Equal(2, column.Incomes.Count);
        Assert.Equal(1500, column.TotalIncome);
        Assert.Equal(2, column.Payments.Count);
        Assert.Equal(600, column.TotalPayment);
        Assert.Equal(300, column.TotalSaving);
        Assert.Equal(900, column.RestAfterPayments);
        Assert.Equal(600, column.RestAfterSavings);
    }

    [Fact]
    public async Task GetColumns_ShouldReturnCorrectNumberOfColumns()
    {
        await using var context = GetContext();
        var mapper = DependencyInjection.GetMapper();

        var board = new Board { Title = "Test Board" };
        context.Boards.Add(board);

        var start = new DateOnly(2025, 8, 1);
        var end = new DateOnly(2025, 8, 10);

        // добавим по одной записи на каждый день (доход, расход, отложение)
        for (int i = 0; i <= 9; i++)
        {
            var date = start.AddDays(i);
            context.Incomes.Add(new Income
            {
                Name = $"Income {i}",
                Amount = 100 + i,
                Date = date,
                Board = board
            });

            context.Payments.Add(new Payment
            {
                Name = $"Payment {i}",
                Amount = 50 + i,
                Date = date,
                GoalId = null,
                Board = board
            });

            context.Payments.Add(new Payment
            {
                Name = $"Saving {i}",
                Amount = 25 + i,
                Date = date,
                GoalId = Guid.NewGuid(),
                Board = board
            });
        }

        await context.SaveChangesAsync();

        var repo = new FinanceUi.Infrastructure.Repositories.PlanningRepository(context, mapper);
        var dto = new GetColumnsDto
        {
            BoardId = board.Id,
            Period = new Period { StartDate = start, EndDate = end },
            EachColumnDaySpan = 5
        };

        var result = await repo.GetColumns(dto);

        Assert.Equal(2, result.Count);

        var first = result.ElementAt(0);
        Assert.Equal(new DateOnly(2025, 8, 1), first.Period.StartDate);
        Assert.Equal(new DateOnly(2025, 8, 5), first.Period.EndDate);

        var second = result.ElementAt(1);
        Assert.Equal(new DateOnly(2025, 8, 6), second.Period.StartDate);
        Assert.Equal(new DateOnly(2025, 8, 10), second.Period.EndDate);
    }
}
