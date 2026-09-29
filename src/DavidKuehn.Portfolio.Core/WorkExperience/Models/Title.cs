namespace DavidKuehn.Portfolio.Core.WorkExperience.Models
{
    /// <summary>
    /// Model a title for a job.
    /// </summary>
    public record class Title
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? Details { get; set; }
        public string? BulletPoints { get; set; }
    }
}
