using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Repositories;

public interface IGoalRepository
{
    Task<PaginationResult<Goal>> GetAllAsync(GetAllGoalsDto dto);
    Task<Goal?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Goal goal);
    Task UpdateAsync(Goal goal);
    Task DeleteAsync(Guid id);
}