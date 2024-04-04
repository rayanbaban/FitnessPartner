using FitnessPartner.Models;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Services.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserDTO>> GetAllUsersAsync();

        Task<UserDTO> UpdateUserAsync(int id, UserDTO userDTO, int inloggedUser);

        Task<UserDTO?> DeleteUserAsync(int id, int loginUserId);

        Task<UserDTO?> GetUserByIdAsync(int userId);
        //Task<UserDTO?> GetAuthenticatedIdAsync(string userName, string password);
        Task<UserLoginDTO?> GetAuthenticatedIdAsync(string userName, string password);

        Task<UserDTO> RegisterUserAsync(UserRegDTO userRegDTO);

        Task<IEnumerable<UserDTO>> GetPageAsync(int pageNr, int pageSize);
		


	}
}
