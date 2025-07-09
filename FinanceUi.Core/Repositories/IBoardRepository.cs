using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Repositories;

public interface IBoardRepository
{
    Task<PaginationResult<BoardDto>> GetAllAsync(GetAllBoardsDto dto);
    Task<BoardDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateBoard(BriefBoardDto dto);
    Task<bool> UpdateBoard(BriefBoardDto dto);
    Task DeleteBoard(Guid id);
}