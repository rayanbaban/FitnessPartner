using FitnessPartner.Models.DTOs;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	[Authorize]
	public class ExerciseLibraryController : ControllerBase
	{
		private readonly IExerciseLibraryService _exersiceLibraryService;
		private readonly ILogger<ExerciseLibraryController> _logger;

		public ExerciseLibraryController(IExerciseLibraryService exersiceLibraryService, ILogger<ExerciseLibraryController> logger)
		{
			_exersiceLibraryService = exersiceLibraryService;
			_logger = logger;
		}



		// GET: api/<ExercisesLibraryController>
		[HttpGet(Name = "GetAllLibraryExercises")]
		//[Authorize(Roles = "Admin")]
		public async Task<ActionResult<IEnumerable<ExerciseLibraryDTO>>> GetAllExercises(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _exersiceLibraryService.GetAllExerciesAsync(pageNr, pageSize));
		}

		// GET api/<ExerciseLibraryController>/5
		[HttpGet("{Id}", Name = "GetExercisLibraryeById")]
		public async Task<ActionResult<ExerciseLibraryDTO>> GetExerciseById(int Id)
		{
			var Exercise = await _exersiceLibraryService.GetExerciseByIdAsync(Id);
			return Id != 0 ? Ok(Exercise) : NotFound();
		}

		// POST api/<ExerciseLibraryController>
		[HttpPost]
		public async Task<ActionResult<ExerciseLibraryDTO>> PostExercise([FromBody] ExerciseLibraryDTO exerciselibraryDTO)
		{
			try
			{
				if (exerciselibraryDTO == null)
				{
					return BadRequest("Ugyldige exercise data");
				}

				//int loginUserId = (int)HttpContext.Items["UserId"]!;
				var addedEvent = await _exersiceLibraryService.AddExerciseLibraryAsync(/*loginUserId,*/ exerciselibraryDTO);

				if (addedEvent != null)
				{
					
					return Ok(addedEvent);
				}

				return BadRequest("Feil ved opprettelse av exercise");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Intern feil: {ex.Message}");
			}
		}

		// PUT api/<ExerciseLibraryController>/5
		[HttpPut("{id}", Name = "UpdateExerciseLibrary")]
		public async Task<ActionResult<ExerciseLibraryDTO>> UpdateExercise(int id, ExerciseLibraryDTO exerciseLibraryDTO)
		{
			//int loginMemberId = (int)HttpContext.Items["UserId"]!;

			var updatedPost = await _exersiceLibraryService.UpdateExerciseAsync(id, /*loginMemberId,*/ exerciseLibraryDTO);

			if (updatedPost != null)
			{
				return  Ok(updatedPost);
			}
			return NotFound($"Exercise med ID {id} ble ikke funnet");
		}

		// DELETE api/<ExerciseLibraryController>/5
		[HttpDelete("{id}", Name = "DeleteExersiceLibrary")]
		public async Task<ActionResult<ExerciseLibraryDTO>> DeleteExercise(int exerciseID)
		{
			int loginMemberId = (HttpContext.Items["UserId"] as int?) ?? 0;

			var deletedexercise = await _exersiceLibraryService.DeleteExerciseAsync(exerciseID, loginMemberId);

			if (deletedexercise != null)
			{
				return Ok($"Exercise med ID {exerciseID} ble slettet vellykket");
			}
			return NotFound($"Exercise med ID {exerciseID} ble ikke funnet");
		}
	}
}
