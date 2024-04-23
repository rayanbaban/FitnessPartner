using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> AddUserAsync(AppUser user);
    Task<AppUser?> UpdateUserAsync(int id, AppUser user);
    Task<AppUser?> DeleteUserByIdAsync(int id);
    Task<ICollection<AppUser?>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<ICollection<AppUser>> GetPageAsync(int pageNr, int pageSize);
    Task<AppUser?> GetUserByNameAsync(string name);


}
