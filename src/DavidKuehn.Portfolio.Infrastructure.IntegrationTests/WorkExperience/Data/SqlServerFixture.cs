using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace DavidKuehn.Portfolio.Infrastructure.IntegrationTests.WorkExperience.Data;

[CollectionDefinition(CollectionName, DisableParallelization = true)]
public sealed class SqlServerCollection : ICollectionFixture<SqlServerFixture>
{
    public const string CollectionName = "WorkExperience SQL Server";
}

public sealed class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder().Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await EnsureSchemaAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public async Task ResetAsync()
    {
        const string resetSql = """
            DELETE FROM [dbo].[WorkExperienceJobSkills];
            DELETE FROM [dbo].[WorkExperienceTitles];
            DELETE FROM [dbo].[WorkExperienceSkills];
            DELETE FROM [dbo].[WorkExperienceJobs];
            """;

        await ExecuteNonQueryAsync(resetSql);
    }

    public async Task ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }

        await command.ExecuteNonQueryAsync();
    }

    private async Task EnsureSchemaAsync()
    {
        const string setupSql = """
            IF OBJECT_ID('dbo.GetJob', 'P') IS NOT NULL DROP PROCEDURE [dbo].[GetJob];
            IF OBJECT_ID('dbo.GetJobList', 'P') IS NOT NULL DROP PROCEDURE [dbo].[GetJobList];
            IF OBJECT_ID('dbo.GetAllSkills', 'P') IS NOT NULL DROP PROCEDURE [dbo].[GetAllSkills];

            IF OBJECT_ID('dbo.WorkExperienceJobSkills', 'U') IS NOT NULL DROP TABLE [dbo].[WorkExperienceJobSkills];
            IF OBJECT_ID('dbo.WorkExperienceTitles', 'U') IS NOT NULL DROP TABLE [dbo].[WorkExperienceTitles];
            IF OBJECT_ID('dbo.WorkExperienceSkills', 'U') IS NOT NULL DROP TABLE [dbo].[WorkExperienceSkills];
            IF OBJECT_ID('dbo.WorkExperienceJobs', 'U') IS NOT NULL DROP TABLE [dbo].[WorkExperienceJobs];

            CREATE TABLE [dbo].[WorkExperienceJobs]
            (
                [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                [StartYear] INT NOT NULL,
                [EndYear] INT NOT NULL,
                [Company] NVARCHAR(50) NOT NULL,
                [CurrentCompanyName] NVARCHAR(50) NOT NULL,
                [Type] TINYINT NOT NULL
            );

            CREATE TABLE [dbo].[WorkExperienceTitles]
            (
                [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                [JobId] UNIQUEIDENTIFIER NOT NULL,
                [Title] VARCHAR(50) NOT NULL,
                [StartYear] INT NOT NULL,
                [EndYear] INT NOT NULL,
                [Details] NVARCHAR(100) NOT NULL,
                [BulletPoints] VARCHAR(MAX) NOT NULL,
                CONSTRAINT [FK_WorkExperienceTitles_Jobs] FOREIGN KEY ([JobId]) REFERENCES [dbo].[WorkExperienceJobs]([Id])
            );

            CREATE TABLE [dbo].[WorkExperienceSkills]
            (
                [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                [Name] VARCHAR(50) NOT NULL,
                [Type] TINYINT NOT NULL
            );

            CREATE TABLE [dbo].[WorkExperienceJobSkills]
            (
                [JobId] UNIQUEIDENTIFIER NOT NULL,
                [SkillId] UNIQUEIDENTIFIER NOT NULL,
                CONSTRAINT [PK_WorkExperienceJobSkills] PRIMARY KEY ([JobId], [SkillId]),
                CONSTRAINT [FK_WorkExperienceJobSkills_Jobs] FOREIGN KEY ([JobId]) REFERENCES [dbo].[WorkExperienceJobs]([Id]),
                CONSTRAINT [FK_WorkExperienceJobSkills_Skills] FOREIGN KEY ([SkillId]) REFERENCES [dbo].[WorkExperienceSkills]([Id])
            );
            """;

        const string createGetJobSql = """
            CREATE PROCEDURE [dbo].[GetJob]
                @JobId UNIQUEIDENTIFIER
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT
                    j.[Id],
                    j.[StartYear],
                    j.[EndYear],
                    j.[Company],
                    j.[CurrentCompanyName],
                    j.[Type]
                FROM [dbo].[WorkExperienceJobs] AS j
                WHERE j.[Id] = @JobId;

                SELECT
                    t.[Id],
                    t.[Title],
                    t.[StartYear],
                    t.[EndYear],
                    t.[Details],
                    t.[BulletPoints]
                FROM [dbo].[WorkExperienceTitles] AS t
                WHERE t.[JobId] = @JobId;

                SELECT
                    js.[SkillId],
                    s.[Name],
                    s.[Type]
                FROM [dbo].[WorkExperienceJobSkills] AS js
                INNER JOIN [dbo].[WorkExperienceSkills] AS s
                    ON s.[Id] = js.[SkillId]
                WHERE js.[JobId] = @JobId;
            END;
            """;

        const string createGetJobListSql = """
            CREATE PROCEDURE [dbo].[GetJobList]
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT
                    j.[StartYear],
                    j.[EndYear],
                    j.[Company],
                    j.[CurrentCompanyName],
                    lt.[Title]
                FROM [dbo].[WorkExperienceJobs] j
                OUTER APPLY
                (
                    SELECT TOP (1)
                        t.[Title]
                    FROM [dbo].[WorkExperienceTitles] t
                    WHERE t.[JobId] = j.[Id]
                    ORDER BY t.[StartYear] DESC
                ) lt
                ORDER BY j.[StartYear] DESC, j.[EndYear] DESC;
            END;
            """;

        const string createGetAllSkillsSql = """
            CREATE PROCEDURE [dbo].[GetAllSkills]
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT 
                    s.[Id],
                    s.[Name],
                    s.[Type] FROM [dbo].[WorkExperienceSkills] s
                ORDER BY s.[Type], s.[Name]
            END;
            """;

        await ExecuteNonQueryAsync(setupSql);
        await ExecuteNonQueryAsync(createGetJobSql);
        await ExecuteNonQueryAsync(createGetJobListSql);
        await ExecuteNonQueryAsync(createGetAllSkillsSql);
    }
}