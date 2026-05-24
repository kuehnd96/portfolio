CREATE TABLE [dbo].[WorkExperienceJobs]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [StartYear] INT NOT NULL, 
    [EndYear] INT NOT NULL, 
    [Company] NVARCHAR(50) NOT NULL, 
    [CurrentCompanyName] NVARCHAR(50) NOT NULL, 
    [Type] TINYINT NOT NULL
)
