using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
		_logger?.LogDebug("Oppdaterer bruker med id: {@id}", id);

		var bruker = await _dbContext.Users.FirstOrDefaultAsync(x => x.AppUserId == id);

		if (bruker == null)
		{
			_logger?.LogWarning("Kunne ikke finne bruker med id: {@id}", id);
			return null;
		}
		bruker.UserName = bruker.UserName;
		bruker.UserName = string.IsNullOrEmpty(user.UserName) ? user.UserName : user.UserName;
		bruker.FirstName = string.IsNullOrEmpty(user.FirstName) ? user.FirstName : user.FirstName;
		bruker.LastName = string.IsNullOrEmpty(user.LastName) ? user.LastName : user.LastName;
		bruker.Email = string.IsNullOrEmpty(user.Email) ? user.Email : user.Email;
		bruker.AppUserEmail = string.IsNullOrEmpty(user.AppUserEmail) ? user.AppUserEmail : user.AppUserEmail;
		bruker.AppUserName = string.IsNullOrEmpty(user.AppUserName) ? user.AppUserName : user.AppUserName;
		bruker.Weight = user.Weight != 0 ? user.Weight : user.Weight;
		bruker.Height = user.Height != 0 ? user.Height : user.Height;
		bruker.Age = user.Age != 0 ? user.Age : user.Age;
		bruker.Email = string.IsNullOrEmpty(user.Email) ? user.Email : user.Email;
		bruker.Email = string.IsNullOrEmpty(user.Email) ? user.Email : user.Email;
		bruker.Email = string.IsNullOrEmpty(user.Email) ? user.Email : user.Email;


		try
		{
			// Lagrer endringene til databasen
			await _dbContext.SaveChangesAsync();
			_logger?.LogInformation("Bruker med id {Id} oppdatert", id);
			return bruker;
		}
		catch (Exception ex)
		{
			// Logg feil hvis oppdateringen mislykkes
			_logger?.LogError(ex, "Feil ved oppdatering av bruker med id {Id}", id);
			throw;
		}
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
