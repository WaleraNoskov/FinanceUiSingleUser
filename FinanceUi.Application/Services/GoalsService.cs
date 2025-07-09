using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Goal;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;

namespace FinanceUi.Application.Services;

public class GoalsService(IGoalRepository repository) : IGoalService
{
    public async Task<PaginationResult<GoalDto>> GetAllAsync(GetAllGoalsDto dto)
    {
        return await repository.GetAllAsync(dto);
    }

    public async Task<GoalDto?> GetByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<Guid> CreateAsync(BriefGoalDto dto)
    {
        return await repository.CreateAsync(dto);
    }

    public async Task<bool> UpdateAsync(BriefGoalDto dto)
    {
        return await repository.UpdateAsync(dto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
    }
}