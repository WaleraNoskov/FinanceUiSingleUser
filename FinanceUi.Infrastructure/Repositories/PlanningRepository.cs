using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Dtos.Planning;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Repositories;
using FinanceUi.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.Infrastructure.Repositories;

public class PlanningRepository(AppDbContext dbContext, IObjectMapper mapper) : IPlanningRepository
{
    public async Task<Column> GetColumn(GetColumnDto dto)
    {
        var incomesQuery = dbContext.Incomes
            .Where(i => i.BoardId == dto.BoardId
                    && i.Date >= dto.Period.StartDate
                    && i.Date <= dto.Period.EndDate);
        var incomes = await incomesQuery.Select(i => mapper.Map<Income, IncomeDto>(i)).ToListAsync();
        var totalIncome = await incomesQuery.SumAsync(i => i.Amount);

        var gloabalPaymentsQuery = dbContext.Payments
            .Where(p => p.BoardId == dto.BoardId
                    && p.Date >= dto.Period.StartDate
                    &&  p.Date <= dto.Period.EndDate);

        var paymentsQuery = gloabalPaymentsQuery
            .Where(p => p.GoalId == null);
        var payments = paymentsQuery.Select(p => mapper.Map<Payment, PaymentDto>(p)).ToListAsync();
        var totalPayment = await paymentsQuery.SumAsync(i => i.Amount);

        var savingsQuery = gloabalPaymentsQuery
            .Where(p => p.GoalId != null);
        var totalSaving = await savingsQuery.SumAsync(s =>  s.Amount);
        var savings = await savingsQuery.Select(s => mapper.Map<Payment, PaymentDto>(s)).ToListAsync();

        var column = new Column
        {
            Period = dto.Period,
            Incomes = incomes,
            TotalIncome = totalIncome,
            Payments = await payments,
            TotalPayment = totalPayment,
            Savings = savings,
            RestAfterPayments = totalIncome - totalPayment,
            TotalSaving = totalSaving,
            RestAfterSavings = totalIncome - totalPayment - totalSaving
        };
        return column;
    }

    public async Task<ICollection<Column>> GetColumns(GetColumnsDto dto)
    {
        var allIncomes = await dbContext.Incomes
            .Where(i => i.BoardId == dto.BoardId
                     && i.Date >= dto.Period.StartDate
                     && i.Date <= dto.Period.EndDate)
            .ToListAsync();

        var allPayments = await dbContext.Payments
            .Where(p => p.BoardId == dto.BoardId
                     && p.Date >= dto.Period.StartDate
                     && p.Date <= dto.Period.EndDate)
            .ToListAsync();

        var columns = new List<Column>();
        var start = dto.Period.StartDate;
        var end = dto.Period.EndDate;

        while (start <= end)
        {
            var subPeriod = new Period
            {
                StartDate = start,
                EndDate = start.AddDays(dto.EachColumnDaySpan - 1)
            };

            if (subPeriod.EndDate > end)
                subPeriod.EndDate = end;

            var incomes = allIncomes
                .Where(i => i.Date >= subPeriod.StartDate && i.Date <= subPeriod.EndDate)
                .ToList();
            var paymentsInPeriod = allPayments
                .Where(p => p.Date >= subPeriod.StartDate && p.Date <= subPeriod.EndDate)
                .ToList();

            var totalIncome = incomes.Sum(i => i.Amount);
            var payments = paymentsInPeriod.Where(p => p.GoalId == null).ToList();
            var savings = paymentsInPeriod.Where(p => p.GoalId != null).ToList();
            var totalPayment = payments.Sum(p => p.Amount);
            var totalSaving = savings.Sum(p => p.Amount);

            var column = new Column
            {
                Period = subPeriod,
                Incomes = incomes.Select(mapper.Map<Income, IncomeDto>).ToList(),
                TotalIncome = totalIncome,
                Payments = payments.Select(mapper.Map<Payment, PaymentDto>).ToList(),
                TotalPayment = totalPayment,
                Savings = savings.Select(mapper.Map<Payment, PaymentDto>).ToList(),
                TotalSaving = totalSaving,
                RestAfterPayments = totalIncome - totalPayment,
                RestAfterSavings = totalIncome - totalPayment - totalSaving
            };

            columns.Add(column);
            start = subPeriod.EndDate.AddDays(1);
        }

        return columns;
    }
}
