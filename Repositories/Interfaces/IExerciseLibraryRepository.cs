using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IExerciseLibraryRepository
{
    Task<ExerciseLibrary?> CreateExerciseAsync(ExerciseLibrary exerciseLib);
    Task<ICollection<ExerciseLibrary?>?> GetAllExercisesAsync(int pageNr, int pageSize);

    Task<ExerciseLibrary?> GetExerciseByNameAsync(string muscleName);
    Task<ExerciseLibrary?> GetExerciseByIdAsync(int id);
    Task<ExerciseLibrary?> DeleteExerciseAsync(int id);
    Task<ExerciseLibrary?> UpdateExerciseAsync(ExerciseLibrary updatedExercise, int id);
}
