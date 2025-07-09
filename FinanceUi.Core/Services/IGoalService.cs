using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Goal;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Services;

public interface IGoalService
{
    Task<PaginationResult<GoalDto>> GetAllAsync(GetAllGoalsDto dto);
    Task<GoalDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(BriefGoalDto dto);
    Task<bool> UpdateAsync(BriefGoalDto dto);
    Task DeleteAsync(Guid id);
}