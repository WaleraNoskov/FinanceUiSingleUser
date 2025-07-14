using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Dtos.Goal;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.Repositories;

public class GoalRepository(AppDbContext dbContext, IObjectMapper mapper) : IGoalRepository
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
            .Select(g => mapper.Map<Goal, GoalDto>(g))
            .ToListAsync();

        var count = await filtered.CountAsync();

        return new PaginationResult<GoalDto>(items, count);
    }

    public async Task<GoalDto?> GetByIdAsync(Guid id)
    {
        var goal = await dbContext.Goals.FirstOrDefaultAsync(b => b.Id == id);
        return goal is not null ? mapper.Map<Goal, GoalDto>(goal) : null;

	}

    public async Task<Guid> CreateAsync(BriefGoalDto dto)
    {
        var goal = mapper.Map<BriefGoalDto, Goal>(dto);
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