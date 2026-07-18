namespace DavidKuehn.Portfolio.UseCases.WorkExperience.Queries
{
    public class JobByIdQuery : IQuery
    {
        public required Guid JobId { get; init; }
    }
}
