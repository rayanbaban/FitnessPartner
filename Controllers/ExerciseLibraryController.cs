using FitnessPartner.Models.DTOs;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPartner.Controllers
{
	/// <summary>
	/// Kontrolleren for øvelsesbiblioteket.
	/// </summary>
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ExerciseLibraryController : ControllerBase
	{
		private readonly IExerciseLibraryService _exersiceLibraryService;
		private readonly ILogger<ExerciseLibraryController> _logger;

		public ExerciseLibraryController(IExerciseLibraryService exersiceLibraryService, ILogger<ExerciseLibraryController> logger)
		{
			_exersiceLibraryService = exersiceLibraryService;
			_logger = logger;
		}

		/// <summary>
		/// Henter alle øvelser fra øvelsesbiblioteket.
		/// </summary>
		/// <param name="pageNr">Sidenummer for paginering.</param>
		/// <param name="pageSize">Antall øvelser per side.</param>
		[HttpGet(Name = "GetAllLibraryExercises")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<IEnumerable<ExerciseLibraryDTO>>> GetAllExercises(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _exersiceLibraryService.GetAllExerciesAsync(pageNr, pageSize));
		}

		/// <summary>
		/// Henter en øvelse fra øvelsesbiblioteket basert på ID.
		/// </summary>
		/// <param name="exerciseId">ID-en til øvelsen.</param>
		[HttpGet]
		[Route("getById")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<ExerciseLibraryDTO>> GetExerciseById(int exerciseId)
		{
			var Exercise = await _exersiceLibraryService.GetExerciseByIdAsync(exerciseId);
			return exerciseId != 0 ? Ok(Exercise) : NotFound();
		}

		[HttpGet]
		[Route("getByName")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<ExerciseLibraryDTO>> GetExerciseByName(string name)
		{
			var Exercise = await _exersiceLibraryService.GetExerciseByNameAsync(name);
			return name != null ? Ok(Exercise) : NotFound();
		}

		/// <summary>
		/// Legger til en ny øvelse i øvelsesbiblioteket.
		/// </summary>
		/// <param name="exerciselibraryDTO">Informasjon om den nye øvelsen.</param>
		[HttpPost]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<ExerciseLibraryDTO>> PostExercise([FromBody] ExerciseLibraryDTO exerciselibraryDTO)
		{
			try
			{
				if (exerciselibraryDTO == null)
				{
					return BadRequest("Ugyldige exercise data");
				}

				var addedEvent = await _exersiceLibraryService.AddExerciseLibraryAsync(exerciselibraryDTO);

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

		/// <summary>
		/// Oppdaterer en øvelse i øvelsesbiblioteket.
		/// </summary>
		/// <param name="id">ID-en til øvelsen som skal oppdateres.</param>
		/// <param name="exerciseLibraryDTO">Oppdatert informasjon om øvelsen.</param>
		[HttpPut(Name = "UpdateExerciseLibrary")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<ExerciseLibraryDTO>> UpdateExercise(int id, ExerciseLibraryDTO exerciseLibraryDTO)
		{
			var updatedPost = await _exersiceLibraryService.UpdateExerciseAsync(id, exerciseLibraryDTO);

			if (updatedPost != null)
			{
				return Ok(updatedPost);
			}
			return NotFound($"Exercise med ID {id} ble ikke funnet");
		}


		/// <summary>
		/// Sletter en øvelse fra øvelsesbiblioteket.
		/// </summary>
		/// <param name="exerciseID">ID-en til øvelsen som skal slettes.</param>
		[HttpDelete(Name = "DeleteExersiceLibrary")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<ExerciseLibraryDTO>> DeleteExercise(int exerciseID)
		{
			var deletedexercise = await _exersiceLibraryService.DeleteExerciseAsync(exerciseID);

			if (deletedexercise != null)
			{
				return Ok($"Exercise med ID {exerciseID} ble slettet vellykket");
			}
			return NotFound($"Exercise med ID {exerciseID} ble ikke funnet");
		}
	}
}
