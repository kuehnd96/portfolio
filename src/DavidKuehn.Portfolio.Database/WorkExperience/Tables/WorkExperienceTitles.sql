CREATE TABLE [dbo].[WorkExperienceTitle]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [JobId] UNIQUEIDENTIFIER NOT NULL,
    [Title] VARCHAR(50) NOT NULL, 
    [StartYear] INT NOT NULL, 
    [EndYear] INT NOT NULL, 
    [Details] NVARCHAR(100) NOT NULL, 
    [BulletPoints] VARCHAR(MAX) NOT NULL,
    CONSTRAINT [FK_WorkExperienceTitle_Jobs] FOREIGN KEY ([JobId]) REFERENCES [dbo].[WorkExperienceJobs]([Id])
)

