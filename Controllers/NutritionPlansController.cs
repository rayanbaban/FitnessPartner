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
	/// Kontrolleren for ernæringsplaner.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class NutritionPlansController : ControllerBase
	{
		private readonly INutritionPlanService _nutritionPlanService;

		public NutritionPlansController(INutritionPlanService nutritionPlanService)
		{
			_nutritionPlanService = nutritionPlanService;
		}

		/// <summary>
		/// Henter alle ernæringsplaner.
		/// </summary>
		/// <param name="pageNr">Sidenummer for paginering.</param>
		/// <param name="pageSize">Antall planer per side.</param>
		[HttpGet(Name = "GetAllNutritionPlans")]
		public async Task<ActionResult<ICollection<NutritionPlansDTO>>> GetAllNutritionPlansAsync(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _nutritionPlanService.GetPageAsync(pageNr, pageSize));
		}

		/// <summary>
		/// Henter en ernæringsplan basert på ID.
		/// </summary>
		/// <param name="id">ID-en til ernæringsplanen.</param>
		[HttpGet("{id}", Name = "GetNutritionPlanById")]
		public async Task<ActionResult<NutritionPlansDTO>> GetNutritionPlanById(int id)
		{
			var res = await _nutritionPlanService.GetNutritionPlanByIdAsync(id);
			return res != null ? Ok(res) : NotFound("Fant ikke NutritionPlan");
		}

		/// <summary>
		/// Oppdaterer en ernæringsplan.
		/// </summary>
		/// <param name="id">ID-en til ernæringsplanen som skal oppdateres.</param>
		/// <param name="nutritionPlanDTO">Oppdatert informasjon om ernæringsplanen.</param>
		[HttpPut(Name = "UpdateNutritionPlan")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<NutritionPlansDTO>> UpdateNutritionPlanAsync(int id, NutritionPlansDTO nutritionPlanDTO)
		{
			var updatedNutritionPlan = await _nutritionPlanService.UpdateNutritionPlanAsync(nutritionPlanDTO, id);

			return updatedNutritionPlan != null ?
					   Ok(updatedNutritionPlan) :
					   NotFound($"Klarte ikke å oppdatere bruker med ID: {id}");
		}

		/// <summary>
		/// Sletter en ernæringsplan.
		/// </summary>
		/// <param name="id">ID-en til ernæringsplanen som skal slettes.</param>
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

		/// <summary>
		/// Oppretter en ny ernæringsplan.
		/// </summary>
		/// <param name="nutritionPlan">Informasjon om den nye ernæringsplanen.</param>
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
