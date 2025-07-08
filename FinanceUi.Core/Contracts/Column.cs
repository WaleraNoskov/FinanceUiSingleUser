using FinanceUi.Core.Entities;

namespace FinanceUi.Core.Contracts;

public class Column
{
    public Period Period { get; set; }
    
    public ICollection<Income> Incomes { get; set; }
    public decimal TotalIncome { get; set; }
    
    public ICollection<Payment> Payments { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal RestAfterPayments { get; set; }
    
    public ICollection<Payment> Savings { get; set; }
    public decimal RestAfterSavings { get; set; }
}