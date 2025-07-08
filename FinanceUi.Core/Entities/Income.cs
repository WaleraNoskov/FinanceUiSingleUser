using FinanceUi.Core.Contracts;
using FinanceUi.Core.Entities.Base;

namespace FinanceUi.Core.Entities;

public class Income :  BaseEntity
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public Periodicity Periodicity { get; set; }
    
    public Guid BoardId { get; set; }
    public Board? Board { get; set; }
}