using FinanceUi.Core.Contracts;

namespace FinanceUi.Core.Dtos.Income;

public class BriefIncomeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public Periodicity Periodicity { get; set; }
    
    public Guid BoardId { get; set; }
}