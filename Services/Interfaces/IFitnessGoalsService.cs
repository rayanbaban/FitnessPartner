using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Services.Interfaces
{
	public interface IFitnessGoalsService
	{

		Task<ICollection<FitnessGoalsDTO?>> GetMyFitnessGoalsAsync(int pageNr, int pageSize);
		Task<FitnessGoalsDTO?> UpdateFitnessGoalAsync(FitnessGoalsDTO fitnessGoalDTO, int goalId, int loggedinUser);
		Task<FitnessGoalsDTO?> DeleteFitnessGoalAsync(int userId,int goalId);
		Task<FitnessGoalsDTO?> CreateFitnessGoalAsync(FitnessGoalsDTO fitnessGoals, int loggedinUser);
		Task<FitnessGoalsDTO?> GetFitnessGoalByIdAsync(int goalId);
		Task<ICollection<FitnessGoalsDTO?>> GetPageAsync(int pageNr, int pageSize);

	}
}
