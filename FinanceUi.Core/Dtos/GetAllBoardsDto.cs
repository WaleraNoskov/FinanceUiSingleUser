using FinanceUi.Core.Contracts;

namespace FinanceUi.Core.Dtos;

public class GetAllBoardsDto
{
    public PaginationParams PaginationParams { get; set; }
    public SortingParams SortingParams { get; set; }
    public string? Filter { get; set; }
}