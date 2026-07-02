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
    }
}
