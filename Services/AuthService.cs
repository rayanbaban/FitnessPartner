using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessPartner.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}
		public async Task<AuthServiceResponseDTO> LoginAsync(LoginDTO loginDTO)
		{
			var user = await _userManager.FindByNameAsync(loginDTO.UserName);

			if (user is null)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Mesage = "Could not find the given Username"
				};

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
			if (!isPasswordCorrect)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Mesage = "Password is not correct"
				};

			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim("JWTID", Guid.NewGuid().ToString()),
				//new Claim("FirstName", user.FirstName),
				//new Claim("LastName", user.LastName)
			};


			foreach (var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var token = GenerateNewJsonWebToken(authClaims);

			return new AuthServiceResponseDTO()
			{
				IsSucceed = true,
				Mesage = token
			};
		}

		public async Task<AuthServiceResponseDTO> MakeAdminAsync(UpdatePermissionDTO updatePermissionDTO)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDTO.UserName);

			if (user is null)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Mesage = "Invalid Username"
				};

			await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

			return new AuthServiceResponseDTO()
			{
				IsSucceed = true,
				Mesage = "User is now admin"
			};
		}

		public async Task<AuthServiceResponseDTO> RegisterAsync(UserRegDTO registerDTO)
		{
			var isExistsUser = await _userManager.FindByNameAsync(registerDTO.UserName);

			if (isExistsUser != null)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Mesage = "Username already exists"
				};

			AppUser newUser = new AppUser()
			{
				AppUserName = registerDTO.UserName,
				AppUserEmail = registerDTO.Email,
				FirstName = registerDTO.FirstName,
				LastName = registerDTO.LastName,
				Weight = registerDTO.Weight,
				Height = registerDTO.Height,
				Age = registerDTO.Age,
				Email = registerDTO.Email,
				UserName = registerDTO.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			var createdUserResult = await _userManager.CreateAsync(newUser, registerDTO.Password);

			if (!createdUserResult.Succeeded)
			{
				var errorString = "User creation failed because of: ";
				foreach (var error in createdUserResult.Errors)
				{
					errorString += " # " + error.Description;
				}
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Mesage = errorString
				};
			}
			await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

			return  new AuthServiceResponseDTO()
			{
				IsSucceed = true,
				Mesage = "User created successfully"
			};
		}

		public async Task<AuthServiceResponseDTO> SeedRolesAsync()
		{
			bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
			bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

			if (isAdminRoleExists && isUserRoleExists)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = true,
					Mesage = "Roles Seeding is already done"
				};

			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));

			return new AuthServiceResponseDTO()
			{
				IsSucceed = true,
				Mesage = "Role Seeding done successfully"
			};
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
