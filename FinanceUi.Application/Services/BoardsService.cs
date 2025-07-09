using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;

namespace FinanceUi.Application.Services;

public class BoardsService(IBoardRepository repository) : IBoardService
{
    public async Task<PaginationResult<BoardDto>> GetAllAsync(GetAllBoardsDto dto)
    {
        return await repository.GetAllAsync(dto);
    }

    public async Task<BoardDto?> GetByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<Guid> CreateBoard(BriefBoardDto dto)
    {
        return await repository.CreateBoard(dto);
    }

    public async Task<bool> UpdateBoard(BriefBoardDto dto)
    {
        return await repository.UpdateBoard(dto);
    }

    public async Task DeleteBoard(Guid id)
    {
        await repository.DeleteBoard(id);
    }
}