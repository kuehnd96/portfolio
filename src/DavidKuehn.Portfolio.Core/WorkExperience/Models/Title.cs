namespace DavidKuehn.Portfolio.Core.WorkExperience.Models
{
    /// <summary>
    /// Model a title for a job.
    /// </summary>
    public record Title
    {
        public required Guid Id { get; set; }
        public required string TitleName { get; set; }
        public required int StartYear { get; set; }
        public required int EndYear { get; set; }
        public required string Details { get; set; }
        public required string BulletPoints { get; set; }
    }
}
