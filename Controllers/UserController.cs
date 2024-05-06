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
	/// Kontrolleren for brukerhandlinger.
	/// </summary>
	[Route("api/v1/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UserController : ControllerBase
	{
		private readonly IUserService _usersService;
		private readonly ILogger<UserController> _logger;

		public UserController(IUserService usersService, ILogger<UserController> logger)
		{
			_usersService = usersService;
			_logger = logger;
		}

		/// <summary>
		/// Henter alle brukere.
		/// </summary>
		/// <param name="pageNr">Sidenummer for paginering.</param>
		/// <param name="pageSize">Antall brukere per side.</param>
		[HttpGet(Name = "GetAllUsers")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<ICollection<UserDTO>>> GetAllUsersAsync(int pageNr = 1, int pageSize = 10)
		{
			return Ok(await _usersService.GetPageAsync(pageNr, pageSize));
		}

		/// <summary>
		/// Henter en bruker basert på ID.
		/// </summary>
		/// <param name="id">ID-en til brukeren.</param>
		[HttpGet("{id}", Name = "GetUserById")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int id)
		{
			var res = await _usersService.GetUserByIdAsync(id);
			return res != null ? Ok(res) : NotFound("Fant Ikke Bruker");
		}

		/// <summary>
		/// Oppdaterer en bruker.
		/// </summary>
		/// <param name="id">ID-en til brukeren som skal oppdateres.</param>
		/// <param name="userDTO">Oppdatert informasjon om brukeren.</param>
		[HttpPut("{id}", Name = "UpdateUser")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<UserDTO>> UpdateUserAsync(int id, UserDTO userDTO)
		{
			var updatedUser = await _usersService.UpdateUserAsync(id, userDTO);

			return updatedUser != null ?
					   Ok(updatedUser) :
					   NotFound($"Klarte ikke finne bruker med ID: {id}");
		}

		/// <summary>
		/// Sletter en bruker.
		/// </summary>
		/// <param name="id">ID-en til brukeren som skal slettes.</param>
		[HttpDelete(Name = "DeleteUser")]
		[Authorize(Roles = StaticUserRoles.USER)]
		public async Task<ActionResult<UserDTO>> DeleteUserAsync(int id)
		{
			try
			{
				var userToDelete = await _usersService.GetUserByIdAsync(id);

				var deletedUser = await _usersService.DeleteUserAsync(id);
				_logger?.LogInformation("Deleted user: {@DeletedUser}", deletedUser);

				return deletedUser != null ?
						   deletedUser :
						   throw new Exception($"Klarte ikke å slette bruker med ID: {id}");
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Feil ved sletting av bruker");
				throw new Exception($"Feil ved sletting av bruker: {ex.Message}");
			}
		}
	}
}
