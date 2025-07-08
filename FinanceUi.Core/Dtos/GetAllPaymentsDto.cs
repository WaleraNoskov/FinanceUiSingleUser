using FinanceUi.Core.Contracts;

namespace FinanceUi.Core.Dtos;

public class GetAllPaymentsDto
{
    public PaginationParams PaginationParams { get; set; }
    public string? Filter { get; set; }
    public Guid BoardId { get; set; }
    public Guid? GoalId { get; set; }
}