using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Services;

public interface IIncomeService
{
    Task<PaginationResult<Income>> GetAllAsync(GetAllIncomesDto dto);
    Task<Income?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Income income);
    Task UpdateAsync(Income income);
    Task DeleteAsync(Guid id);
}