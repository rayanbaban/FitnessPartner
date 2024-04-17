using FitnessPartner.Models.DTOs;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionPlansController : ControllerBase
    {
        private readonly INutritionPlanService _nutritionPlanService;

        public NutritionPlansController(INutritionPlanService nutritionPlanService)
        {
            _nutritionPlanService = nutritionPlanService;
        }

        [HttpGet(Name = "GetAllNutritionPlans")]
        public async Task<ActionResult<ICollection<NutritionPlansDTO>>> GetAllNutritionPlansAsync(int pageNr = 1, int pageSize = 10)
        {
            return Ok(await _nutritionPlanService.GetPageAsync(pageNr, pageSize));
        }

        [HttpGet("{id}", Name = "GetNutritionPlanById")]
        public async Task<ActionResult<NutritionPlansDTO>> GetNutritionPlanById(int id)
        {
            var res = await _nutritionPlanService.GetNutritionPlanByIdAsync(id);
            return res != null ? Ok(res) : NotFound("Fant ikke NutritionPlan");
        }

        [HttpPut("{id}", Name = "UpdateNutritionPlan")]
        public async Task<ActionResult<NutritionPlansDTO>> UpdateNutritionPlanAsync(int id, NutritionPlansDTO nutritionPlanDTO)
        {
            int loginUserId = (int)HttpContext.Items["UserId"]!;

            var updatedNutritionPlan = await _nutritionPlanService.UpdateNutritionPlanAsync(nutritionPlanDTO, id, loginUserId);

            return updatedNutritionPlan != null ?
                       Ok(updatedNutritionPlan) :
                       NotFound($"Klarte ikke å oppdatere bruker med ID: {id}");
        }

        [HttpDelete("{id}", Name = "DeleteNutritionPlan")]
        public async Task<ActionResult<NutritionPlansDTO>> DeleteNutritionPlan(int id, int nutritionPlanId)
        {
            int loginMemberId = (HttpContext.Items["UserId"] as int?) ?? 0;

            var deletedNutritionPlan = await _nutritionPlanService.DeleteNutritionPlanAsync(id, nutritionPlanId);

            if (deletedNutritionPlan != null)
            {
                return Ok($"Exercise med ID {nutritionPlanId} ble slettet vellykket");
            }
            return NotFound($"Exercise med ID {nutritionPlanId} ble ikke funnet");
        }

        [HttpPost(Name = "CreateNutritionPlan")]
        public async Task<ActionResult<NutritionPlansDTO>> PostNutritionPlan([FromBody] NutritionPlansDTO nutritionPlan, int loggedinUser)
        {
            try
            {
                if (nutritionPlan == null)
                {
                    return BadRequest("Ugyldige nutritionPlan data");
                }

                //int loginUserId = (int)HttpContext.Items["UserId"]!;
                var addedNutritionPlan = await _nutritionPlanService.CreateNutritionPlanAsync(/*loginUserId,*/ nutritionPlan, loggedinUser);

                if (addedNutritionPlan != null)
                {

                    return Ok(addedNutritionPlan);
                }

                return BadRequest("Feil ved opprettelse av nutritionPlan");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Intern feil: {ex.Message}");
            }
        }
    }
}
