using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IExerciseLibraryRepository
{
	Task<ExerciseLibrary> CreateExerciseAsync(ExerciseLibrary exerciseLib);

	Task<ExerciseLibrary> GetExerciseByMuscleNameAsync(string muscleName);
	Task<ExerciseLibrary> GetExerciseByIdAsync(int id );
	Task<ExerciseLibrary> DeleteExerciseAsync(int id);
	Task<ExerciseLibrary> UpdateExerciseAsync(int id, ExerciseLibrary updatedExercise);




}
