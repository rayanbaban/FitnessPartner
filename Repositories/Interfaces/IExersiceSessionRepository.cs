using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IExersiceSessionRepository
{
	Task<ExerciseSession?> AddSessionAsync(ExerciseSession session, int id);
	Task<ExerciseSession?> UpdateSessionsAsync(ExerciseSession session, int id);
	Task<ExerciseSession?> DeleteSessionsAsync(int id); 
	Task<ExerciseSession?> GetSessionsAsync(int id); 

}
