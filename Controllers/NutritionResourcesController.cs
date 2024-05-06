using FitnessPartner.Models.DTOs;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPartner.Controllers
{
	/// <summary>
	/// Kontrolleren for ernæringsressurser.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class NutritionResourcesController : ControllerBase
	{
		private readonly INutritionResourceService _nutritionResourcesService;

		public NutritionResourcesController(INutritionResourceService nutritionResourcesService)
		{
			_nutritionResourcesService = nutritionResourcesService;
		}

		/// <summary>
		/// Henter alle ernæringsressurser.
		/// </summary>
		/// <param name="pageNr">Sidenummer for paginering.</param>
		/// <param name="pageSize">Antall ressurser per side.</param>
		[HttpGet(Name = "GetAllNutritionResources")]
		public async Task<ActionResult<ICollection<NutritionResourcesDTO>>> GetAllNutritionResourcesAsync(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _nutritionResourcesService.GetPageAsync(pageNr, pageSize));
		}

		/// <summary>
		/// Henter en ernæringsressurs basert på ID.
		/// </summary>
		/// <param name="id">ID-en til ernæringsressursen.</param>
		[HttpGet("{id}", Name = "GetNutritionResourcesById")]
		public async Task<ActionResult<NutritionResourcesDTO>> GetNutritionResourceById(int id)
		{
			var res = await _nutritionResourcesService.GetNutritionResourceByIdAsync(id);
			return res != null ? Ok(res) : NotFound("Fant ikke NutritionResource");
		}

		/// <summary>
		/// Oppdaterer en ernæringsressurs.
		/// </summary>
		/// <param name="id">ID-en til ernæringsressursen som skal oppdateres.</param>
		/// <param name="nutritionResourceDTO">Oppdatert informasjon om ernæringsressursen.</param>
		[HttpPut("{id}", Name = "UpdateNutritionResource")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<NutritionResourcesDTO>> UpdateNutritionResourceAsync(int id, NutritionResourcesDTO nutritionResourceDTO)
		{
			var updatedNutritionResource = await _nutritionResourcesService.UpdateNutritionResourceAsync(nutritionResourceDTO, id);

			return updatedNutritionResource != null ?
					   Ok(updatedNutritionResource) :
					   NotFound($"Klarte ikke å oppdatere bruker med ID: {id}");
		}

		/// <summary>
		/// Sletter en ernæringsressurs.
		/// </summary>
		/// <param name="nutritionResourceId">ID-en til ernæringsressursen som skal slettes.</param>
		[HttpDelete(Name = "DeleteNutritionResource")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<NutritionResourcesDTO>> DeleteNutritionResource(int nutritionResourceId)
		{
			var deletedNutritionResource = await _nutritionResourcesService.DeleteNutritionResourceAsync(nutritionResourceId);

			if (deletedNutritionResource != null)
			{
				return Ok($"Exercise med ID {nutritionResourceId} ble slettet vellykket");
			}
			return NotFound($"Exercise med ID {nutritionResourceId} ble ikke funnet");
		}

		/// <summary>
		/// Oppretter en ny ernæringsressurs.
		/// </summary>
		/// <param name="nutritionResource">Informasjon om den nye ernæringsressursen.</param>
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
