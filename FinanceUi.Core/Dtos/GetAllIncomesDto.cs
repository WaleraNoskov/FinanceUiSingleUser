using FinanceUi.Core.Contracts;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Dtos;

public class GetAllIncomesDto
{
    public PaginationParams PaginationParams { get; set; }
    public string? Filter { get; set; }
    public Guid BoardId { get; set; }
    public Period Period { get; set; }
}