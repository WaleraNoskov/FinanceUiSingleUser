using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.Core.Services;

public interface IObjectMapper
{
	TDestination Map<TSource, TDestination>(TSource source);
}

