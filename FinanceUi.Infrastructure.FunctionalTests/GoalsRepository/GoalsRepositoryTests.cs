using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Goal;
using FinanceUi.Core.Entities;
using FinanceUi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.FunctionalTests.GoalsRepository;

public class GoalsRepositoryTests
{
    private AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsFilteredSortedPaginatedGoals()
    {
        var context = GetContext();
        var board = new Board { Title = "Board A" };
        context.Boards.Add(board);

        context.Goals.AddRange(
            new Goal { Title = "Save for Car", Board = board },
            new Goal { Title = "Save for Trip", Board = board },
            new Goal { Title = "Invest", Board = board }
        );
        await context.SaveChangesAsync();

        var repo = new GoalRepository(context, DependencyInjection.GetMapper());
        var dto = new GetAllGoalsDto
        {
            Title = "save", // filter
            SortingParams = new SortingParams { PropertyName = "Title", IsDescending = false },
            PaginationParams = new PaginationParams { Page = 1, PageSize = 10 }
        };

        var result = await repo.GetAllAsync(dto);

        Assert.Equal(2, result.TotalCount); // "Save for Car" and "Save for Trip"
        Assert.All(result.Items, g => Assert.Contains("Save", g.Title, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGoalWithPaymentsCountAndBoardTitle()
    {
        var context = GetContext();
        var board = new Board { Title = "Board X" };
        var goal = new Goal { Title = "Goal X", Board = board };
        context.Boards.Add(board);
        context.Goals.Add(goal);
        context.Payments.Add(new Payment { Name = "Name", Goal = goal });
        await context.SaveChangesAsync();

        var repo = new GoalRepository(context, DependencyInjection.GetMapper());
        var result = await repo.GetByIdAsync(goal.Id);

        Assert.NotNull(result);
        Assert.Equal("Goal X", result!.Title);
        Assert.Equal("Board X", result.BoardName);
        Assert.Equal(1, result.PaymentsCount);
    }

    [Fact]
    public async Task CreateAsync_CreatesGoal()
    {
        var context = GetContext();
        var board = new Board { Title = "Main" };
        context.Boards.Add(board);
        await context.SaveChangesAsync();

        var repo = new GoalRepository(context, DependencyInjection.GetMapper());
        var dto = new BriefGoalDto
        {
            Title = "New Goal",
            Amount = 1000,
            PaidAmount = 200,
            Deadline = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
            BoardId = board.Id
        };

        var id = await repo.CreateAsync(dto);
        var goal = await context.Goals.FindAsync(id);

        Assert.NotNull(goal);
        Assert.Equal("New Goal", goal!.Title);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesGoal()
    {
        var context = GetContext();
        var board1 = new Board { Title = "Board1" };
        var board2 = new Board { Title = "Board2" };
        var goal = new Goal { Title = "Old", Amount = 100, Board = board1 };

        context.Boards.AddRange(board1, board2);
        context.Goals.Add(goal);
        await context.SaveChangesAsync();

        var repo = new GoalRepository(context, DependencyInjection.GetMapper());
        var dto = new BriefGoalDto
        {
            Id = goal.Id,
            Title = "Updated",
            Amount = 200,
            PaidAmount = 50,
            Deadline = DateOnly.FromDateTime(DateTime.Today.AddDays(10)),
            BoardId = board2.Id
        };

        var success = await repo.UpdateAsync(dto);
        var updated = await context.Goals.FindAsync(goal.Id);

        Assert.True(success);
        Assert.Equal("Updated", updated!.Title);
        Assert.Equal(200, updated.Amount);
        Assert.Equal(board2.Id, updated.BoardId);
    }

    [Fact]
    public async Task DeleteAsync_RemovesGoal()
    {
        var context = GetContext();
        var goal = new Goal { Title = "To Delete" };
        context.Goals.Add(goal);
        await context.SaveChangesAsync();

        var repo = new GoalRepository(context, DependencyInjection.GetMapper());
        await repo.DeleteAsync(goal.Id);

        var deleted = await context.Goals.FindAsync(goal.Id);
        Assert.Null(deleted);
    }

}