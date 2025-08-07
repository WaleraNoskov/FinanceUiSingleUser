using FinanceUi.Core.Contracts;

namespace FinanceUi.Core.Dtos;

public class PaymentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public Periodicity Periodicity { get; set; }

    public Guid BoardId { get; set; }
    public string BoardTitle { get; set; }

    public Guid? GoalId { get; set; }
    public string?  GoalTitle { get; set; }
}