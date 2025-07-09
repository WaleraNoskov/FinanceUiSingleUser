namespace FinanceUi.Core.Dtos.Board;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public int IncomesCount { get; set; }
    public int PaymentsCount { get; set; }
    public int GoalsCount { get; set; }
}