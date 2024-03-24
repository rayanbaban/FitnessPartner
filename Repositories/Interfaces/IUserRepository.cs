﻿using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<User> DeleteUserByIdAsync(int id);
    Task<ICollection<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);

}
