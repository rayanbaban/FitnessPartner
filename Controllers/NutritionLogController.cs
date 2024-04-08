using FitnessPartner.Models.DTOs;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionLogController : ControllerBase
    {
        //private readonly NutritionLogService _nutritionLogService;
        //private readonly ILogger<NutritionResourcesController> _logger;
        private readonly INutritionLogService _nutritionLogService;
        private readonly ILogger<NutritionLogController> _logger;



        public NutritionLogController(INutritionLogService nutritionLogService, ILogger<NutritionLogController> logger)
        {
            _nutritionLogService = nutritionLogService;
            _logger = logger;
        }


        [HttpGet(Name = "GetAllNutritionLogs")]
        public async Task<ActionResult<ICollection<NutritionLogDTO>>> GetAllNutritionLogsAsync(int pageNr = 1, int pageSize = 10)
        {
            return Ok(await _nutritionLogService.GetPageAsync(pageNr, pageSize));
        }

        [HttpGet("{id}", Name = "GetNutritionLogById")]
        public async Task<ActionResult<NutritionLogDTO>> GetNutritionLogById(int id)
        {
            var res = await _nutritionLogService.GetNutritionLogByIdAsync(id);
            return res != null ? Ok(res) : NotFound("Fant ikke NutritionLog");
        }

        [HttpPut("{id}", Name = "UpdateNutritionLog")]
        public async Task<ActionResult<NutritionLogDTO>> UpdateNutritionLogAsync(int id, NutritionLogDTO nutritionLogDTO)
        {
            int loginUserId = (int)HttpContext.Items["UserId"]!;

            var updatedNutritionLog = await _nutritionLogService.UpdateNutritionLogAsync(nutritionLogDTO, id, loginUserId);

            return updatedNutritionLog != null ?
                       Ok(updatedNutritionLog) :
                       NotFound($"Klarte ikke å oppdatere bruker med ID: {id}");
        }

        [HttpDelete("{id}", Name = "DeleteNutritionLog")]
        public async Task<ActionResult<NutritionLogDTO>> DeleteNutritionLog(int id, int nutritionLogId)
        {
            int loginMemberId = (HttpContext.Items["UserId"] as int?) ?? 0;

            var deletedNutritionLog = await _nutritionLogService.DeleteNutritionLogAsync(id, nutritionLogId);

            if (deletedNutritionLog != null)
            {
                return Ok($"Exercise med ID {nutritionLogId} ble slettet vellykket");
            }
            return NotFound($"Exercise med ID {nutritionLogId} ble ikke funnet");
        }

        [HttpPost(Name = "CreateNutritionLog")]

        public async Task<ActionResult<NutritionLogDTO>> PostNutritionLog([FromBody] NutritionLogDTO nutritionLog, int loggedinUser)
        {
            try
            {
                if (nutritionLog == null)
                {
                    return BadRequest("Ugyldige nutritionLog data");
                }

                //int loginUserId = (int)HttpContext.Items["UserId"]!;
                var addedNutritionLog = await _nutritionLogService.CreateNutritionLogAsync(/*loginUserId,*/ nutritionLog, loggedinUser);

                if (addedNutritionLog != null)
                {

                    return Ok(addedNutritionLog);
                }

                return BadRequest("Feil ved opprettelse av nutritionLog");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Intern feil: {ex.Message}");
            }
        }
    }
}


