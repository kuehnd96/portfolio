using DavidKuehn.Portfolio.Core.WorkExperience.Enums;

namespace DavidKuehn.Portfolio.Core.WorkExperience.Models
{
    /// <summary>
    /// Models a skill from a job.
    /// </summary>
    public record Skill
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required SkillType Type { get; set; } = SkillType.None;
    }
}
