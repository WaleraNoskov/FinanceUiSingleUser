using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Planning;

namespace FinanceUi.Core.Services;

public interface IPlanningService
{
    Task<ICollection<Column>> GetColumns(GetColumnsDto dto);

    Task<Column> GetColumn(GetColumnDto dto);
}