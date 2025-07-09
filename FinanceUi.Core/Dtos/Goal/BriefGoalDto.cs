namespace FinanceUi.Core.Dtos.Goal;

public class BriefGoalDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateOnly Deadline { get; set; }
    
    public Guid BoardId { get; set; }
}