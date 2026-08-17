using DavidKuehn.Portfolio.Core.WorkExperience.Enums;
using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models;

namespace DavidKuehn.Portfolio.Infrastructure.WorkExperience.Adapters
{
    internal static class WorkExperienceAdapter
    {
        internal static Job? ToJob(IEnumerable<JobResult> jobResults)
        {
            var jobResultList = jobResults.ToList();
            var firstResult = jobResultList.FirstOrDefault();

            if (firstResult is null)
            {
                return null;
            }

            return new Job
            {
                Id = firstResult.Id,
                StartYear = firstResult.StartYear,
                EndYear = firstResult.EndYear,
                Company = firstResult.Company,
                CurrentCompanyName = firstResult.CurrentCompanyName,
                Type = (JobType)firstResult.Type,
                Titles = jobResultList.Select(result => new Title
                {
                    Id = result.TitleId,
                    Name = result.Title,
                    StartYear = result.TitleStartYear,
                    EndYear = result.TitleEndYear,
                    Details = result.Details,
                    BulletPoints = result.BulletPoints
                }).ToList(),
                Skills = jobResultList
                    .Where(result => result.SkillId.HasValue && !string.IsNullOrEmpty(result.SkillName))
                    .Select(result => new Skill
                    {
                        Id = result.SkillId!.Value,
                        Name = result.SkillName!,
                        Type = result.SkillType.HasValue ? (SkillType)result.SkillType.Value : SkillType.None
                    }).ToList()
            };
        }
    }
}
