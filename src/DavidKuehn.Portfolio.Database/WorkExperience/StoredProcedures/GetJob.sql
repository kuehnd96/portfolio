CREATE PROCEDURE [dbo].[GetJob]
	@JobId UNIQUEIDENTIFIER
AS
 SELECT
		j.[Id],
		j.[StartYear],
		j.[EndYear],
		j.[Company],
		j.[CurrentCompanyName],
		j.[Type],
		t.[Id] AS [TitleId],
		t.[Title],
		t.[StartYear] AS [TitleStartYear],
		t.[EndYear] AS [TitleEndYear],
		t.[Details],
		t.[BulletPoints],
		js.[SkillId],
		s.[Name] AS [SkillName],
		s.[Type] AS [SkillType]
	FROM [dbo].[WorkExperienceJobs] j
	LEFT JOIN [dbo].[WorkExperienceTitles] t ON t.[JobId] = j.[Id]
	LEFT JOIN [dbo].[WorkExperienceJobSkills] js ON js.[JobId] = j.[Id]
	LEFT JOIN [dbo].[WorkExperienceSkills] s ON s.[Id] = js.[SkillId]
	WHERE j.Id = @JobId