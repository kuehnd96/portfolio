using DavidKuehn.Portfolio.Core.WorkExperience.Enums;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data;
using Microsoft.Data.SqlClient;

namespace DavidKuehn.Portfolio.Infrastructure.IntegrationTests.WorkExperience.Data;

[Collection(SqlServerCollection.CollectionName)]
public class WorkExperienceDataIntegrationTests
{
    private readonly SqlServerFixture _fixture;

    public WorkExperienceDataIntegrationTests(SqlServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetJob_WhenJobDoesNotExist_ReturnsNull()
    {
        // Arrange
        await _fixture.ResetAsync();
        var sut = new WorkExperienceData(_fixture.ConnectionString);

        // Act
        var result = await sut.GetJob(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetJob_WhenJobExists_ReturnsMappedJobWithTitlesAndSkills()
    {
        // Arrange
        await _fixture.ResetAsync();

        var jobId = Guid.NewGuid();
        var titleId = Guid.NewGuid();
        var skillId = Guid.NewGuid();

        await SeedJobAsync(jobId, 2021, 2024, "Contoso", "Contoso LLC", JobType.Hybrid);
        await SeedTitleAsync(titleId, jobId, "Senior Engineer", 2022, 2024, "Led delivery", "C#, SQL");
        await SeedSkillAsync(skillId, "C#", SkillType.Technical);
        await SeedJobSkillAsync(jobId, skillId);

        var sut = new WorkExperienceData(_fixture.ConnectionString);

        // Act
        var result = await sut.GetJob(jobId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jobId, result.Id);
        Assert.Equal(2021, result.StartYear);
        Assert.Equal(2024, result.EndYear);
        Assert.Equal("Contoso", result.Company);
        Assert.Equal("Contoso LLC", result.CurrentCompanyName);
        Assert.Equal(JobType.Hybrid, result.Type);

        var title = Assert.Single(result.Titles);
        Assert.Equal(titleId, title.Id);
        Assert.Equal("Senior Engineer", title.Name);
        Assert.Equal(2022, title.StartYear);
        Assert.Equal(2024, title.EndYear);
        Assert.Equal("Led delivery", title.Details);
        Assert.Equal("C#, SQL", title.BulletPoints);

        var skill = Assert.Single(result.Skills);
        Assert.Equal(skillId, skill.Id);
        Assert.Equal("C#", skill.Name);
        Assert.Equal(SkillType.Technical, skill.Type);
    }

    [Fact]
    public async Task GetJobList_WhenJobsExist_ReturnsSortedListWithLatestTitle()
    {
        // Arrange
        await _fixture.ResetAsync();

        var latestJobId = Guid.NewGuid();
        var olderJobId = Guid.NewGuid();

        await SeedJobAsync(olderJobId, 2018, 2020, "Older Co", "Older Co", JobType.Office);
        await SeedTitleForListAsync(Guid.NewGuid(), olderJobId, "Developer I", 2018, 2019, "Built features", "ASP.NET");
        await SeedTitleForListAsync(Guid.NewGuid(), olderJobId, "Developer II", 2019, 2020, "Owned modules", "SQL");

        await SeedJobAsync(latestJobId, 2021, 2024, "New Co", "New Co", JobType.Remote);
        await SeedTitleForListAsync(Guid.NewGuid(), latestJobId, "Engineer", 2021, 2022, "Implemented APIs", "REST");
        await SeedTitleForListAsync(Guid.NewGuid(), latestJobId, "Senior Engineer", 2022, 2024, "Led delivery", "Architecture");

        var sut = new WorkExperienceData(_fixture.ConnectionString);

        // Act
        var result = (await sut.GetJobList()).ToList();

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal(2021, result[0].StartYear);
        Assert.Equal(2024, result[0].EndYear);
        Assert.Equal("New Co", result[0].Company);
        Assert.Equal("New Co", result[0].CurrentCompanyName);
        Assert.Equal("Senior Engineer", result[0].Title);

        Assert.Equal(2018, result[1].StartYear);
        Assert.Equal(2020, result[1].EndYear);
        Assert.Equal("Older Co", result[1].Company);
        Assert.Equal("Older Co", result[1].CurrentCompanyName);
        Assert.Equal("Developer II", result[1].Title);
    }

    [Fact]
    public async Task GetSkills_WhenSkillsExist_ReturnsSkillsSortedByTypeThenName()
    {
        // Arrange
        await _fixture.ResetAsync();

        var noneSkillId = Guid.NewGuid();
        var technicalSkillId = Guid.NewGuid();
        var softSkillId = Guid.NewGuid();

        await SeedSkillAsync(softSkillId, "Communication", SkillType.Soft);
        await SeedSkillAsync(technicalSkillId, "C#", SkillType.Technical);
        await SeedSkillAsync(noneSkillId, "General", SkillType.None);

        var sut = new WorkExperienceData(_fixture.ConnectionString);

        // Act
        var result = (await sut.GetSkills()).ToList();

        // Assert
        Assert.Equal(3, result.Count);

        Assert.Equal(noneSkillId, result[0].Id);
        Assert.Equal("General", result[0].Name);
        Assert.Equal(SkillType.None, result[0].Type);

        Assert.Equal(technicalSkillId, result[1].Id);
        Assert.Equal("C#", result[1].Name);
        Assert.Equal(SkillType.Technical, result[1].Type);

        Assert.Equal(softSkillId, result[2].Id);
        Assert.Equal("Communication", result[2].Name);
        Assert.Equal(SkillType.Soft, result[2].Type);
    }

    private Task SeedJobAsync(
        Guid jobId,
        int startYear,
        int endYear,
        string company,
        string currentCompanyName,
        JobType jobType)
    {
        const string sql = """
            INSERT INTO [dbo].[WorkExperienceJobs]
                ([Id], [StartYear], [EndYear], [Company], [CurrentCompanyName], [Type])
            VALUES
                (@Id, @StartYear, @EndYear, @Company, @CurrentCompanyName, @Type);
            """;

        return _fixture.ExecuteNonQueryAsync(
            sql,
            new SqlParameter("@Id", jobId),
            new SqlParameter("@StartYear", startYear),
            new SqlParameter("@EndYear", endYear),
            new SqlParameter("@Company", company),
            new SqlParameter("@CurrentCompanyName", currentCompanyName),
            new SqlParameter("@Type", (byte)jobType));
    }

    private Task SeedTitleAsync(
        Guid titleId,
        Guid jobId,
        string title,
        int startYear,
        int endYear,
        string details,
        string bulletPoints)
    {
        const string sql = """
            INSERT INTO [dbo].[WorkExperienceTitle]
                ([Id], [JobId], [Title], [StartYear], [EndYear], [Details], [BulletPoints])
            VALUES
                (@Id, @JobId, @Title, @StartYear, @EndYear, @Details, @BulletPoints);
            """;

        return _fixture.ExecuteNonQueryAsync(
            sql,
            new SqlParameter("@Id", titleId),
            new SqlParameter("@JobId", jobId),
            new SqlParameter("@Title", title),
            new SqlParameter("@StartYear", startYear),
            new SqlParameter("@EndYear", endYear),
            new SqlParameter("@Details", details),
            new SqlParameter("@BulletPoints", bulletPoints));
    }

    private Task SeedTitleForListAsync(
        Guid titleId,
        Guid jobId,
        string title,
        int startYear,
        int endYear,
        string details,
        string bulletPoints)
    {
        const string sql = """
            INSERT INTO [dbo].[WorkExperienceTitles]
                ([Id], [JobId], [Title], [StartYear], [EndYear], [Details], [BulletPoints])
            VALUES
                (@Id, @JobId, @Title, @StartYear, @EndYear, @Details, @BulletPoints);
            """;

        return _fixture.ExecuteNonQueryAsync(
            sql,
            new SqlParameter("@Id", titleId),
            new SqlParameter("@JobId", jobId),
            new SqlParameter("@Title", title),
            new SqlParameter("@StartYear", startYear),
            new SqlParameter("@EndYear", endYear),
            new SqlParameter("@Details", details),
            new SqlParameter("@BulletPoints", bulletPoints));
    }

    private Task SeedSkillAsync(Guid skillId, string name, SkillType skillType)
    {
        const string sql = """
            INSERT INTO [dbo].[WorkExperienceSkill]
                ([Id], [Name], [Type])
            VALUES
                (@Id, @Name, @Type);
            """;

        return _fixture.ExecuteNonQueryAsync(
            sql,
            new SqlParameter("@Id", skillId),
            new SqlParameter("@Name", name),
            new SqlParameter("@Type", (byte)skillType));
    }

    private Task SeedJobSkillAsync(Guid jobId, Guid skillId)
    {
        const string sql = """
            INSERT INTO [dbo].[WorkExperienceJobSkill]
                ([JobId], [SkillId])
            VALUES
                (@JobId, @SkillId);
            """;

        return _fixture.ExecuteNonQueryAsync(
            sql,
            new SqlParameter("@JobId", jobId),
            new SqlParameter("@SkillId", skillId));
    }
}