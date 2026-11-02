using DavidKuehn.Portfolio.Core.WorkExperience.Interfaces;
using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.UseCases.General.Enums;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Handlers;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Queries;

namespace DavidKuehn.Portfolio.UseCases.UnitTests.WorkExperience.Handlers;

public class JobListQueryHandlerTests
{
    [Fact]
    public void Constructor_WhenWorkExperienceDataIsNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new JobListQueryHandler(null!));

        Assert.Equal("workExperienceData", exception.ParamName);
    }

    [Fact]
    public async Task HandleAsync_WhenQueryIsNull_ThrowsArgumentNullException()
    {
        var handler = new JobListQueryHandler(new StubWorkExperienceData());

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.HandleAsync(null!));

        Assert.Equal("query", exception.ParamName);
    }

    [Fact]
    public async Task HandleAsync_WhenJobsExist_ReturnsOkResultWithJobs()
    {
        var jobs = new List<ListJob>
        {
            CreateListJob("Contoso", "Senior Engineer"),
            CreateListJob("Fabrikam", "Lead Developer")
        };
        var data = new StubWorkExperienceData { JobListToReturn = jobs };
        var handler = new JobListQueryHandler(data);

        var result = await handler.HandleAsync(new JobListQuery());

        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Same(jobs, result.Value);
        Assert.Null(result.ErrorMessage);
        Assert.Equal(1, data.GetJobListCallCount);
    }

    [Fact]
    public async Task HandleAsync_WhenNoJobsExist_ReturnsEmptyCollection()
    {
        var handler = new JobListQueryHandler(new StubWorkExperienceData
        {
            JobListToReturn = Array.Empty<ListJob>()
        });

        var result = await handler.HandleAsync(new JobListQuery());

        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task HandleAsync_WhenDataAccessThrows_ReturnsErrorResult()
    {
        var handler = new JobListQueryHandler(new StubWorkExperienceData
        {
            ExceptionToThrow = new InvalidOperationException("database failure")
        });

        var result = await handler.HandleAsync(new JobListQuery());

        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Equal("An error occurred while retrieving the jobs: database failure", result.ErrorMessage);
    }

    private static ListJob CreateListJob(string company, string title) => new()
    {
        StartYear = 2020,
        EndYear = 2024,
        Company = company,
        CurrentCompanyName = company,
        Title = title
    };

    private sealed class StubWorkExperienceData : IWorkExperienceData
    {
        public int GetJobListCallCount { get; private set; }

        public IEnumerable<ListJob>? JobListToReturn { get; set; }

        public Exception? ExceptionToThrow { get; set; }

        public Task<IEnumerable<ListJob>> GetJobList()
        {
            GetJobListCallCount++;

            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }

            return Task.FromResult(JobListToReturn ?? Array.Empty<ListJob>());
        }

        public Task<IEnumerable<Skill>> GetSkills() => throw new NotImplementedException();

        public Task<Job?> GetJob(Guid jobId) => throw new NotImplementedException();
    }
}
