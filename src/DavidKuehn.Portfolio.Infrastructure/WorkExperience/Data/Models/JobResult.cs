namespace DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models
{
    /// <summary>
    /// Models the result of a job query from the database, including job details and associated skills.
    /// </summary>
    internal record JobResult
    {
        public Guid Id { get; init; }
        public int StartYear { get; init; }
        public int EndYear { get; init; }
        public string Company { get; init; } = string.Empty;
        public string CurrentCompanyName { get; init; } = string.Empty;
        public Int16 Type { get; init; }
        public Guid? TitleId { get; init; }
        public string? Title { get; init; }
        public int? TitleStartYear { get; init; }
        public int? TitleEndYear { get; init; }
        public string? Details { get; init; }
        public string? BulletPoints { get; init; }
        public Guid? SkillId { get; init; }
        public string? SkillName { get; init; }
        public Int16? SkillType { get; init; }
    }
}
