CREATE PROCEDURE [dbo].[GetJob]
    @JobId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- 1) Job row(s)
    SELECT
        j.[Id],
        j.[StartYear],
        j.[EndYear],
        j.[Company],
        j.[CurrentCompanyName],
        j.[Type]
    FROM [dbo].[WorkExperienceJobs] AS j
    WHERE j.[Id] = @JobId;

    -- 2) Title row(s) for that job
    SELECT
        t.[Id] AS [TitleId],
        t.[JobId],
        t.[Title],
        t.[StartYear] AS [TitleStartYear],
        t.[EndYear] AS [TitleEndYear],
        t.[Details],
        t.[BulletPoints]
    FROM [dbo].[WorkExperienceTitles] AS t
    WHERE t.[JobId] = @JobId;

    -- 3) Skills for that job
    SELECT
        js.[JobId],
        js.[SkillId],
        s.[Name] AS [SkillName],
        s.[Type] AS [SkillType]
    FROM [dbo].[WorkExperienceJobSkills] AS js
    INNER JOIN [dbo].[WorkExperienceSkills] AS s
        ON s.[Id] = js.[SkillId]
    WHERE js.[JobId] = @JobId;
END;