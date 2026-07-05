using DavidKuehn.Portfolio.Core.WorkExperience.Models;

namespace DavidKuehn.Portfolio.Core.WorkExperience.Interfaces
{
    /// <summary>
    /// Surface for getting work experience data from a data source.
    /// </summary>
    public interface IWorkExperienceData
    {
        /// <summary>
        /// Gets all jobs in summary format.
        /// </summary>
        /// <returns>A collection of jobs in summary format.</returns>
        Task<IEnumerable<ListJob>> GetJobList();

        /// <summary>
        /// Gets all skills.
        /// </summary>
        /// <returns>A collection of skills.</returns>
        Task<IEnumerable<Skill>> GetSkills();

        /// <summary>
        /// Gets a job by its unique identifier with all of its details.
        /// </summary>
        /// <param name="jobId">The <see cref="Guid">identifier</see> of the job.</param>
        /// <returns>A <see cref="Job"/> object containing all details of the specified job. Otherwise null.</returns>
        Task<Job> GetJob(Guid jobId);
    }
}
