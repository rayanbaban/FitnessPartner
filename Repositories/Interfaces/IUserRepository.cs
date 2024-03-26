using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(int id, User user);
    Task<User> DeleteUserByIdAsync(int id);
    Task<ICollection<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<ICollection<User>> GetPageAsync(int pageNr, int pageSize);

}
