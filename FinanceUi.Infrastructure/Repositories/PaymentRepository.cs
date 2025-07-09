using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext dbContext;

    public PaymentRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<PaginationResult<PaymentDto>> GetAllAsync(GetAllPaymentsDto dto)
    {
        var query = dbContext.Payments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(dto.Filter))
        {
            query = query.Where(p => p.Name.ToLower().Contains(dto.Filter.ToLower()));
        }

        var sorted = query.OrderByDynamic(dto.SortingParams.PropertyName, dto.SortingParams.IsDescending);

        var paginated = sorted
            .Skip((dto.PaginationParams.Page - 1) * dto.PaginationParams.PageSize)
            .Take(dto.PaginationParams.PageSize);

        var items = await paginated
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                Name = p.Name,
                Amount = p.Amount,
                Periodicity = p.Periodicity,
                BoardId = p.BoardId,
                BoardTitle = p.Board.Title,
                GoalId = p.GoalId,
                GoalTitle = p.Goal != null ? p.Goal.Title : null
            })
            .ToListAsync();

        var count = await query.CountAsync();

        return new PaginationResult<PaymentDto>(items, count);
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid id)
    {
        return await dbContext.Payments
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                Name = p.Name,
                Amount = p.Amount,
                Periodicity = p.Periodicity,
                BoardId = p.BoardId,
                BoardTitle = p.Board.Title,
                GoalId = p.GoalId,
                GoalTitle = p.Goal != null ? p.Goal.Title : null
            })
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Guid> CreateAsync(BriefPaymentDto dto)
    {
        var payment = new Payment
        {
            Name = dto.Name,
            Amount = dto.Amount,
            Periodicity = dto.Periodicity,
            BoardId = dto.BoardId,
            GoalId = dto.GoalId
        };

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