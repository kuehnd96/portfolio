namespace DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models
{
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