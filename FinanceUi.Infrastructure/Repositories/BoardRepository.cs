using System.Linq.Expressions;
using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.Repositories;

public class BoardRepository(AppDbContext dbContext) : IBoardRepository
{
    public async Task<PaginationResult<BoardDto>> GetAllAsync(GetAllBoardsDto dto)
    {
        var query = dbContext.Boards.AsQueryable();

        var filtered = dto.Filter != null
            ? query.Where(b => b.Title.ToLower().Contains(dto.Filter.ToLower()))
            : query;

        var sorted = filtered.OrderByDynamic(dto.SortingParams.PropertyName, dto.SortingParams.IsDescending);

        var paginated = sorted
            .Skip((dto.PaginationParams.Page - 1) * dto.PaginationParams.PageSize)
            .Take(dto.PaginationParams.PageSize);

        var items = await paginated
            .Include(b => b.Goals)
            .Include(b => b.Payments)
            .Include(b => b.Incomes)
            .Select(b => new BoardDto
            {
                Id = b.Id,
                Title = b.Title,
                IncomesCount = b.Incomes.Count,
                PaymentsCount = b.Payments.Count,
                GoalsCount = b.Goals.Count
            })
            .ToListAsync();

        var count = await filtered.CountAsync();

        return new PaginationResult<BoardDto>(items, count);
    }

    public Task<BoardDto?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateBoard(BriefBoardDto dto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBoard(BriefBoardDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBoard(Guid id)
    {
        throw new NotImplementedException();
    }
}