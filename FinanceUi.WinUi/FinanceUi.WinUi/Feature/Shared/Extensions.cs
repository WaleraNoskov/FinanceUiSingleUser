using FinanceUi.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.WinUi.Feature.Shared;

internal static class Extensions
{
    public static bool IsInPeriod(this DateOnly dateOnly, Period period) => dateOnly >= period.StartDate && dateOnly <= period.EndDate;

    public static DateOnly StartOfMonth(this DateOnly dateOnly) => new DateOnly(dateOnly.Year, dateOnly.Month, 1);

    public static DateOnly EndOfMonth(this DateOnly dateOnly) => new DateOnly(dateOnly.Year, dateOnly.Month, DateTime.DaysInMonth(dateOnly.Year, dateOnly.Month));
}