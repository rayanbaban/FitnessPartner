using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessPartner.Controllers
{
	[Route("api/v1[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly IUserService _userService;
		public static User user = new User();
		private readonly IConfiguration _configuration;
		private readonly ILogger<AuthController> _logger;

		public AuthController(IUserService userService, IConfiguration configuration, ILogger<AuthController> logger)
		{
			_userService = userService;
			_configuration = configuration;
			_logger = logger;
		}

		[HttpPost("register")]
		public async Task<ActionResult<UserDTO>> Register(UserRegDTO userRegDTO)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var userDTO = await _userService.RegisterUserAsync(userRegDTO);

			return userDTO != null ? Ok(userDTO) : BadRequest("Klarte ikke registrere en ny bruker");


		}

		[HttpPost("login")]
		public ActionResult<User> Login(UserDTO request)
		{                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
			if(user.UserName != request.UserName)
			{
				return BadRequest("User not found");
			}

			if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
			{
				return BadRequest("Wrong password.");
			}
			string token = CreateToken(user);

			return Ok(token);
		}

		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
				(_configuration.GetSection("AppSettings:Token").Value!));
			
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds
				);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
