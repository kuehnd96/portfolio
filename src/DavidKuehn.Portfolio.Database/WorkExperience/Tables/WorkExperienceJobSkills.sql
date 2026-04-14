CREATE TABLE [dbo].[WorkExperienceJobSkill]
(
    [JobId] UNIQUEIDENTIFIER NOT NULL,
    [SkillId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_WorkExperienceJobSkill] PRIMARY KEY ([JobId], [SkillId]),
    CONSTRAINT [FK_WorkExperienceJobSkill_Jobs] FOREIGN KEY ([JobId]) REFERENCES [dbo].[WorkExperienceJobs]([Id]),
    CONSTRAINT [FK_WorkExperienceJobSkill_WorkExperienceSkill] FOREIGN KEY ([SkillId]) REFERENCES [dbo].[WorkExperienceSkill]([Id])
)
