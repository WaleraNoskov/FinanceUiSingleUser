using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;

namespace FinanceUi.Application.Services;

public class IncomesService(IIncomeRepository repository) : IIncomeService
{
    public async Task<PaginationResult<IncomeDto>> GetAllAsync(GetAllIncomesDto dto)
    {
        return await repository.GetAllAsync(dto);
    }

    public async Task<IncomeDto?> GetByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<Guid> CreateAsync(BriefIncomeDto income)
    {
        return await repository.CreateAsync(income);
    }

    public async Task<bool> UpdateAsync(BriefIncomeDto income)
    {
        return await repository.UpdateAsync(income);
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
    }
}