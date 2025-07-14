using System.Linq.Expressions;
using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.Repositories;

public class BoardRepository(AppDbContext dbContext, IObjectMapper mapper) : IBoardRepository
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
            .Select(b => mapper.Map<Board, BoardDto>(b))
            .ToListAsync();

        var count = await filtered.CountAsync();

        return new PaginationResult<BoardDto>(items, count);
    }

    public async Task<BoardDto?> GetByIdAsync(Guid id)
    {
        var board = await dbContext.Boards.FirstOrDefaultAsync(b => b.Id == id);

        return board is not null ? mapper.Map<Board, BoardDto>(board) : null;
	}

    public async Task<Guid> CreateBoard(BriefBoardDto dto)
    {
        var board = mapper.Map<BriefBoardDto, Board>(dto);
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