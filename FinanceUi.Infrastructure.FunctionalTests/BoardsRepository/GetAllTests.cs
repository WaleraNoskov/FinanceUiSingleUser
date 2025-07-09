using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;
using FinanceUi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.FunctionalTests.BoardsRepository;

public class GetAllTests
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
}