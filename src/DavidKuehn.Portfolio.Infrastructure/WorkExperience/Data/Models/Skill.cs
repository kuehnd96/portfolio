namespace DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models
{
    public record Skill
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required short Type { get; set; }
    }
}