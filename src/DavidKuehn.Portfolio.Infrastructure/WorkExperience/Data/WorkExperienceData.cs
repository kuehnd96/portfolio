using DavidKuehn.Portfolio.Core.WorkExperience.Enums;
using DavidKuehn.Portfolio.Core.WorkExperience.Interfaces;
using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models;
using Microsoft.Data.SqlClient;

namespace DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data
{
    public class WorkExperienceData : IWorkExperienceData
    {
        //TODO: Get SQL connection string in place
        
        public async Task<Job?> GetJob(Guid jobId)
        {
            var jobResults = new List<JobResult>();
            var connectionString = "YourConnectionStringHere";

            using (var connection = new SqlConnection(connectionString))
            {
                // Open the connection
                await connection.OpenAsync();

                // Create a command
                using (var command = new SqlCommand("dbo.GetJob", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@JobId", jobId);

                    // Execute the command
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var jobResult = new JobResult
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                StartYear = reader.GetInt32(reader.GetOrdinal("StartYear")),
                                EndYear = reader.GetInt32(reader.GetOrdinal("EndYear")),
                                Company = reader.GetString(reader.GetOrdinal("Company")),
                                CurrentCompanyName = reader.GetString(reader.GetOrdinal("CurrentCompanyName")),
                                Type = (byte)reader.GetInt32(reader.GetOrdinal("Type")),
                                TitleId = reader.IsDBNull(reader.GetOrdinal("TitleId")) ? null : reader.GetGuid(reader.GetOrdinal("TitleId")),
                                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                                TitleStartYear = reader.IsDBNull(reader.GetOrdinal("TitleStartYear")) ? null : reader.GetInt32(reader.GetOrdinal("TitleStartYear")),
                                TitleEndYear = reader.IsDBNull(reader.GetOrdinal("TitleEndYear")) ? null : reader.GetInt32(reader.GetOrdinal("TitleEndYear")),
                                Details = reader.IsDBNull(reader.GetOrdinal("Details")) ? null : reader.GetString(reader.GetOrdinal("Details")),
                                BulletPoints = reader.IsDBNull(reader.GetOrdinal("BulletPoints")) ? null : reader.GetString(reader.GetOrdinal("BulletPoints")),
                                SkillId = reader.IsDBNull(reader.GetOrdinal("SkillId")) ? null : reader.GetGuid(reader.GetOrdinal("SkillId")),
                                SkillName = reader.IsDBNull(reader.GetOrdinal("SkillName")) ? null : reader.GetString(reader.GetOrdinal("SkillName")),
                                SkillType = reader.IsDBNull(reader.GetOrdinal("SkillType")) ? null : (Int16?)reader.GetInt16(reader.GetOrdinal("SkillType"))
                            };

                            jobResults.Add(jobResult);
                        }
                    }
                }
            }

            return jobResults.GroupBy(result => result.Id)
                .Select(group => new Job
                {
                    Id = group.Key,
                    StartYear = group.First().StartYear,
                    EndYear = group.First().EndYear,
                    Company = group.First().Company,
                    CurrentCompanyName = group.First().CurrentCompanyName,
                    Type = (JobType)group.First().Type,
                    Titles = group.Select(result => new Title
                    {
                        Id = result.TitleId,
                        Name = result.Title,
                        StartYear = result.TitleStartYear,
                        EndYear = result.TitleEndYear,
                        Details = result.Details,
                        BulletPoints = result.BulletPoints
                    }).ToList(),
                    Skills = group
                        .Where(result => result.SkillId.HasValue && !string.IsNullOrEmpty(result.SkillName))
                        .Select(result => new Skill
                        {
                            Id = result.SkillId!.Value,
                            Name = result.SkillName!,
                            Type = result.SkillType.HasValue ? (SkillType)result.SkillType.Value : SkillType.None
                        }).ToList()
                }).FirstOrDefault();
        }

        public async Task<IEnumerable<ListJob>> GetJobList()
        {
            var jobs = new List<ListJob>();
            var connectionString = "YourConnectionStringHere";

            using (var connection = new SqlConnection(connectionString))
            {
                // Open the connection
                await connection.OpenAsync();

                // Create a command
                using (var command = new SqlCommand("dbo.GetJobList", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Execute the command
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var listJob = new ListJob
                            {
                                StartYear = reader.GetInt32(reader.GetOrdinal("StartYear")),
                                EndYear = reader.GetInt32(reader.GetOrdinal("EndYear")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Company = reader.GetString(reader.GetOrdinal("Company")),
                                CurrentCompanyName = reader.GetString(reader.GetOrdinal("CurrentCompanyName"))
                            };

                            jobs.Add(listJob);
                        }
                    }
                }
            }

            return jobs;
        }

        public async Task<IEnumerable<Skill>> GetSkills()
        {
            var skills = new List<Skill>();
            var connectionString = "YourConnectionStringHere";

            using (var connection = new SqlConnection(connectionString))
            {
                // Open the connection
                await connection.OpenAsync();

                // Create a command
                using (var command = new SqlCommand("dbo.GetAllSkills", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Execute the command
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var skill = new Skill
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")), 
                                Type = (SkillType)reader.GetInt32(reader.GetOrdinal("Type"))
                            };

                            skills.Add(skill);
                        }
                    }
                }
            }

            return skills;
        }
    }
}
