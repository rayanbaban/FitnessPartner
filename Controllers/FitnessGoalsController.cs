using FitnessPartner.Models.DTOs;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace FitnessPartner.Controllers
{
	[Route("api/v1[controller]")]
	[ApiController]
	public class FitnessGoalsController : ControllerBase
	{
		private readonly ILogger<FitnessGoalsController> _logger;
		private readonly IFitnessGoalsService _fitnessGoalsService;

		public FitnessGoalsController(ILogger<FitnessGoalsController> logger, IFitnessGoalsService fitnessGoalsService)
		{
			_logger = logger;
			_fitnessGoalsService = fitnessGoalsService;
		}

		[HttpGet(Name = "GetFitnessGoals")]
		public async Task<ActionResult<IEnumerable<FitnessGoalsDTO>>> GetFitnessGoals(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _fitnessGoalsService.GetMyFitnessGoalsAsync(pageNr, pageSize));
		}

		[HttpGet("{id}", Name = "GetFitnessGoalById")]
		public async Task<ActionResult<FitnessGoalsDTO>> GetFitnessGoalById(int goalId)
		{
			var result = await _fitnessGoalsService.GetFitnessGoalByIdAsync(goalId);
			return goalId != 0 ? Ok(result) : NotFound();
		}

		[HttpPost]
		public async Task<ActionResult<FitnessGoalsDTO>> PostFitnessGoals([FromBody] FitnessGoalsDTO fitnessgoalsDTO)
		{
			try
			{
				if (fitnessgoalsDTO == null)
				{
					return BadRequest("Ugyldige fitness goal data");
				}

				int loginUserId = (int)HttpContext.Items["UserId"]!;
				var addedFitnessGoal = await _fitnessGoalsService.CreateFitnessGoalAsync(fitnessgoalsDTO, loginUserId);

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

		[HttpPut(Name = "UpdateFitnessGoal")]
		public async Task<ActionResult<FitnessGoalsDTO>> UpdateExerciseSession(int goalId, FitnessGoalsDTO fitnessGoalsDTO)
		{
			int loginMemberId = (int)HttpContext.Items["UserId"]!;

			var updatedFitnessGoal = await _fitnessGoalsService.UpdateFitnessGoalAsync(fitnessGoalsDTO, loginMemberId, goalId);

			if (updatedFitnessGoal != null)
			{
				return Ok(updatedFitnessGoal);
			}
			return NotFound($"Fitness goal med goalId {goalId} ble ikke funnet");
		}
	}
}
