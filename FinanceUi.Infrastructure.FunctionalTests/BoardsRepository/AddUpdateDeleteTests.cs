using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;
using FinanceUi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.FunctionalTests.BoardsRepository;

public class AddUpdateDeleteTests
{
    private AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
    
    [Fact]
    public async Task CreateBoard_CreatesBoardSuccessfully()
    {
        var context = GetContext();
        var service = new BoardRepository(context);

        var dto = new BriefBoardDto { Title = "New Board" };
        var id = await service.CreateBoard(dto);

        var board = await context.Boards.FindAsync(id);
        Assert.NotNull(board);
        Assert.Equal("New Board", board!.Title);
    }

    [Fact]
    public async Task UpdateBoard_UpdatesTitle()
    {
        var context = GetContext();
        var board = new Board { Title = "Old Title" };
        context.Boards.Add(board);
        await context.SaveChangesAsync();

        var service = new BoardRepository(context);
        await service.UpdateBoard(new BriefBoardDto { Id = board.Id, Title = "Updated Title" });

        var updated = await context.Boards.FindAsync(board.Id);
        Assert.Equal("Updated Title", updated!.Title);
    }

    [Fact]
    public async Task DeleteBoard_RemovesBoard()
    {
        var context = GetContext();
        var board = new Board { Title = "To Delete" };
        context.Boards.Add(board);
        await context.SaveChangesAsync();

        var service = new BoardRepository(context);
        await service.DeleteBoard(board.Id);

        var deleted = await context.Boards.FindAsync(board.Id);
        Assert.Null(deleted);
    }
}