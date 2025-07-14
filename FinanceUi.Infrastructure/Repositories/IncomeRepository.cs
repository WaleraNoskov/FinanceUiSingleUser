using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.Repositories;

public class IncomeRepository(AppDbContext dbContext, IObjectMapper mapper) : IIncomeRepository
{
    public async Task<PaginationResult<IncomeDto>> GetAllAsync(GetAllIncomesDto dto)
    {
        var query = dbContext.Incomes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(dto.Filter))
        {
            query = query.Where(i => i.Name.ToLower().Contains(dto.Filter.ToLower()));
        }

        var sorted = query.OrderByDynamic(dto.SortingParams.PropertyName, dto.SortingParams.IsDescending);

        var paginated = sorted
            .Skip((dto.PaginationParams.Page - 1) * dto.PaginationParams.PageSize)
            .Take(dto.PaginationParams.PageSize);

        var items = await paginated
            .Select(i => mapper.Map<Income, IncomeDto>(i))
            .ToListAsync();

        var count = await query.CountAsync();

        return new PaginationResult<IncomeDto>(items, count);
    }

    public async Task<IncomeDto?> GetByIdAsync(Guid id)
    {
        var income = await dbContext.Incomes.FirstOrDefaultAsync(i => i.Id == id);
        return income is not null ? mapper.Map<Income, IncomeDto>(income) : null;
	}

    public async Task<Guid> CreateAsync(BriefIncomeDto dto)
    {
        var income = mapper.Map<BriefIncomeDto, Income>(dto);

        await dbContext.AddAsync(income);
        await dbContext.SaveChangesAsync();

        return income.Id;
    }

    public async Task<bool> UpdateAsync(BriefIncomeDto dto)
    {
        var income = await dbContext.Incomes.FindAsync(dto.Id);
        if (income is null)
            return false;

        income.Name = dto.Name;
        income.Amount = dto.Amount;
        income.Date = dto.Date;
        income.Periodicity = dto.Periodicity;
        income.BoardId = dto.BoardId;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(Guid id)
    {
        var income = await dbContext.Incomes.FindAsync(id);
        if (income is null)
            return;

        dbContext.Remove(income);
        await dbContext.SaveChangesAsync();
    }
}