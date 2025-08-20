namespace FinanceUi.Core.Contracts;

public class Period
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public Period(DateOnly startDate, DateOnly endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
}