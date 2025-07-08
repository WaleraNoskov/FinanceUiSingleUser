using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Repositories;

public interface IPaymentRepository
{
    Task<PaginationResult<Payment>> GetAllAsync(GetAllPaymentsDto dto);
    Task<Payment?> GetByIdAsync(Guid paymentId);
    Task<Guid> Create(Payment payment);
    Task UpdateAsync(Payment payment);
    Task DeleteAsync(Guid id);
}