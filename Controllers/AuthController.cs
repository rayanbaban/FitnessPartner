using FitnessPartner.Models;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.OtherObjects;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessPartner.Controllers
{

	[Route("api/v1/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		// route for seeding roles to database

		[HttpPost]
		[Route("seed-roles")]
		public async Task<ActionResult> SeedRoles()
		{
			var seedRoles = await _authService.SeedRolesAsync();

			return Ok(seedRoles);
		}

		[HttpPost]
		[Route("register")]
		public async Task<ActionResult> Register([FromBody] UserRegDTO registerDTO)
		{
			var registerResult = await _authService.RegisterAsync(registerDTO);

			if(registerResult.IsSucceed)
				return Ok(registerResult);
			return BadRequest(registerResult);
			
		}

		[HttpPost]
		[Route("login")]
		public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
		{
			var loginResult = await _authService.LoginAsync(loginDTO);

			if (loginResult.IsSucceed)
				return Ok(loginResult);
            await Console.Out.WriteLineAsync($"{loginResult}");

            return Unauthorized(loginResult);
		}



		[HttpPost]
		[Route ("MakeAdmin")]
		public async Task<ActionResult> MakeAdmin([FromBody] UpdatePermissionDTO upadtePermission)
		{
			var operationResult = await _authService.MakeAdminAsync(upadtePermission);

			if (operationResult.IsSucceed)
				return Ok(operationResult);
			return BadRequest(operationResult);
		}

		
	}
}
