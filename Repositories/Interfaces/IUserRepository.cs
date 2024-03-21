using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(int id, User user);
    Task<User> DeleteUserByIdAsync(int id);
    Task<User?> GetUserByIdAsync(int userId);
    Task<ICollection<User>> GetAllUsersAsync();

}
