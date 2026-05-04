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
        FROM [dbo].[WorkExperienceJobTitles] t
        WHERE t.[WorkExperienceJobId] = j.[Id]
        ORDER BY t.[StartYear] DESC
    ) lt
    ORDER BY j.[StartYear] DESC, j.[EndYear] DESC;
END