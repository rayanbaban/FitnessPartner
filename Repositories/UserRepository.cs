using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FitnessPartnerDbContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(FitnessPartnerDbContext dbContext, ILogger<UserRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<User> AddUserAsync(User user)
    {
        _logger?.LogInformation($"Legger til et nytt medlem med ID{user.UserId}");
        var entry = await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        if (entry != null)
        {
            return entry.Entity;
        }

        return null;
    }

    public async Task<User?> DeleteUserByIdAsync(int id)
    {
        _logger?.LogDebug("Sletter bruker med id: {@id}", id);
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

        if (user == null)
            return null;

        await _dbContext.Users.Where(x => x.UserId == id)
            .ExecuteDeleteAsync();

        _dbContext.SaveChanges();

        return user;
    }

    public async Task<ICollection<User>> GetAllUsersAsync()
    {
        try
        {
            return await _dbContext.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Feil ved forsøk på å hente medlemmer.");
            throw;
        }
    }

    public async Task<ICollection<User>> GetAllUsersAsync(User user)
    {
        try
        {
            return await _dbContext.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Feil ved forsøk på å hente medlemmer.");
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.UserId == id);

        return user is null ? null : user;
    }

    public async Task<User> UpdateUserAsync(int id, User user)
    {
        _logger?.LogDebug("Sletter medlem med id: {@id}", id);
        var member = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

        if (member == null)
            return null;

        await _dbContext.Users.Where(x => x.UserId == id)
            .ExecuteDeleteAsync();

        _dbContext.SaveChanges();

        return member;
    }
}
