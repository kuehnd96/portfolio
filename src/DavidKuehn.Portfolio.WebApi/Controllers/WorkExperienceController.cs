using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.UseCases.General;
using DavidKuehn.Portfolio.UseCases.General.Enums;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DavidKuehn.Portfolio.WebApi.Controllers;
[ApiController]
[Route("workexperience")]
[Authorize(Policy = "ApiKeyPolicy")]
public class WorkExperienceController : ControllerBase
{
    private readonly ILogger<WorkExperienceController> _logger;
    private readonly IQueryInvoker _queryInvoker;
    
    public WorkExperienceController(ILogger<WorkExperienceController> logger, IQueryInvoker queryInvoker)
    {
        _logger = logger;
        _queryInvoker = queryInvoker;
    }

    [HttpGet("GetJob/{jobId}", Name = "GetJobById")]
    public async Task<ActionResult<Job>> Get(Guid jobId)
    {
        var result = await _queryInvoker.InvokeQueryAsync<JobByIdQuery, Job>(new JobByIdQuery(jobId));

        if (result.Status == ResultStatus.Error)
        {
            _logger.LogError(result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }
        else if (result.Status == ResultStatus.NotFound)
        {
            _logger.LogWarning(result.ErrorMessage);
            return NotFound(result.ErrorMessage);
        }
        
        return Ok(result.Value);
    }

    [HttpGet("GetJobs", Name = "GetJobs")]  
    public async Task<ActionResult<IEnumerable<ListJob>>> GetJobs()
    {
        var result = await _queryInvoker.InvokeQueryAsync<JobListQuery, IEnumerable<ListJob>>(new JobListQuery());

        if (result.Status == ResultStatus.Error)
        {
            _logger.LogError(result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        return Ok(result.Value);
    }
}