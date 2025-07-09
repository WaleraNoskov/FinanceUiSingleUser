using FinanceUi.Core.Entities.Base;
using FinanceUi.Core.Repositories;

namespace FinanceUi.Core.Contracts;

public class PaginationResult<T>(ICollection<T> items, int totalCount)
{
    public ICollection<T> Items { get; } = items;
    public int TotalCount { get; } = totalCount;
}