using FinanceUi.Core.Entities.Base;

namespace FinanceUi.Core.Entities;

public class Board : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public ICollection<Goal>?  Goals { get; set; }
    public ICollection<Income>? Incomes { get; set; }
    public ICollection<Payment>? Payments { get; set; }
}