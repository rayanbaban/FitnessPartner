using FitnessPartner.Models;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.OtherObjects;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}


		// route for seeding roles to database

		[HttpPost]
		[Route("seed-roles")]
		public async Task<ActionResult> SeedRoles()
		{

			bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
			bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

			if (isAdminRoleExists && isUserRoleExists)
				return Ok("Roles Seeding Is Already Done");

			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));

			return Ok("Role Seeding Done Successfully");
		}

		[HttpPost]
		[Route("register")]
		public async Task<ActionResult> Register([FromBody] UserRegDTO registerDTO)
		{
			var isExistsUser = await _userManager.FindByNameAsync(registerDTO.UserName);

			if (isExistsUser != null )
				return BadRequest("Username already exists. try another one");

			IdentityUser newUser = new IdentityUser()
			{
				Email = registerDTO.Email,
				UserName = registerDTO.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			var createdUserResult = await _userManager.CreateAsync(newUser, registerDTO.Password);

			if (!createdUserResult.Succeeded)
			{
				var errorString = "User creation failed because of: ";
				foreach(var error in createdUserResult.Errors)
				{
					errorString += " # " + error.Description;
				}
				return BadRequest(errorString);
			}
			await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

			return Ok("User Created Succesfully");
		}

		[HttpPost]
		[Route("login")]
		public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
		{
			var user = await _userManager.FindByNameAsync(loginDTO.UserName);

			if (user is null)
				return Unauthorized("Could not find User");

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
			 if(!isPasswordCorrect)
				return Unauthorized("Wrong password or username");

			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim("JWTID", Guid.NewGuid().ToString()),
			};


			foreach(var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var token = GenerateNewJsonWebToken(authClaims);

			return Ok(token);
		}

		private string GenerateNewJsonWebToken(List<Claim> claims)
		{
			var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var tokenObject = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(1),
				claims: claims,
				signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256));

			string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

			return token;
		}
	}
}

//ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//        ValidAudience = builder.Configuration["JWT:ValidAudience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))