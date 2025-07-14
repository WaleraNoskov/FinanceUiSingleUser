using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.Core.Contracts;

public interface IMapper
{
	object Map(object source);
	Type SourceType { get; }
	Type DestinationType { get; }
}

