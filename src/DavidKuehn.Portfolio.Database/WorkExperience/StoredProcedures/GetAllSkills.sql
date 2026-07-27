CREATE PROCEDURE [dbo].[GetAllSkills]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.[Id],
        s.[Name],
        s.[Type] FROM [dbo].[WorkExperienceSkills] s
	ORDER BY s.[Type], s.[Name]
END