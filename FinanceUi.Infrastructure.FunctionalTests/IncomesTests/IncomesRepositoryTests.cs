using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Entities;
using FinanceUi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.FunctionalTests.IncomesTests;

public class IncomeRepositoryTests
{
    private AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldCreateIncome()
    {
        await using var context = GetContext();
        var board = new Board { Title = "Main Board" };
        context.Boards.Add(board);
        await context.SaveChangesAsync();

        var repo = new IncomeRepository(context);
        var dto = new BriefIncomeDto
        {
            Name = "Salary",
            Amount = 1000,
            Date = new DateOnly(2025, 7, 1),
            Periodicity = Periodicity.Monthly,
            BoardId = board.Id
        };

        var id = await repo.CreateAsync(dto);
        var created = await context.Incomes.FindAsync(id);

        Assert.NotNull(created);
        Assert.Equal("Salary", created!.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredSortedAndPaginated()
    {
        await using var context = GetContext();
        var board = new Board { Title = "Board" };
        context.Boards.Add(board);
        await context.SaveChangesAsync();

        var incomes = new List<Income>
        {
            new() { Name = "Salary", Amount = 1000, Date = new DateOnly(2025, 7, 1), Periodicity = Periodicity.Monthly, BoardId = board.Id },
            new() { Name = "Bonus", Amount = 500, Date = new DateOnly(2025, 7, 2), Periodicity = Periodicity.Once, BoardId = board.Id },
            new() { Name = "Freelance", Amount = 700, Date = new DateOnly(2025, 7, 3), Periodicity = Periodicity.Monthly, BoardId = board.Id }
        };
        context.Incomes.AddRange(incomes);
        await context.SaveChangesAsync();

        var repo = new IncomeRepository(context);
        var dto = new GetAllIncomesDto
        {
            Filter = "a",
            SortingParams = new SortingParams { PropertyName = "Name", IsDescending = false },
            PaginationParams = new PaginationParams { Page = 1, PageSize = 2 }
        };

        var result = await repo.GetAllAsync(dto);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(2, result.TotalCount);
        Assert.Contains(result.Items, i => i.Name == "Salary");
        Assert.Contains(result.Items, i => i.Name == "Freelance");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDto()
    {
        await using var context = GetContext();
        var board = new Board { Title = "Board" };
        var income = new Income
        {
            Name = "Dividend",
            Amount = 200,
            Date = new DateOnly(2025, 7, 5),
            Periodicity = Periodicity.Once,
            Board = board
        };

        context.Add(income);
        await context.SaveChangesAsync();

        var repo = new IncomeRepository(context);
        var dto = await repo.GetByIdAsync(income.Id);

        Assert.NotNull(dto);
        Assert.Equal("Dividend", dto!.Name);
        Assert.Equal("Board", dto.BoardTitle);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateIncome()
    {
        await using var context = GetContext();
        var board1 = new Board { Title = "Board1" };
        var board2 = new Board { Title = "Board2" };
        context.AddRange(board1, board2);

        var income = new Income
        {
            Name = "Old",
            Amount = 100,
            Date = new DateOnly(2025, 7, 1),
            Periodicity = Periodicity.Monthly,
            Board = board1
        };

        context.Add(income);
        await context.SaveChangesAsync();

        var repo = new IncomeRepository(context);
        var result = await repo.UpdateAsync(new BriefIncomeDto
        {
            Id = income.Id,
            Name = "Updated",
            Amount = 300,
            Date = new DateOnly(2025, 7, 10),
            Periodicity = Periodicity.Once,
            BoardId = board2.Id
        });

        Assert.True(result);

        var updated = await context.Incomes.FindAsync(income.Id);
        Assert.Equal("Updated", updated!.Name);
        Assert.Equal(300, updated.Amount);
        Assert.Equal(board2.Id, updated.BoardId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveIncome()
    {
        await using var context = GetContext();
        var income = new Income
        {
            Name = "ToDelete",
            Amount = 150,
            Date = new DateOnly(2025, 7, 6),
            Periodicity = Periodicity.Once,
            Board = new Board { Title = "Board" }
        };
        context.Add(income);
        await context.SaveChangesAsync();

        var repo = new IncomeRepository(context);
        await repo.DeleteAsync(income.Id);

        var found = await context.Incomes.FindAsync(income.Id);
        Assert.Null(found);
    }
}
