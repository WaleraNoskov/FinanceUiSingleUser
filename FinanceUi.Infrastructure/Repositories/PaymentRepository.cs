using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;
using FinanceUi.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.Repositories;

public class PaymentRepository(AppDbContext dbContext, IObjectMapper mapper) : IPaymentRepository
{

    public async Task<PaginationResult<PaymentDto>> GetAllAsync(GetAllPaymentsDto dto)
    {
        var query = dbContext.Payments.AsQueryable();

        var filtered = query
            .Where(p => p.Date <= dto.Period.StartDate && p.Date <= dto.Period.EndDate);
        if (!string.IsNullOrEmpty(dto.Filter))
            filtered = filtered.Where(p => p.Name.ToLower().Contains(dto.Filter.ToLower()));

        if (dto.IsForGoal)
            filtered = filtered.Where(p => p.GoalId != null);
        else
            filtered = filtered.Where(p => p.GoalId == null);

        var sorted = dto.SortingParams is not null
            ? filtered.OrderByDynamic(dto.SortingParams.PropertyName, dto.SortingParams.IsDescending)
            : filtered;

        var paginated = dto.PaginationParams is not null
            ? sorted
                .Skip((dto.PaginationParams.Page - 1) * dto.PaginationParams.PageSize)
                .Take(dto.PaginationParams.PageSize)
            : sorted;

        var items = await paginated
            .Select(p => mapper.Map<Payment, PaymentDto>(p))
            .ToListAsync();

        var count = await query.CountAsync();

        return new PaginationResult<PaymentDto>(items, count);
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid id)
    {
        var payment = await dbContext.Payments.FirstOrDefaultAsync(p => p.Id == id);
        return payment is not null ? mapper.Map<Payment, PaymentDto>(payment) : null;

    }

    public async Task<Guid> CreateAsync(BriefPaymentDto dto)
    {
        var payment = mapper.Map<BriefPaymentDto, Payment>(dto);

        await dbContext.Payments.AddAsync(payment);
        await dbContext.SaveChangesAsync();

        return payment.Id;
    }

    public async Task<bool> UpdateAsync(BriefPaymentDto dto)
    {
        var payment = await dbContext.Payments.FindAsync(dto.Id);
        if (payment == null) return false;

        payment.Name = dto.Name;
        payment.Amount = dto.Amount;
        payment.Periodicity = dto.Periodicity;
        payment.BoardId = dto.BoardId;
        payment.GoalId = dto.GoalId;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(Guid id)
    {
        var payment = await dbContext.Payments.FindAsync(id);
        if (payment == null) return;

        dbContext.Payments.Remove(payment);
        await dbContext.SaveChangesAsync();
    }
}