﻿using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.IUserRepository
{
	public interface IExersiceSessionRepository
	{
		Task<ExerciseSession?> CreateSessionAsync(ExerciseSession session, int id);
		Task<ExerciseSession?> UpdateSessionsAsync(ExerciseSession session, int id);
		Task<ExerciseSession?> DeleteSessionsAsync(int id);

	}
}
