using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
	public interface IExerciseLibraryService
	{
		Task<ExerciseLibraryDTO?> AddExerciseLibraryAsync(int inloggedUser, ExerciseLibraryDTO exerciseDTO);
		Task<ICollection<ExerciseLibraryDTO?>> GetAllExerciesAsync(int pageNr, int pageSize);
		Task<ExerciseLibraryDTO?> GetExerciseByNameAsync(string name);
		Task<ExerciseLibraryDTO?> UpdateExerciseAsync(int exerciseId, int userId, ExerciseLibraryDTO exerciseDTO);
		Task<ExerciseLibraryDTO?> DeleteExerciseAsync(int exerciseId, int userId);

	}
}
