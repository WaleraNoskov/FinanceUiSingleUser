using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;

namespace FinanceUi.Application.Services;

public class PaymentsService(IPaymentRepository repository) : IPaymentService
{
    public async Task<PaginationResult<PaymentDto>> GetAllAsync(GetAllPaymentsDto dto)
    {
        return await repository.GetAllAsync(dto);
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid paymentId)
    {
        return await repository.GetByIdAsync(paymentId);
    }

    public async Task<Guid> CreateAsync(BriefPaymentDto dto)
    {
        return await repository.CreateAsync(dto);
    }

    public async Task<bool> UpdateAsync(BriefPaymentDto dto)
    {
        return await repository.UpdateAsync(dto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
    }
}