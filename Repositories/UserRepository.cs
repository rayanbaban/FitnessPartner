using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;

namespace FitnessPartner.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FitnessPartnerDbContext _dbContext;
    private readonly object _logger;

    public UserRepository(FitnessPartnerDbContext dbContext, ILogger<UserRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<User> AddUserAsync(User user)
    {
        _logger?.LogInformation($"Legger til et nytt medlem med ID{user.UserId}");
        var entry = await _dbContext.
        await _dbContext.SaveChangesAsync();

        if (entry != null)
        {
            return entry.Entity;
        }

        return null;
    }

    public Task<User> DeleteUserByIdAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}
