using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Services;
using FinanceUi.WinUi.Feature.BoardsManagement;
using Moq;

namespace FinanceUi.WinUi.Tests;

public class BoardsManagementModelTests
{
    private readonly Mock<IBoardService> _boardServiceMock = new();

    private BoardsManagementModel CreateModel() =>
        new BoardsManagementModel(_boardServiceMock.Object);

    [Fact]
    public async Task RestoreAsync_LoadsBoardsIntoCollection()
    {
        // Arrange
        var boards = new List<BoardDto>
        {
            new() { Id = Guid.NewGuid(), Title = "Board 1" },
            new() { Id = Guid.NewGuid(), Title = "Board 2" }
        };

        _boardServiceMock
            .Setup(s => s.GetAllAsync(It.IsAny<GetAllBoardsDto>()))
            .ReturnsAsync(new PaginationResult<BoardDto>(boards, boards.Count));

        var model = CreateModel();

        // Act
        await model.RestoreAsync();

        // Assert
        Assert.Equal(2, model.Boards.Count);
        Assert.Contains(model.Boards, b => b.Title == "Board 1");
    }

    [Fact]
    public async Task CreateBoard_CallsService_AndReloadsBoards()
    {
        // Arrange
        var board = new BriefBoardDto { Title = "New Board" };
        var createdId = Guid.NewGuid();
        var model = CreateModel();

        _boardServiceMock
            .Setup(s => s.CreateBoard(board))
            .ReturnsAsync(createdId);

        _boardServiceMock
            .Setup(s => s.GetAllAsync(It.IsAny<GetAllBoardsDto>()))
            .ReturnsAsync(new PaginationResult<BoardDto>(new List<BoardDto>(), 0));

        // Act
        await model.CreateBoard(board);

        // Assert
        _boardServiceMock.Verify(s => s.CreateBoard(board), Times.Once);
        _boardServiceMock.Verify(s => s.GetAllAsync(It.IsAny<GetAllBoardsDto>()), Times.Once);
    }

    [Fact]
    public async Task IsLoading_IsSetDuringRestore()
    {
        // Arrange
        var tcs = new TaskCompletionSource<PaginationResult<BoardDto>>();

        _boardServiceMock
            .Setup(s => s.GetAllAsync(It.IsAny<GetAllBoardsDto>()))
            .Returns(tcs.Task); // эмулируем "долгий запрос"

        var model = CreateModel();

        // Act
        var task = model.RestoreAsync();

        // Assert (пока не завершилось)
        Assert.True(model.IsLoading);

        // Завершаем задачу
        tcs.SetResult(new PaginationResult<BoardDto>([], 0));
        await task;

        // Assert
        Assert.False(model.IsLoading);
    }

    [Fact]
    public async Task DeleteBoard_DeletesAndReloads()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = CreateModel();

        _boardServiceMock
            .Setup(s => s.DeleteBoard(id))
            .Returns(Task.CompletedTask);

        _boardServiceMock
            .Setup(s => s.GetAllAsync(It.IsAny<GetAllBoardsDto>()))
            .ReturnsAsync(new PaginationResult<BoardDto>([], 0));

        // Act
        await model.DeleteBoardAsync(id);

        // Assert
        _boardServiceMock.Verify(s => s.DeleteBoard(id), Times.Once);
        _boardServiceMock.Verify(s => s.GetAllAsync(It.IsAny<GetAllBoardsDto>()), Times.Once);
    }

    [Fact]
    public async Task RestoreAsync_HandlesServiceFailure()
    {
        // Arrange
        _boardServiceMock
            .Setup(s => s.GetAllAsync(It.IsAny<GetAllBoardsDto>()))
            .ThrowsAsync(new Exception("Connection error"));

        var model = CreateModel();

        // Act & Assert: не должно выбрасывать исключение наружу
        var ex = await Record.ExceptionAsync(() => model.RestoreAsync());
        Assert.Null(ex);
        Assert.Empty(model.Boards);
    }
}