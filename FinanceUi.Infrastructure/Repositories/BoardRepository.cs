using System.Linq.Expressions;
using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;
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

    public async Task<BoardDto?> GetByIdAsync(Guid id)
    {
        return await dbContext.Boards
            .Select(b => new BoardDto()
            {
                Id = b.Id,
                Title = b.Title,
                IncomesCount = b.Incomes.Count,
                PaymentsCount = b.Payments.Count,
                GoalsCount = b.Goals.Count
            })
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Guid> CreateBoard(BriefBoardDto dto)
    {
        var board = new Board { Title = dto.Title };
        await dbContext.AddAsync(board);
        await dbContext.SaveChangesAsync();
        
        return board.Id;
    }

    public async Task<bool> UpdateBoard(BriefBoardDto dto)
    {
        var board = await dbContext.Boards.FindAsync(dto.Id);
        if (board is null)
            return false;

        board.Title = dto.Title;
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task DeleteBoard(Guid id)
    {
        var board = await dbContext.Boards.FindAsync(id);
        
        if(board is null)
            return;
        
        dbContext.Remove(board);
        await dbContext.SaveChangesAsync();
    }
}