using FinanceUi.Core.Contracts;

namespace FinanceUi.Core.Dtos;

public class BriefPaymentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public Periodicity Periodicity { get; set; }

    public Guid BoardId { get; set; }

    public Guid? GoalId { get; set; }
}