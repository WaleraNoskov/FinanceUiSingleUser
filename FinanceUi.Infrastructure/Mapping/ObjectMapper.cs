using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceUi.Core.Contracts;
using FinanceUi.Core.Services;

namespace FinanceUi.Infrastructure.Mapping;

public class ObjectMapper : IObjectMapper
{
	private readonly Dictionary<(Type source, Type destination), IMapper> mappers = new();

	public void Register<TSource, TDestination>(Func<TSource, TDestination> mapFunc)
	{
		var key = (typeof(TSource), typeof(TDestination));
		mappers[key] = new Mapper<TSource, TDestination>(mapFunc);
	}

	public TDestination Map<TSource, TDestination>(TSource source)
	{
		var key = (typeof(TSource), typeof(TDestination));

		if (!mappers.TryGetValue(key, out var mapper))
			throw new InvalidOperationException($"No mapper registered for {key}");

		return (TDestination)mapper.Map(source!);
	}
}

