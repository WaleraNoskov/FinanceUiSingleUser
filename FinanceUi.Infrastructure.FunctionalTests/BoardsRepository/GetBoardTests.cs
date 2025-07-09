using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Entities;
using FinanceUi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.FunctionalTests.BoardsRepository;

public class GetBoardTests
{
    private AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCorrectPage_WithFilterAndSorting()
    {
        // Arrange
        var context = GetContext();

        context.Boards.AddRange(
            new Board { Title = "Budget" },
            new Board { Title = "Savings" },
            new Board { Title = "Investments" }
        );
        await context.SaveChangesAsync();

        var service = new BoardRepository(context);

        var dto = new GetAllBoardsDto
        {
            Filter = "sav",
            SortingParams = new SortingParams { PropertyName = "Title", IsDescending = false },
            PaginationParams = new PaginationParams { Page = 0, PageSize = 10 }
        };

        // Act
        var result = await service.GetAllAsync(dto);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("Savings", result.Items.First().Title ?? string.Empty);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task GetAllAsync_PaginatesCorrectly()
    {
        var context = GetContext();
        for (int i = 0; i < 20; i++)
            context.Boards.Add(new Board { Title = $"Board {i}" });

        await context.SaveChangesAsync();
        var service = new BoardRepository(context);

        var dto = new GetAllBoardsDto
        {
            Filter = null,
            SortingParams = new SortingParams { PropertyName = "Title", IsDescending = false },
            PaginationParams = new PaginationParams { Page = 1, PageSize = 5 }
        };

        var result = await service.GetAllAsync(dto);

        Assert.Equal(5, result.Items.Count);
        Assert.Equal(20, result.TotalCount);
        Assert.Contains(result.Items, b => b.Title == "Board 11");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCorrectIncomesCount()
    {
        // Arrange
        var context = GetContext();

        var board = new Board { Title = "My Board" };
        context.Boards.Add(board);

        context.Incomes.AddRange(
            new Income
            {
                Name = "Income",
                Amount = 100,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Periodicity = Periodicity.Once,
                Board = board
            },
            new Income
            {
                Name = "Income",
                Amount = 50,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Periodicity = Periodicity.Once,
                Board = board
            }
        );

        await context.SaveChangesAsync();

        var service = new BoardRepository(context); // или твой сервис, который вызывает GetAllAsync

        var dto = new GetAllBoardsDto
        {
            Filter = null,
            SortingParams = new SortingParams { PropertyName = "Title", IsDescending = false },
            PaginationParams = new PaginationParams { Page = 0, PageSize = 10 }
        };

        // Act
        var result = await service.GetAllAsync(dto);

        // Assert
        var boardDto = result.Items.FirstOrDefault();
        Assert.NotNull(boardDto);
        Assert.Equal(2, boardDto.IncomesCount); // проверяем количество доходов
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBoardWithCorrectCounts()
    {
        var context = GetContext();

        var board = new Board { Title = "My Board" };
        var incomes = new[]
        {
            new Income
            {
                Name = "income",
                Board = board,
                Amount = 10,
                Date = DateOnly.FromDateTime(DateTime.Now)
            },
            new Income
            {
                Name = "income2",
                Board = board,
                Amount = 20,
                Date = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        var payments = new[]
        {
            new Payment
            {
                Name = "Payment",
                Board = board,
                Amount = 5,
                Periodicity = Periodicity.Once
            }
        };
        var goals = new[]
        {
            new Goal
            {
                Board = board,
                Title = "Save",
                Deadline = DateOnly.FromDateTime(DateTime.Now)
            }
        };

        context.Boards.Add(board);
        context.Incomes.AddRange(incomes);
        context.Payments.AddRange(payments);
        context.Goals.AddRange(goals);
        await context.SaveChangesAsync();

        var service = new BoardRepository(context);

        var result = await service.GetByIdAsync(board.Id);

        Assert.NotNull(result);
        Assert.Equal(2, result!.IncomesCount);
        Assert.Equal(1, result.PaymentsCount);
        Assert.Equal(1, result.GoalsCount);
    }
}