using DavidKuehn.Portfolio.Core.WorkExperience.Interfaces;
using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.UseCases.General;
using DavidKuehn.Portfolio.UseCases.General.Enums;
using DavidKuehn.Portfolio.UseCases.General.Interfaces;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Queries;

namespace DavidKuehn.Portfolio.UseCases.WorkExperience.Handlers
{
    public class JobByIdQueryHandler : IQueryHandler<JobByIdQuery, Job>
    {
        private readonly IWorkExperienceData _workExperienceData;

        public JobByIdQueryHandler(IWorkExperienceData workExperienceData)
        {
            _workExperienceData = workExperienceData;
        }

        public async Task<IResult<Job>> HandleAsync(JobByIdQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (query.JobId == Guid.Empty)
            {
                throw new ArgumentException("JobId cannot be empty.", nameof(query.JobId));
            }

            var job = await _workExperienceData.GetJob(query.JobId);

            if (job == null)
            {
                return new Result<Job>(ResultStatus.NotFound, $"Job with ID {query.JobId} not found.");
            }
            return new Result<Job>(job);
        }
    }
}
