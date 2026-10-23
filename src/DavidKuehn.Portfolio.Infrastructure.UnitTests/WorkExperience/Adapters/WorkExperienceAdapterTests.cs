using DavidKuehn.Portfolio.Core.WorkExperience.Enums;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Adapters;
using DataModels = DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models;

namespace DavidKuehn.Portfolio.Infrastructure.UnitTests.WorkExperience.Adapters;

public class WorkExperienceAdapterTests
{
    [Fact]
    public void ToJob_WhenJobAndCollectionsAreProvided_MapsAllJobData()
    {
        // Arrange
        var job = new DataModels.Job
        {
            Id = Guid.NewGuid(),
            StartYear = 2020,
            EndYear = 2024,
            Company = "Contoso",
            CurrentCompanyName = "Contoso Inc.",
            Type = (short)JobType.Hybrid
        };

        var titles = new[]
        {
            new DataModels.Title
            {
                Id = Guid.NewGuid(),
                TitleName = "Developer",
                StartYear = 2020,
                EndYear = 2022,
                Details = "Built APIs",
                BulletPoints = "C#, SQL"
            },
            new DataModels.Title
            {
                Id = Guid.NewGuid(),
                TitleName = "Senior Developer",
                StartYear = 2022,
                EndYear = 2024,
                Details = "Led projects",
                BulletPoints = "Architecture, mentoring"
            }
        };

        var skills = new[]
        {
            new DataModels.Skill
            {
                Id = Guid.NewGuid(),
                Name = "C#",
                Type = (short)SkillType.Technical
            },
            new DataModels.Skill
            {
                Id = Guid.NewGuid(),
                Name = "Communication",
                Type = (short)SkillType.None
            }
        };

        // Act
        var result = WorkExperienceAdapter.ToJob(job, titles, skills);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(job.Id, result.Id);
        Assert.Equal(job.StartYear, result.StartYear);
        Assert.Equal(job.EndYear, result.EndYear);
        Assert.Equal(job.Company, result.Company);
        Assert.Equal(job.CurrentCompanyName, result.CurrentCompanyName);
        Assert.Equal(JobType.Hybrid, result.Type);

        var mappedTitles = result.Titles.ToList();
        Assert.Equal(2, mappedTitles.Count);
        Assert.Equal(titles[0].Id, mappedTitles[0].Id);
        Assert.Equal(titles[0].TitleName, mappedTitles[0].TitleName);
        Assert.Equal(titles[0].StartYear, mappedTitles[0].StartYear);
        Assert.Equal(titles[0].EndYear, mappedTitles[0].EndYear);
        Assert.Equal(titles[0].Details, mappedTitles[0].Details);
        Assert.Equal(titles[0].BulletPoints, mappedTitles[0].BulletPoints);

        Assert.Equal(titles[1].Id, mappedTitles[1].Id);
        Assert.Equal(titles[1].TitleName, mappedTitles[1].TitleName);
        Assert.Equal(titles[1].StartYear, mappedTitles[1].StartYear);
        Assert.Equal(titles[1].EndYear, mappedTitles[1].EndYear);
        Assert.Equal(titles[1].Details, mappedTitles[1].Details);
        Assert.Equal(titles[1].BulletPoints, mappedTitles[1].BulletPoints);

        var mappedSkills = result.Skills.ToList();
        Assert.Equal(2, mappedSkills.Count);
        Assert.Equal(skills[0].Id, mappedSkills[0].Id);
        Assert.Equal(skills[0].Name, mappedSkills[0].Name);
        Assert.Equal(SkillType.Technical, mappedSkills[0].Type);

        Assert.Equal(skills[1].Id, mappedSkills[1].Id);
        Assert.Equal(skills[1].Name, mappedSkills[1].Name);
        Assert.Equal(SkillType.None, mappedSkills[1].Type);
    }

    [Fact]
    public void ToJob_WhenCollectionsAreEmpty_ReturnsEmptyCollections()
    {
        // Arrange
        var job = new DataModels.Job
        {
            Id = Guid.NewGuid(),
            StartYear = 2019,
            EndYear = 2021,
            Company = "Northwind",
            CurrentCompanyName = "Northwind Labs",
            Type = (short)JobType.Office
        };

        // Act
        var result = WorkExperienceAdapter.ToJob(job, Array.Empty<DataModels.Title>(), Array.Empty<DataModels.Skill>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(job.Id, result.Id);
        Assert.Equal(job.Company, result.Company);
        Assert.Equal(JobType.Office, result.Type);
        Assert.Empty(result.Titles);
        Assert.Empty(result.Skills);
    }
}
