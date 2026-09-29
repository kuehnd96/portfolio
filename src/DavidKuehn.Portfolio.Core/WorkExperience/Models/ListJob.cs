namespace DavidKuehn.Portfolio.Core.WorkExperience.Models
{
    /// <summary>
    /// Summary of a job for listing purposes.
    /// </summary>
    public record ListJob
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public required string Company { get; set; }
        public required string CurrentCompanyName { get; set; }
        /// <summary>
        /// Gets the lastest job title for the company.
        /// </summary>
        public required string Title { get; set; }
    }
}
