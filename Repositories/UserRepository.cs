using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;

namespace FitnessPartner.Repositories;

public class UserRepository : IUserRepository
{
	public Task<User> Create(User user)
	{
		throw new NotImplementedException();
	}
}
