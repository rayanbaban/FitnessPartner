using FitnessPartner.Models.DTOs;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPartner.Controllers
{
	/// <summary>
	/// Kontrolleren for ernæringslogger.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class NutritionLogController : ControllerBase
	{
		private readonly INutritionLogService _nutritionLogService;
		private readonly ILogger<NutritionLogController> _logger;

		public NutritionLogController(INutritionLogService nutritionLogService, ILogger<NutritionLogController> logger)
		{
			_nutritionLogService = nutritionLogService;
			_logger = logger;
		}

		/// <summary>
		/// Henter alle ernæringslogger.
		/// </summary>
		/// <param name="pageNr">Sidenummer for paginering.</param>
		/// <param name="pageSize">Antall logger per side.</param>
		[HttpGet(Name = "GetAllNutritionLogs")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<ICollection<NutritionLogDTO>>> GetAllNutritionLogsAsync(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _nutritionLogService.GetPageAsync(pageNr, pageSize));
		}

		/// <summary>
		/// Henter en ernæringslogg basert på ID.
		/// </summary>
		/// <param name="id">ID-en til ernæringsloggen.</param>
		[HttpGet("{id}", Name = "GetNutritionLogById")]
		[Authorize(Roles = StaticUserRoles.USER)]

		public async Task<ActionResult<NutritionLogDTO>> GetNutritionLogById(int id)
		{
			var res = await _nutritionLogService.GetNutritionLogByIdAsync(id);
			return res != null ? Ok(res) : NotFound("Fant ikke NutritionLog");
		}

		/// <summary>
		/// Oppdaterer en ernæringslogg.
		/// </summary>
		/// <param name="id">ID-en til ernæringsloggen som skal oppdateres.</param>
		/// <param name="nutritionLogDTO">Oppdatert informasjon om ernæringsloggen.</param>
		[HttpPut("{id}", Name = "UpdateNutritionLog")]
		[Authorize(Roles = StaticUserRoles.USER)]

		public async Task<ActionResult<NutritionLogDTO>> UpdateNutritionLogAsync(int id, NutritionLogDTO nutritionLogDTO)
		{
			var updatedNutritionLog = await _nutritionLogService.UpdateNutritionLogAsync(nutritionLogDTO, id);

			return updatedNutritionLog != null ?
					   Ok(updatedNutritionLog) :
					   NotFound($"Fant ikke nutrition logg med ID: {id}");
		}

		/// <summary>
		/// Sletter en ernæringslogg.
		/// </summary>
		/// <param name="nutritionLogId">ID-en til ernæringsloggen som skal slettes.</param>
		[HttpDelete(Name = "DeleteNutritionLog")]
		[Authorize(Roles = StaticUserRoles.USER)]

		public async Task<ActionResult<NutritionLogDTO>> DeleteNutritionLog(int nutritionLogId)
		{
			var deletedNutritionLog = await _nutritionLogService.DeleteNutritionLogAsync(nutritionLogId);

			if (deletedNutritionLog != null)
			{
				return Ok($"Exercise med ID {nutritionLogId} ble slettet vellykket");
			}

			return NotFound($"Exercise med ID {nutritionLogId} ble ikke funnet");
		}

		/// <summary>
		/// Oppretter en ny ernæringslogg.
		/// </summary>
		/// <param name="nutritionLog">Informasjon om den nye ernæringsloggen.</param>
		[HttpPost(Name = "CreateNutritionLog")]
		[Authorize(Roles = StaticUserRoles.USER)]

		public async Task<ActionResult<NutritionLogDTO>> PostNutritionLog([FromBody] NutritionLogDTO nutritionLog)
		{
			try
			{
				if (nutritionLog == null)
				{
					return BadRequest("Ugyldige nutritionLog data");
				}

				var addedNutritionLog = await _nutritionLogService.CreateNutritionLogAsync(nutritionLog);

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
