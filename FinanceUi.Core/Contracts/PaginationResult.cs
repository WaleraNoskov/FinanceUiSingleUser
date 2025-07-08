using FinanceUi.Core.Entities.Base;
using FinanceUi.Core.Repositories;

namespace FinanceUi.Core.Contracts;

public class PaginationResult<T>
{
    public ICollection<T> Items { get; set; } = new List<T>();
    public int Count { get; set; }
    public int TotalCount { get; set; }
}