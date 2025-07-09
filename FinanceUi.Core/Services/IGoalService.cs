using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Goal;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Services;

public interface IGoalService
{
    Task<PaginationResult<Goal>> GetAllAsync(GetAllGoalsDto dto);
    Task<Goal?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Goal goal);
    Task UpdateAsync(Goal goal);
    Task DeleteAsync(Guid id);
}