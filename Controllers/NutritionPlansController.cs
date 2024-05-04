using FitnessPartner.Models.DTOs;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPut(Name = "UpdateNutritionPlan")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<NutritionPlansDTO>> UpdateNutritionPlanAsync(int id, NutritionPlansDTO nutritionPlanDTO)
        {
            var updatedNutritionPlan = await _nutritionPlanService.UpdateNutritionPlanAsync(nutritionPlanDTO, id);

            return updatedNutritionPlan != null ?
                       Ok(updatedNutritionPlan) :
                       NotFound($"Klarte ikke å oppdatere bruker med ID: {id}");
        }

        [HttpDelete(Name = "DeleteNutritionPlan")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<NutritionPlansDTO>> DeleteNutritionPlan(int id)
        {
            var deletedNutritionPlan = await _nutritionPlanService.DeleteNutritionPlanAsync(id);

            if (deletedNutritionPlan != null)
            {
                return Ok($"Exercise med ID {id} ble slettet vellykket");
            }
            return NotFound($"Exercise med ID {id} ble ikke funnet");
        }

        [HttpPost(Name = "CreateNutritionPlan")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<NutritionPlansDTO>> PostNutritionPlan([FromBody] NutritionPlansDTO nutritionPlan)
        {
            try
            {
                if (nutritionPlan == null)
                {
                    return BadRequest("Ugyldige nutritionPlan data");
                }

                var addedNutritionPlan = await _nutritionPlanService.CreateNutritionPlanAsync(nutritionPlan);

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
