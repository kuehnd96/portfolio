using DavidKuehn.Portfolio.UseCases.General.Interfaces;

namespace DavidKuehn.Portfolio.UseCases.WorkExperience.Queries
{
    /// <summary>
    /// Represents a query to retrieve a job by its unique identifier (JobId).
    /// </summary>
    public class JobByIdQuery : IQuery
    {
        /// <summary>
        /// Gets or sets the unique identifier of the job to retrieve.
        /// </summary>
        public required Guid JobId { get; init; }
    }
}
