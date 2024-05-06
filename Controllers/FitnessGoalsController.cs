using FitnessPartner.Models.DTOs;
using FitnessPartner.Services.Interfaces;
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
		public async Task<ActionResult<IEnumerable<FitnessGoalsDTO>>> GetFitnessGoals(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _fitnessGoalsService.GetMyFitnessGoalsAsync(pageNr, pageSize));
		}

		/// <summary>
		/// Henter et treningsmål basert på ID.
		/// </summary>
		/// <param name="goalId">ID-en til treningsmålet.</param>
		[HttpGet]
		[Route("Id")]
		public async Task<ActionResult<FitnessGoalsDTO>> GetFitnessGoalById(int goalId)
		{
			var result = await _fitnessGoalsService.GetFitnessGoalByIdAsync(goalId);
			return goalId != 0 ? Ok(result) : NotFound();
		}

		/// <summary>
		/// Legger til et nytt treningsmål.
		/// </summary>
		/// <param name="fitnessgoalsDTO">Informasjon om det nye treningsmålet.</param>
		[HttpPost]
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
		public async Task<ActionResult<FitnessGoalsDTO>> UpdateFitnessGoal(int goalId, FitnessGoalsDTO fitnessGoalsDTO)
		{
			var updatedFitnessGoal = await _fitnessGoalsService.UpdateFitnessGoalAsync(fitnessGoalsDTO, goalId);

			if (updatedFitnessGoal != null)
			{
				return Ok(updatedFitnessGoal);
			}
			return NotFound($"Fitness goal med goalId {goalId} ble ikke funnet");
		}
	}
}
