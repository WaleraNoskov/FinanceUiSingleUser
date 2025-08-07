using FinanceUi.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.Core.Dtos.Planning;

public class GetColumnsDto
{
    public Guid BoardId { get; set; }
    public Period Period { get; set; }

    public int EachColumnDaySpan { get; set; }

    public GetColumnsDto()
    {
        Period = new Period();
        BoardId = Guid.Empty;
    }
}
