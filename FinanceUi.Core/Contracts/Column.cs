using FinanceUi.Core.Dtos;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Contracts;

public class Column
{
    public Column()
    {
        Period = new Period();
        Incomes = new List<IncomeDto>();
        Payments = new List<PaymentDto>();
        Savings = new List<PaymentDto>();
    }

    public Period Period { get; set; }
    
    public ICollection<IncomeDto> Incomes { get; set; }
    public decimal TotalIncome { get; set; }
    
    public ICollection<PaymentDto> Payments { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal RestAfterPayments { get; set; }
    
    public ICollection<PaymentDto> Savings { get; set; }
    public decimal TotalSaving { get; set; }
    public decimal RestAfterSavings { get; set; }
}