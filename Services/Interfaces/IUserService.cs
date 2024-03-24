using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
    public interface IUserService
    {
        Task<ICollection<UserDTO>> GetAllUsersAsync();

        Task<UserDTO> UpdateUserAsync(int id, UserDTO userDTO, int inloggedUser);

        Task<UserDTO?> DeleteUserAsync(int id, int loginUserId);

        Task<UserDTO?> GetUserByIdAsync(int userId);
        Task<int> GetAuthenticatedIdAsync(string userName, string password);
    }
}
