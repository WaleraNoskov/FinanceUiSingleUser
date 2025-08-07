using FinanceUi.Core.Contracts;

namespace FinanceUi.Core.Dtos.Income;

public class GetAllIncomesDto
{
    public PaginationParams? PaginationParams { get; set; }
    public SortingParams? SortingParams { get; set; }
    public string? Filter { get; set; }
    public Guid BoardId { get; set; }
    public Period Period { get; set; }
}