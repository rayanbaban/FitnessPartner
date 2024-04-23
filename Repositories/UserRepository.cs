using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

	public async Task<AppUser?> AddUserAsync(AppUser user)
	{
		_logger?.LogInformation($"Legger til et nytt user med ID{user.Id}");
		var entry = await _dbContext.Users.AddAsync(user);
		await _dbContext.SaveChangesAsync();

		if (entry != null)
		{
			return entry.Entity;
		}

		return null;
	}

	public async Task<AppUser?> DeleteUserByIdAsync(int id)
	{
		_logger?.LogDebug("Sletter bruker med id: {@id}", id);
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.AppUserId == id);

		if (user == null)
			return null;

		await _dbContext.Users.Where(x => x.AppUserId == id)
			.ExecuteDeleteAsync();

		_dbContext.SaveChanges();

		return user;
	}

	public async Task<AppUser?> GetUserByIdAsync(int id)
	{
		var user = await _dbContext.Users
			.FirstOrDefaultAsync(x => x.AppUserId == id);

		return user is null ? null : user;
	}

	public async Task<ICollection<AppUser?>> GetAllUsersAsync()
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

	public async Task<AppUser?> UpdateUserAsync(int id, AppUser user)
	{
		_logger?.LogDebug("Sletter bruker med id: {@id}", id);
		var bruker = await _dbContext.Users.FirstOrDefaultAsync(x => x.AppUserId == id);

		if (user == null)
			return null;

		await _dbContext.Users.Where(x => x.AppUserId == id)
			.ExecuteDeleteAsync();

		_dbContext.SaveChanges();

		return user;
	}

	public async Task<ICollection<AppUser>> GetPageAsync(int pageNr, int pageSize)
	{
		var totCount = _dbContext.Users.Count();
		var totPages = (int)Math.Ceiling((double)totCount / pageSize);

		return await _dbContext.Users
			 .OrderBy(x => x.Id)
			 .Skip((pageNr - 1) * pageSize)
			 .Take(pageSize)
			 .ToListAsync();
	}
	public async Task<AppUser?> GetUserByNameAsync(string name)
	{
		var user = await _dbContext.Users
				.FirstOrDefaultAsync(x => x.UserName!.Equals(name));
		return user is null ? null : user;
	}
}
