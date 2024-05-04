using FitnessPartner.Models.DTOs;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionResourcesController : ControllerBase
    {
        private readonly INutritionResourceService _nutritionResourcesService;

        public NutritionResourcesController(INutritionResourceService nutritionResourcesService)
        {
            _nutritionResourcesService = nutritionResourcesService;
        }

        [HttpGet(Name = "GetAllNutritionResources")]
        public async Task<ActionResult<ICollection<NutritionResourcesDTO>>> GetAllNutritionResourcesAsync(int pageNr = 1, int pageSize = 10)
        {
            return Ok(await _nutritionResourcesService.GetPageAsync(pageNr, pageSize));
        }

        [HttpGet("{id}", Name = "GetNutritionResourcesById")]
        public async Task<ActionResult<NutritionResourcesDTO>> GetNutritionResourceById(int id)
        {
            var res = await _nutritionResourcesService.GetNutritionResourceByIdAsync(id);
            return res != null ? Ok(res) : NotFound("Fant ikke NutritionResource");
        }

        [HttpPut("{id}", Name = "UpdateNutritionResource")]
        [Authorize (Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<NutritionResourcesDTO>> UpdateNutritionResourceAsync(int id, NutritionResourcesDTO nutritionResourceDTO)
        {

            var updatedNutritionResource = await _nutritionResourcesService.UpdateNutritionResourceAsync(nutritionResourceDTO, id);

            return updatedNutritionResource != null ?
                       Ok(updatedNutritionResource) :
                       NotFound($"Klarte ikke å oppdatere bruker med ID: {id}");

        }

        [HttpDelete(Name = "DeleteNutritionResource")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<NutritionResourcesDTO>> DeleteNutritionResource(int nutritionResourceId)
        {

            var deletedNutritionResource = await _nutritionResourcesService.DeleteNutritionResourceAsync( nutritionResourceId);

            if (deletedNutritionResource != null)
            {
                return Ok($"Exercise med ID {nutritionResourceId} ble slettet vellykket");
            }
            return NotFound($"Exercise med ID {nutritionResourceId} ble ikke funnet");
        }

        [HttpPost(Name = "CreateNutritionResource")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<NutritionResourcesDTO>> PostNutritionResource([FromBody] NutritionResourcesDTO nutritionResource)
        {
            try
            {
                if (nutritionResource == null)
                {
                    return BadRequest("Ugyldige nutritionResource data");
                }

                var addedNutritionResource = await _nutritionResourcesService.CreateNutritionResourceAsync(nutritionResource);

                if (addedNutritionResource != null)
                {

                    return Ok(addedNutritionResource);
                }

                return BadRequest("Feil ved opprettelse av nutritionResource");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Intern feil: {ex.Message}");
            }
        }
    }
}
