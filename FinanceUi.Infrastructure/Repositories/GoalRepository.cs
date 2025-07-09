using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Dtos.Goal;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.Repositories;

public class GoalRepository(AppDbContext dbContext) : IGoalRepository
{
    public async Task<PaginationResult<GoalDto>> GetAllAsync(GetAllGoalsDto dto)
    {
        var query = dbContext.Goals.AsQueryable();

        var filtered = dto.Title != null
            ? query.Where(b => b.Title.ToLower().Contains(dto.Title.ToLower()))
            : query;

        var sorted = filtered.OrderByDynamic(dto.SortingParams.PropertyName, dto.SortingParams.IsDescending);

        var paginated = sorted
            .Skip((dto.PaginationParams.Page - 1) * dto.PaginationParams.PageSize)
            .Take(dto.PaginationParams.PageSize);

        var items = await paginated
            .Select(g => new GoalDto
            {
                Id = g.Id,
                Title = g.Title,
                Amount = g.Amount,
                PaidAmount = g.PaidAmount,
                Deadline = g.Deadline,
                BoardId = g.BoardId,
                BoardName = g.Board.Title,
                PaymentsCount = g.Payments.Count,
            })
            .ToListAsync();

        var count = await filtered.CountAsync();

        return new PaginationResult<GoalDto>(items, count);
    }

    public async Task<GoalDto?> GetByIdAsync(Guid id)
    {
        return await dbContext.Goals
            .Select(g => new GoalDto()
            {
                Id = g.Id,
                Title = g.Title,
                Amount = g.Amount,
                PaidAmount = g.PaidAmount,
                Deadline = g.Deadline,
                BoardId = g.BoardId,
                BoardName = g.Board.Title,
                PaymentsCount = g.Payments.Count,
            })
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Guid> CreateAsync(BriefGoalDto dto)
    {
        var goal = new Goal
        {
            Title = dto.Title,
            Amount = dto.Amount,
            PaidAmount = dto.PaidAmount,
            Deadline = dto.Deadline,
            BoardId = dto.BoardId,
        };
        await dbContext.AddAsync(goal);
        await dbContext.SaveChangesAsync();
        
        return goal.Id;
    }

    public async Task<bool> UpdateAsync(BriefGoalDto dto)
    {
        var goal = await dbContext.Goals.FindAsync(dto.Id);
        if (goal is null)
            return false;

        goal.Title = dto.Title;
        goal.Amount = dto.Amount;
        goal.PaidAmount = dto.PaidAmount;
        goal.Deadline = dto.Deadline;
        goal.BoardId = dto.BoardId;
        
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(Guid id)
    {
        var goal = await dbContext.Goals.FindAsync(id);
        
        if(goal is null)
            return;
        
        dbContext.Remove(goal);
        await dbContext.SaveChangesAsync();
    }
}