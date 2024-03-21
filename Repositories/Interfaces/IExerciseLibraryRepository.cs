using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IExerciseLibrary
{
	Task<ExerciseLibrary> GetExercisetByMuscleNameAsync(string muscleName);
	Task<ExerciseLibrary> DeleteExerciseAsync(int id);
	Task<ExerciseLibrary> UpdateExerciseAsync(int id);



}
