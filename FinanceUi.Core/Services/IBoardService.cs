using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Board;

namespace FinanceUi.Core.Services;

public interface IBoardService
{
    Task<PaginationResult<BoardDto>> GetAllAsync(GetAllBoardsDto dto);
    Task<BoardDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateBoard(BriefBoardDto dto);
    Task<bool> UpdateBoard(BriefBoardDto dto);
    Task DeleteBoard(Guid id);
}