namespace DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models;

internal record Job
{
    public required Guid Id { get; set; }
    public required int StartYear { get; set; }
    public required int EndYear { get; set; }
    public required string Company { get; set; }
    public required string CurrentCompanyName { get; set; }
    public required short Type { get; set; }
}
