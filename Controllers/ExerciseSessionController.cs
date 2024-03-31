using FitnessPartner.Models.DTOs;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ExerciseSessionController : ControllerBase
	{
		private readonly IExerciseSessionService _exersiceSessionService;
		private readonly ILogger<ExerciseSessionController> _logger;

		public ExerciseSessionController(IExerciseSessionService exersiceSessionService, ILogger<ExerciseSessionController> logger)
		{
			_exersiceSessionService = exersiceSessionService;
			_logger = logger;
		}



		// GET: api/<ExerciseSessionController>
		[HttpGet(Name = "GetAllExerciseSession")]
		public async Task<ActionResult<IEnumerable<ExerciseSessionDTO>>> GetAllSessions(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _exersiceSessionService.GetAllSessionsAsync(pageNr, pageSize));
		}

		// GET api/<ExerciseSessionController>/5
		[HttpGet("{Id}", Name = "GetExerciseSessionById")]
		public async Task<ActionResult<ExerciseSessionDTO>> GetExerciseById(int exerciseId)
		{
			var ExerciseSesId = await _exersiceSessionService.GetSessionByIdAsync(exerciseId);
			return exerciseId != 0 ? Ok(ExerciseSesId) : NotFound();
		}

		// POST api/<ExerciseSessionController>
		[HttpPost]
		public async Task<ActionResult<ExerciseSessionDTO>> PostExercise([FromBody] ExerciseSessionDTO exerciseSesDTO)
		{
			try
			{
				if (exerciseSesDTO == null)
				{
					return BadRequest("Ugyldige exercise session data");
				}

				int loginUserId = (int)HttpContext.Items["UserId"]!;
				var addedExerciseSes = await _exersiceSessionService.AddSessionAsync(exerciseSesDTO, loginUserId);

				if (addedExerciseSes != null)
				{

					return Ok(addedExerciseSes);
				}

				return BadRequest("Feil ved opprettelse av exercise session");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Intern feil: {ex.Message}");
			}
		}

		// PUT api/<ExerciseSessionController>/5
		[HttpPut(Name = "UpdateExerciseSession")]
		public async Task<ActionResult<ExerciseSessionDTO>> UpdateExerciseSession(int exercisesesId, ExerciseSessionDTO exerciseSesLibraryDTO)
		{
			int loginMemberId = (int)HttpContext.Items["UserId"]!;

			var updatedExerciseSes = await _exersiceSessionService.UpdateSessionAsync(exerciseSesLibraryDTO, loginMemberId, exercisesesId);

			if (updatedExerciseSes != null)
			{
				return Ok(updatedExerciseSes);
			}
			return NotFound($"Exercise Session med ID {exercisesesId} ble ikke funnet");
		}

		// DELETE api/<ExerciseSessionController>/5
		[HttpDelete("{id}", Name = "DeleteExersiceSession")]
		public async Task<ActionResult<ExerciseSessionDTO>> DeleteExercise(int exerciseSesID)
		{
			int loginMemberId = (HttpContext.Items["UserId"] as int?) ?? 0;

			var deletedExerciseSes = await _exersiceSessionService.DeleteSessionByIdAsync(exerciseSesID, loginMemberId);

			if (deletedExerciseSes != null)
			{
				return Ok($"Exercise session med ID {exerciseSesID} ble slettet vellykket");
			}
			return NotFound($"Exercise session med ID {exerciseSesID} ble ikke funnet");
		}

	}
}
