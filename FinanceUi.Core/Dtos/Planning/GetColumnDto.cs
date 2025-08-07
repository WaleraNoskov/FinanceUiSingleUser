using FinanceUi.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.Core.Dtos.Planning;

public class GetColumnDto
{
    public Guid BoardId { get; set; }
    public Period Period { get; set; }
}
