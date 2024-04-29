using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace FitnessPartner.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signinManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;


		public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<AppUser> signinManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_signinManager = signinManager;
		}
		public async Task<AuthServiceResponseDTO> LoginAsync(LoginDTO loginDTO)
		{

			var user = await _userManager.FindByNameAsync(loginDTO.UserName);

			if (user is null)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Message = "Could not find the given Username"
				};

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
			if (!isPasswordCorrect)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Message = "Password is not correct"
				};

			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName!),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
				Message = token
			};
		}

		public async Task<AuthServiceResponseDTO> MakeAdminAsync(UpdatePermissionDTO updatePermissionDTO)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDTO.UserName);

			if (user is null)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Message = "Invalid Username"
				};

			await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

			return new AuthServiceResponseDTO()
			{
				IsSucceed = true,
				Message = "User is now admin"
			};
		}

		public async Task<AuthServiceResponseDTO> RegisterAsync(UserRegDTO registerDTO)
		{
			var isExistsUser = await _userManager.FindByNameAsync(registerDTO.UserName);
			int appuserid = 0;
			appuserid++;
			if (isExistsUser != null)
				return new AuthServiceResponseDTO()
				{
					IsSucceed = false,
					Message = "Username already exists"
				};

			AppUser newUser = new AppUser()
			{
				AppUserId = appuserid,
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
				PasswordHash = registerDTO.Password
			};

			var createdUserResult = await _userManager.CreateAsync(newUser, newUser.PasswordHash);

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
					Message = errorString
				};
			}
			await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

			return  new AuthServiceResponseDTO()
			{
				IsSucceed = true,
				Message = "User created successfully"
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
					Message = "Roles Seeding is already done"
				};

			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));

			return new AuthServiceResponseDTO()
			{
				IsSucceed = true,
				Message = "Role Seeding done successfully"
			};
		}


		private string GenerateNewJsonWebToken(List<Claim> claims)
		{
			var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

			//var tokenObject = new JwtSecurityToken(
			//	issuer: _configuration["JWT:ValidIssuer"],
			//	audience: _configuration["JWT:ValidAudience"],
			//	expires: DateTime.Now.AddHours(1),
			//	claims: claims,
			//	signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256));

			//string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
   //         return token;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(10),
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                SigningCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);
            return jwt;
        }
	}
}
