using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Planning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.Core.Repositories;

public interface IPlanningRepository
{
    Task<ICollection<Column>> GetColumns(GetColumnsDto dto);
    Task<Column> GetColumn(GetColumnDto dto);
}
