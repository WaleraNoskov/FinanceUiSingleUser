using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceUi.Core.Contracts;

namespace FinanceUi.Infrastructure.Mapping;

internal class Mapper<TSource, TDestination> : IMapper
{
	private readonly Func<TSource, TDestination> mapFunc;

	public Mapper(Func<TSource, TDestination> mapFunc)
	{
		this.mapFunc = mapFunc;
	}

	public object Map(object source) => mapFunc((TSource)source);
	public Type SourceType => typeof(TSource);
	public Type DestinationType => typeof(TDestination);
}

