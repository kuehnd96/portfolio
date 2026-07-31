using DavidKuehn.Portfolio.Core.WorkExperience.Interfaces;
using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.UseCases.General;
using DavidKuehn.Portfolio.UseCases.General.Enums;
using DavidKuehn.Portfolio.UseCases.General.Interfaces;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Queries;

namespace DavidKuehn.Portfolio.UseCases.WorkExperience.Handlers
{
    /// <summary>
    /// Handles querying for a list of jobs.
    /// </summary>
    public class JobListQueryHandler : IQueryHandler<JobListQuery, IEnumerable<ListJob>>
    {
        private readonly IWorkExperienceData _workExperienceData;

        public JobListQueryHandler(IWorkExperienceData workExperienceData)
        {
            ArgumentNullException.ThrowIfNull(workExperienceData, nameof(workExperienceData));
            _workExperienceData = workExperienceData;
        }

        public async Task<IResult<IEnumerable<ListJob>>> HandleAsync(JobListQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            IEnumerable<ListJob>? jobs;

            try
            {
                jobs = await _workExperienceData.GetJobList();
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<ListJob>>(ResultStatus.Error, $"An error occurred while retrieving the jobs: {ex.Message}");
            }

            if (jobs == null || !jobs.Any())
            {
                return new Result<IEnumerable<ListJob>>(ResultStatus.NotFound, "No jobs found.");
            }

            return new Result<IEnumerable<ListJob>>(jobs);
        }
    }
}
