namespace FinanceUi.Core.Dtos.Board;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public decimal PaidTotalGoal { get; set; }
    public decimal TotalGoal { get; set; }
}