using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;

namespace FinanceUi.Core.Services;

public interface IBoardService
{
    Task<PaginationResult<BoardDto>> GetAllAsync(GetAllBoardsDto dto);
    Task<BoardDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateBoard(BriefBoardDto dto);
    Task UpdateBoard(BriefBoardDto dto);
    Task DeleteBoard(Guid id);
}