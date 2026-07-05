using DavidKuehn.Portfolio.Core.WorkExperience.Enums;

namespace DavidKuehn.Portfolio.Core.WorkExperience.Models
{
    /// <summary>
    /// Represents a row returned from the GetJob stored procedure.
    /// </summary>
    public record Job
    {
        public Guid Id { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public required string Company { get; set; }
        public required string CurrentCompanyName { get; set; }
        public byte Type { get; set; }

        // Title
        public IEnumerable<Title> Titles { get; set; } = Array.Empty<Title>();

        // Skill
        public IEnumerable<Skill> Skills { get; set; } = Array.Empty<Skill>();
    }
}
