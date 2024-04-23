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

	public Task<AppUser?> AddUserAsync(AppUser user)
	{
		throw new NotImplementedException();
	}

	public Task<AppUser?> DeleteUserByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<ICollection<AppUser?>> GetAllUsersAsync()
	{
		throw new NotImplementedException();
	}

	public Task<ICollection<AppUser>> GetPageAsync(int pageNr, int pageSize)
	{
		throw new NotImplementedException();
	}

	public Task<AppUser?> GetUserByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<AppUser?> GetUserByNameAsync(string name)
	{
		throw new NotImplementedException();
	}

	public Task<AppUser?> UpdateUserAsync(int id, AppUser user)
	{
		throw new NotImplementedException();
	}

	//   public async Task<User?> AddUserAsync(User user)
	//   {
	//       _logger?.LogInformation($"Legger til et nytt user med ID{user.UserId}");
	//       var entry = await _dbContext.Users.AddAsync(user);
	//       await _dbContext.SaveChangesAsync();

	//       if (entry != null)
	//       {
	//           return entry.Entity;
	//       }

	//       return null;
	//   }

	//   public async Task<User?> DeleteUserByIdAsync(int id)
	//   {
	//       _logger?.LogDebug("Sletter bruker med id: {@id}", id);
	//       var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

	//       if (user == null)
	//           return null;

	//       await _dbContext.Users.Where(x => x.UserId == id)
	//           .ExecuteDeleteAsync();

	//       _dbContext.SaveChanges();

	//       return user;
	//   }

	//   public async Task<User?> GetUserByIdAsync(int id)
	//   {
	//       var user = await _dbContext.Users
	//           .FirstOrDefaultAsync(x => x.UserId == id);

	//       return user is null ? null : user;
	//   }

	//   public async Task<ICollection<User?>> GetAllUsersAsync()
	//   {
	//       try
	//       {
	//           return await _dbContext.Users.ToListAsync();
	//       }
	//       catch (Exception ex)
	//       {
	//           _logger?.LogError(ex, "Feil ved forsøk på å hente medlemmer.");
	//           throw;
	//       }
	//   }

	//   public async Task<User?> UpdateUserAsync(int id, User user)
	//   {
	//       _logger?.LogDebug("Sletter bruker med id: {@id}", id);
	//       var bruker = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

	//       if (user == null)
	//           return null;

	//       await _dbContext.Users.Where(x => x.UserId == id)
	//           .ExecuteDeleteAsync();

	//       _dbContext.SaveChanges();

	//       return user;
	//   }

	//   public async Task<ICollection<User>> GetPageAsync(int pageNr, int pageSize)
	//   {
	//       var totCount = _dbContext.Users.Count();
	//       var totPages = (int)Math.Ceiling((double)totCount / pageSize);

	//       return await _dbContext.Users
	//            .OrderBy(x => x.UserId)
	//            .Skip((pageNr - 1) * pageSize)
	//            .Take(pageSize)
	//            .ToListAsync();
	//   }
	//public async Task<User?> GetUserByNameAsync(string name)
	//{
	//	var user = await _dbContext.Users
	//			.FirstOrDefaultAsync(x => x.UserName!.Equals(name));
	//	return user is null ? null : user;
	//}
}
