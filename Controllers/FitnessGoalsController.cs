using FitnessPartner.Models.DTOs;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPartner.Controllers
{
	/// <summary>
	/// Kontrolleren for treningsmål.
	/// </summary>
	[Route("api/v1/[controller]")]
	[ApiController]
	public class FitnessGoalsController : ControllerBase
	{
		private readonly ILogger<FitnessGoalsController> _logger;
		private readonly IFitnessGoalsService _fitnessGoalsService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public FitnessGoalsController(ILogger<FitnessGoalsController> logger, IFitnessGoalsService fitnessGoalsService, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_fitnessGoalsService = fitnessGoalsService;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Henter alle treningsmål.
		/// </summary>
		/// <param name="pageNr">Sidenummer for paginering.</param>
		/// <param name="pageSize">Antall mål per side.</param>
		[HttpGet(Name = "GetFitnessGoals")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]

		public async Task<ActionResult<IEnumerable<FitnessGoalsDTO>>> GetFitnessGoals(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _fitnessGoalsService.GetAllFitnessGoalsAsync(pageNr, pageSize));
		}

		


		[HttpGet("GetMyGoals", Name = "GetMyGoals")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<ExerciseSessionDTO>> GetGoalsByUserAsync(int pageNr = 1, int pageSize = 10)
		{
			var userId = HttpContext.Items["UserId"] ?? string.Empty;
			if (userId is null)
			{
				throw new UnauthorizedAccessException("");
			}

			var getSessions = await _fitnessGoalsService.GetMyFitnessGoalsAsync(userId.ToString()!, pageNr, pageSize);

			if (getSessions == null) return NotFound($"Exercise sessions for bruker {userId} ble ikke funnet");
			return Ok(getSessions);
		}

		/// <summary>
		/// Legger til et nytt treningsmål.
		/// </summary>
		/// <param name="fitnessgoalsDTO">Informasjon om det nye treningsmålet.</param>
		[HttpPost]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<FitnessGoalsDTO>> PostFitnessGoals([FromBody] FitnessGoalsDTO fitnessgoalsDTO)
		{
			try
			{
				if (fitnessgoalsDTO == null)
				{
					return BadRequest("Ugyldige fitness goal data");
				}

				var addedFitnessGoal = await _fitnessGoalsService.CreateFitnessGoalAsync(fitnessgoalsDTO);

				if (addedFitnessGoal != null)
				{
					return Ok(addedFitnessGoal);
				}

				return BadRequest("Feil ved forsøk på å legge til fitness goal");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Intern feil: {ex.Message}");
			}
		}

		/// <summary>
		/// Oppdaterer et treningsmål.
		/// </summary>
		/// <param name="goalId">ID-en til treningsmålet som skal oppdateres.</param>
		/// <param name="fitnessGoalsDTO">Oppdatert informasjon om treningsmålet.</param>
		[HttpPut(Name = "UpdateFitnessGoal")]
		[Authorize(Roles = StaticUserRoles.USER)]

		public async Task<ActionResult<FitnessGoalsDTO>> UpdateFitnessGoal(int goalId, FitnessGoalsDTO fitnessGoalsDTO)
		{
			var updatedFitnessGoal = await _fitnessGoalsService.UpdateFitnessGoalAsync(fitnessGoalsDTO, goalId);

			if (updatedFitnessGoal != null)
			{
				return Ok(updatedFitnessGoal);
			}
			return NotFound($"Fitness goal med goalId {goalId} ble ikke funnet");
		}

		/// <summary>
		/// Sletter et treningsmål.
		/// </summary>
		/// <param name="id">ID-en til treningsmålet som skal slettes.</param>
		[HttpDelete(Name = "DeleteFitnessGoal")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<NutritionPlansDTO>> DeleteFitnessGoal(int id)
		{
			var goalToDelete = await _fitnessGoalsService.DeleteFitnessGoalAsync(id);

			if (goalToDelete != null)
			{
				return Ok($"Goal med ID {id} ble slettet vellykket");
			}
			return NotFound($"Goal med ID {id} ble ikke funnet");
		}
	}
}
