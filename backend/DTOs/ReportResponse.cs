public class ReportResponse<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public GeneralTotalsDto General { get; set; } = new GeneralTotalsDto();
}