using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using Microsoft.Extensions.Logging;

namespace FitnessPartner.Services.Interfaces
{
	public interface IExerciseSessionService
	{
		Task<ExerciseSessionDTO?> AddSessionAsync(ExerciseSessionDTO exerciseSessionDTO);
		Task<ExerciseSessionDTO?> UpdateSessionAsync(ExerciseSessionDTO exerciseSession, int inloggedUser, int id);

		Task<ExerciseSessionDTO?> DeleteSessionByIdAsync(int id, int userId);

		Task<ICollection<ExerciseSessionDTO>> GetAllSessionsAsync(int pageNr, int pageSize);

		Task<ExerciseSessionDTO?> GetSessionByIdAsync(int id);

	}
}
