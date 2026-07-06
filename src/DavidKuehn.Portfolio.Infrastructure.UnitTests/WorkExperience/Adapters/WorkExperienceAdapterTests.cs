using DavidKuehn.Portfolio.Core.WorkExperience.Enums;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Adapters;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models;

namespace DavidKuehn.Portfolio.Infrastructure.UnitTests.WorkExperience.Adapters;

public class WorkExperienceAdapterTests
{
    [Fact]
    public void ToJob_WhenJobResultsAreEmpty_ReturnsNull()
    {
        // Arrange
        var jobResults = Enumerable.Empty<JobResult>();

        // Act
        var result = WorkExperienceAdapter.ToJob(jobResults);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToJob_WhenJobResultsAreProvided_MapsJobTitlesAndValidSkills()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var firstTitleId = Guid.NewGuid();
        var secondTitleId = Guid.NewGuid();
        var firstSkillId = Guid.NewGuid();
        var secondSkillId = Guid.NewGuid();

        var jobResults = new[]
        {
            new JobResult
            {
                Id = jobId,
                StartYear = 2020,
                EndYear = 2024,
                Company = "Contoso",
                CurrentCompanyName = "Contoso Inc.",
                Type = (short)JobType.Hybrid,
                TitleId = firstTitleId,
                Title = "Developer",
                TitleStartYear = 2020,
                TitleEndYear = 2022,
                Details = "Built APIs",
                BulletPoints = "C#, SQL",
                SkillId = firstSkillId,
                SkillName = "C#",
                SkillType = (short)SkillType.Technical
            },
            new JobResult
            {
                Id = jobId,
                StartYear = 2020,
                EndYear = 2024,
                Company = "Contoso",
                CurrentCompanyName = "Contoso Inc.",
                Type = (short)JobType.Hybrid,
                TitleId = secondTitleId,
                Title = "Senior Developer",
                TitleStartYear = 2022,
                TitleEndYear = 2024,
                Details = "Led projects",
                BulletPoints = "Architecture, Mentoring",
                SkillId = null,
                SkillName = "ShouldBeIgnored",
                SkillType = (short)SkillType.Soft
            },
            new JobResult
            {
                Id = jobId,
                StartYear = 2020,
                EndYear = 2024,
                Company = "Contoso",
                CurrentCompanyName = "Contoso Inc.",
                Type = (short)JobType.Hybrid,
                TitleId = null,
                Title = null,
                TitleStartYear = null,
                TitleEndYear = null,
                Details = null,
                BulletPoints = null,
                SkillId = secondSkillId,
                SkillName = "Communication",
                SkillType = null
            }
        };

        // Act
        var result = WorkExperienceAdapter.ToJob(jobResults);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jobId, result.Id);
        Assert.Equal(2020, result.StartYear);
        Assert.Equal(2024, result.EndYear);
        Assert.Equal("Contoso", result.Company);
        Assert.Equal("Contoso Inc.", result.CurrentCompanyName);
        Assert.Equal(JobType.Hybrid, result.Type);

        var titles = result.Titles.ToList();
        Assert.Equal(3, titles.Count);

        Assert.Equal(firstTitleId, titles[0].Id);
        Assert.Equal("Developer", titles[0].Name);
        Assert.Equal(2020, titles[0].StartYear);
        Assert.Equal(2022, titles[0].EndYear);
        Assert.Equal("Built APIs", titles[0].Details);
        Assert.Equal("C#, SQL", titles[0].BulletPoints);

        Assert.Equal(secondTitleId, titles[1].Id);
        Assert.Equal("Senior Developer", titles[1].Name);
        Assert.Equal(2022, titles[1].StartYear);
        Assert.Equal(2024, titles[1].EndYear);
        Assert.Equal("Led projects", titles[1].Details);
        Assert.Equal("Architecture, Mentoring", titles[1].BulletPoints);

        Assert.Null(titles[2].Id);
        Assert.Null(titles[2].Name);

        var skills = result.Skills.ToList();
        Assert.Equal(2, skills.Count);

        Assert.Equal(firstSkillId, skills[0].Id);
        Assert.Equal("C#", skills[0].Name);
        Assert.Equal(SkillType.Technical, skills[0].Type);

        Assert.Equal(secondSkillId, skills[1].Id);
        Assert.Equal("Communication", skills[1].Name);
        Assert.Equal(SkillType.None, skills[1].Type);
    }

    [Fact]
    public void ToJob_WhenJobLevelValuesDifferAcrossRows_UsesFirstRowForJobFields()
    {
        // Arrange
        var jobId = Guid.NewGuid();

        var jobResults = new[]
        {
            new JobResult
            {
                Id = jobId,
                Company = "First Co",
                CurrentCompanyName = "First Co",
                Type = (short)JobType.Office
            },
            new JobResult
            {
                Id = jobId,
                Company = "Second Co",
                CurrentCompanyName = "Second Co",
                Type = (short)JobType.Remote
            }
        };

        // Act
        var result = WorkExperienceAdapter.ToJob(jobResults);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jobId, result.Id);
        Assert.Equal("First Co", result.Company);
        Assert.Equal(JobType.Office, result.Type);
    }
}
