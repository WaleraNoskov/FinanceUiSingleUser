using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Repositories;

public interface IIncomeRepository
{
    Task<PaginationResult<IncomeDto>> GetAllAsync(GetAllIncomesDto dto);
    Task<IncomeDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(BriefIncomeDto income);
    Task<bool> UpdateAsync(BriefIncomeDto income);
    Task DeleteAsync(Guid id);
}