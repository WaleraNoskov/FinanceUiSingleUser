using FinanceUi.Core.Contracts;

namespace FinanceUi.Core.Dtos;

public class GetAllGoalsDto
{
    public PaginationParams PaginationParams { get; set; }
    public SortingParams SortingParams { get; set; }
    public string? Title { get; set; }
    public Guid BoardId { get; set; }
}