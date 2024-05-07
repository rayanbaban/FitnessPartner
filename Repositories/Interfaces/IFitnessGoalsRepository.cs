using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IFitnessGoalsRepository
{

	Task<ICollection<FitnessGoals>> GetMyFitnessGoalsAsync(string userId, int pageNr, int pageSize);
	Task<ICollection<FitnessGoals>> GetAllFitnessGoalsAsync(int pageNr, int pageSize);
	Task<FitnessGoals> UpdateFitnessGoalAsync(FitnessGoals fitnessGoals, int goalId);
	Task<FitnessGoals> DeleteFitnessGoalAsync(int goalId);
	Task<FitnessGoals> GetFitnessGoalByIdAsync(int goalId);
	Task<FitnessGoals> CreateFitnessGoalAsync(FitnessGoals fitnessGoals);
	Task<ICollection<FitnessGoals>> GetPageAsync(int pageNr, int pageSize);


}
