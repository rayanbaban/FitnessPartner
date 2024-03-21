using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IUserRepository
{
	Task<User> Create(User user);
}
