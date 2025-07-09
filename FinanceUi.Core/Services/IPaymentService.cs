using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Services;

public interface IPaymentService
{
    Task<PaginationResult<PaymentDto>> GetAllAsync(GetAllPaymentsDto dto);
    Task<PaymentDto?> GetByIdAsync(Guid paymentId);
    Task<Guid> CreateAsync(BriefPaymentDto dto);
    Task<bool> UpdateAsync(BriefPaymentDto dto);
    Task DeleteAsync(Guid id);
}