using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
	public interface IAuthService
	{
		Task<AuthServiceResponseDTO> SeedRolesAsync();
		Task<AuthServiceResponseDTO> RegisterAsync(UserRegDTO registerDTO);
		Task<AuthServiceResponseDTO> LoginAsync(LoginDTO loginDTO);
		Task<AuthServiceResponseDTO> MakeAdminAsync(UpdatePermissionDTO updatePermissionDTO);
	}
}
