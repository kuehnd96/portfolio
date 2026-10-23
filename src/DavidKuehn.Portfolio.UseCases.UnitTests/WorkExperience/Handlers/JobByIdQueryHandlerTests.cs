using DavidKuehn.Portfolio.Core.WorkExperience.Enums;
using DavidKuehn.Portfolio.Core.WorkExperience.Interfaces;
using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.UseCases.General.Enums;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Handlers;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Queries;

namespace DavidKuehn.Portfolio.UseCases.UnitTests.WorkExperience.Handlers;

public class JobByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenQueryIsNull_ThrowsArgumentNullException()
    {
        var handler = new JobByIdQueryHandler(new StubWorkExperienceData());

        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.HandleAsync(null!));
    }

    [Fact]
    public async Task HandleAsync_WhenJobIdIsEmpty_ThrowsArgumentException()
    {
        var handler = new JobByIdQueryHandler(new StubWorkExperienceData());

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(new JobByIdQuery(Guid.Empty)));

        Assert.Equal("JobId", exception.ParamName);
        Assert.Equal("JobId cannot be empty. (Parameter 'JobId')", exception.Message);
    }

    [Fact]
    public async Task HandleAsync_WhenJobIsFound_ReturnsOkResultWithJob()
    {
        var jobId = Guid.NewGuid();
        var job = CreateJob(jobId);
        var data = new StubWorkExperienceData { JobToReturn = job };
        var handler = new JobByIdQueryHandler(data);

        var result = await handler.HandleAsync(new JobByIdQuery(jobId));

        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Same(job, result.Value);
        Assert.Null(result.ErrorMessage);
        Assert.Equal(jobId, data.LastRequestedJobId);
    }

    [Fact]
    public async Task HandleAsync_WhenJobIsMissing_ReturnsNotFoundResult()
    {
        var jobId = Guid.NewGuid();
        var handler = new JobByIdQueryHandler(new StubWorkExperienceData());

        var result = await handler.HandleAsync(new JobByIdQuery(jobId));

        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Equal($"Job with ID {jobId} not found.", result.ErrorMessage);
    }

    [Fact]
    public async Task HandleAsync_WhenDataAccessThrows_ReturnsErrorResult()
    {
        var jobId = Guid.NewGuid();
        var handler = new JobByIdQueryHandler(new StubWorkExperienceData
        {
            ExceptionToThrow = new InvalidOperationException("database failure")
        });

        var result = await handler.HandleAsync(new JobByIdQuery(jobId));

        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Equal("An error occurred while retrieving the job: database failure", result.ErrorMessage);
    }

    private static Job CreateJob(Guid jobId) => new()
    {
        Id = jobId,
        StartYear = 2020,
        EndYear = 2024,
        Company = "Example Company",
        CurrentCompanyName = "Example Company",
        Type = JobType.Office
    };

    private sealed class StubWorkExperienceData : IWorkExperienceData
    {
        public Guid? LastRequestedJobId { get; private set; }

        public Job? JobToReturn { get; set; }

        public Exception? ExceptionToThrow { get; set; }

        public Task<IEnumerable<ListJob>> GetJobList() => throw new NotImplementedException();

        public Task<IEnumerable<Skill>> GetSkills() => throw new NotImplementedException();

        public Task<Job?> GetJob(Guid jobId)
        {
            LastRequestedJobId = jobId;

            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }

            return Task.FromResult(JobToReturn);
        }
    }
}