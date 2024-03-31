using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IFitnessGoalsRepository
{
	Task<ICollection<FitnessGoals>> GetMyFitnessGoalsAsync(int pageNr, int pageSize);
	Task<FitnessGoals> UpdateFitnessGoalAsync(FitnessGoals fitnessGoals, int goalId);
	Task<FitnessGoals> DeleteFitnessGoalAsync(int goalId);
	Task<FitnessGoals> CreateFitnessGoalAsync(FitnessGoals fitnessGoals);
	Task<FitnessGoals> GetFitnessGoalByIdAsync( int goalId);
	Task<ICollection<FitnessGoals>> GetPageAsync(int pageNr, int pageSize);


}
