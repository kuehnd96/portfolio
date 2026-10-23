using DavidKuehn.Portfolio.Core.WorkExperience.Enums;
using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models;
using CoreModels = DavidKuehn.Portfolio.Core.WorkExperience.Models;

namespace DavidKuehn.Portfolio.Infrastructure.WorkExperience.Adapters
{
    internal static class WorkExperienceAdapter
    {
        internal static CoreModels.Job ToJob(Data.Models.Job job, IEnumerable<Data.Models.Title> titles, IEnumerable<Data.Models.Skill> skills)
        {
            return new CoreModels.Job
            {
                Id = job.Id,
                StartYear = job.StartYear,
                EndYear = job.EndYear,
                Company = job.Company,
                CurrentCompanyName = job.CurrentCompanyName,
                Type = (JobType)job.Type,
                Titles = titles
                    .Select(result => new CoreModels.Title
                    {
                        Id = result.Id,
                        TitleName = result.TitleName,
                        StartYear = result.StartYear,
                        EndYear = result.EndYear,
                        Details = result.Details,
                        BulletPoints = result.BulletPoints
                    }).ToList(),
                Skills = skills
                    .Select(result => new CoreModels.Skill
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Type = (SkillType)result.Type
                    }).ToList()
            };
        }
    }
}
