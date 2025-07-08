using FinanceUi.Core.Entities.Base;

namespace FinanceUi.Core.Entities;

public class Goal : BaseEntity
{
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateOnly Deadline { get; set; }
    
    public Guid BoardId { get; set; }
    public Board? Board { get; set; }
}