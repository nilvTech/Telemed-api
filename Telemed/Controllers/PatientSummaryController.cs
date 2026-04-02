// Telemed/Controllers/PatientSummaryController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientSummaryController : ControllerBase
    {
        private readonly IPatientSummaryService _summaryService;

        public PatientSummaryController(IPatientSummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<PatientSummaryDto>> GetSummary()
        {
            var data = await _summaryService.GetPatientSummaryAsync();
            return Ok(data);
        }
    }
}