using DavidKuehn.Portfolio.Core.WorkExperience.Enums;
using DavidKuehn.Portfolio.Core.WorkExperience.Interfaces;
using CoreModels = DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Adapters;
using DataModels = DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data.Models;
using Microsoft.Data.SqlClient;

namespace DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data
{
    public class WorkExperienceData : IWorkExperienceData
    {
        private readonly string _connectionString;

        public WorkExperienceData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Core.WorkExperience.Models.Job> GetJob(Guid jobId)
        {
            List<Models.Skill> skills = new List<Models.Skill>();
            List<Models.Title> titles = new List<Models.Title>();
            Models.Job jobResult = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("dbo.GetJob", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@JobId", jobId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Read job (only one expected)
                        if (await reader.ReadAsync())
                        {
                            jobResult = new Models.Job()
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                StartYear = reader.GetInt32(reader.GetOrdinal("StartYear")),
                                EndYear = reader.GetInt32(reader.GetOrdinal("EndYear")),
                                Company = reader.GetString(reader.GetOrdinal("Company")),
                                CurrentCompanyName = reader.GetString(reader.GetOrdinal("CurrentCompanyName")),
                                Type = (short)reader.GetByte(reader.GetOrdinal("Type"))
                            };
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var titleResult = new Models.Title
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                    TitleName = reader.GetString(reader.GetOrdinal("Title")),
                                    StartYear = reader.GetInt32(reader.GetOrdinal("StartYear")),
                                    EndYear = reader.GetInt32(reader.GetOrdinal("EndYear")),
                                    Details = reader.GetString(reader.GetOrdinal("Details")),
                                    BulletPoints = reader.GetString(reader.GetOrdinal("BulletPoints"))
                                };

                                titles.Add(titleResult);
                            }
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var skillResult = new Models.Skill
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("SkillId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Type = reader.GetByte(reader.GetOrdinal("Type"))
                                };

                                skills.Add(skillResult);
                            }
                        }
                    }
                }
            }

            if (jobResult == null) return null;

            return WorkExperienceAdapter.ToJob(jobResult, titles, skills);
        }

        public async Task<IEnumerable<CoreModels.ListJob>> GetJobList()
        {
            var jobs = new List<CoreModels.ListJob>();

            using (var connection = new SqlConnection(_connectionString))
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
                            var listJob = new CoreModels.ListJob
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

        public async Task<IEnumerable<CoreModels.Skill>> GetSkills()
        {
            var skills = new List<CoreModels.Skill>();

            using (var connection = new SqlConnection(_connectionString))
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
                            var skill = new CoreModels.Skill
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")), 
                                Type = (SkillType)reader.GetByte(reader.GetOrdinal("Type"))
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
