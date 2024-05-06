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
	/// Kontrolleren for øvelsessesjoner.
	/// </summary>
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

		/// <summary>
		/// Henter alle øvelsessesjoner.
		/// </summary>
		/// <param name="pageNr">Sidenummer for paginering.</param>
		/// <param name="pageSize">Antall øvelser per side.</param>
		[HttpGet(Name = "GetAllExerciseSession")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<IEnumerable<ExerciseSessionDTO>>> GetAllSessions(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _exersiceSessionService.GetAllSessionsAsync(pageNr, pageSize));
		}

		/// <summary>
		/// Henter en øvelsessesjon basert på ID.
		/// </summary>
		/// <param name="exerciseId">ID-en til øvelsessesjonen.</param>
		[HttpGet]
		[Route("GetById")]
		public async Task<ActionResult<ExerciseSessionDTO>> GetExerciseById(int exerciseId)
		{
			var ExerciseSesId = await _exersiceSessionService.GetSessionByIdAsync(exerciseId);
			return exerciseId != 0 ? Ok(ExerciseSesId) : NotFound();
		}

		/// <summary>
		/// Legger til en ny øvelsessesjon.
		/// </summary>
		/// <param name="exerciseSesDTO">Informasjon om den nye øvelsessesjonen.</param>
		[HttpPost]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<ExerciseSessionDTO>> PostExercise([FromBody] ExerciseSessionDTO exerciseSesDTO)
		{
			try
			{
				if (exerciseSesDTO == null)
				{
					return BadRequest("Ugyldige exercise session data");
				}

				var addedExerciseSes = await _exersiceSessionService.AddSessionAsync(exerciseSesDTO);

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

		/// <summary>
		/// Oppdaterer en øvelsessesjon.
		/// </summary>
		/// <param name="exercisesesId">ID-en til øvelsessesjonen som skal oppdateres.</param>
		/// <param name="exerciseSesLibraryDTO">Oppdatert informasjon om øvelsessesjonen.</param>
		[HttpPut(Name = "UpdateExerciseSession")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<ExerciseSessionDTO>> UpdateExerciseSession(int exercisesesId, ExerciseSessionDTO exerciseSesLibraryDTO)
		{
			var updatedExerciseSes = await _exersiceSessionService.UpdateSessionAsync(exerciseSesLibraryDTO, exercisesesId);

			if (updatedExerciseSes != null)
			{
				return Ok(updatedExerciseSes);
			}
			return NotFound($"Exercise Session med ID {exercisesesId} ble ikke funnet");
		}

		/// <summary>
		/// Sletter en øvelsessesjon.
		/// </summary>
		/// <param name="exerciseSesID">ID-en til øvelsessesjonen som skal slettes.</param>
		[HttpDelete(Name = "DeleteExersiceSession")]
		[Authorize(Roles = StaticUserRoles.USER)]

		public async Task<ActionResult<ExerciseSessionDTO>> DeleteExercise(int exerciseSesID)
		{
			var deletedExerciseSes = await _exersiceSessionService.DeleteSessionByIdAsync(exerciseSesID);

			if (deletedExerciseSes != null)
			{
				return Ok($"Exercise session med ID {exerciseSesID} ble slettet vellykket");
			}
			return NotFound($"Exercise session med ID {exerciseSesID} ble ikke funnet");
		}


		[HttpGet ("GetSessionsByUsers",Name = "GetSessionsByUserId")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<ExerciseSessionDTO>> GetSesionsByUserAsync(string userId, int pageNr = 1, int pageSize = 10)
		{
			var getSessions = await _exersiceSessionService.GetSessionsByUserIdAsync(userId, pageNr, pageSize);

			if (getSessions == null) return NotFound($"Exercise session med ID {userId} ble ikke funnet");
			return Ok(getSessions);
		}
	}
}
