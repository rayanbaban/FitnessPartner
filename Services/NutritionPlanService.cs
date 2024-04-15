using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;

namespace FitnessPartner.Services
{
    public class NutritionPlanService : INutritionPlanService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<NutritionPlans, NutritionPlansDTO> _nutritionPlanMapper;
        private readonly INutritionPlansRepository _nutritionPlanRepository;
        private readonly ILogger<NutritionPlanService> _logger;

        public NutritionPlanService(IUserRepository userRepository,
            IMapper<NutritionPlans, NutritionPlansDTO> nutritionPlanMapper,
            INutritionPlansRepository nutritionPlanRepository,
            ILogger<NutritionPlanService> logger)
        {
            _userRepository = userRepository;
            _nutritionPlanMapper = nutritionPlanMapper;
            _nutritionPlanRepository = nutritionPlanRepository;
            _logger = logger;
        }

        public async Task<NutritionPlansDTO?> CreateNutritionPlanAsync(NutritionPlansDTO nutritionPlan, int loggedinUser)
        {
            var inloggedUser = await _userRepository.GetUserByIdAsync(loggedinUser);

            var nutritionPlanToAdd = _nutritionPlanMapper.MapToModel(nutritionPlan);
            nutritionPlan.UserId = loggedinUser;

            var addedNutritionPlan = await _nutritionPlanRepository.CreateNutritionPlanAsync(nutritionPlanToAdd, loggedinUser);

            return addedNutritionPlan != null ? _nutritionPlanMapper.MapToDto(addedNutritionPlan) : null;
        }

        public async Task<NutritionPlansDTO?> DeleteNutritionPlanAsync(int userId, int planId)
        {
            var nutritionPlanToDelete = await _nutritionPlanRepository.GetNutritionPlanByIdAsync(planId);

            if (nutritionPlanToDelete == null)
            {
                _logger?.LogError("NutritionPlan med ID {NutritionPlanId} ble ikke funnet for sletting", planId);
                return null;
            }

            if (!(userId == nutritionPlanToDelete.UserId || (nutritionPlanToDelete.UserId != null && nutritionPlanToDelete.User.IsAdminUser)))
            {
                _logger?.LogError("User {UserId} har ikke tilgang til å slette denne NutritionPlan", userId);
                throw new UnauthorizedAccessException($"User {userId} har ikke tilgang til å slette NutritionPlan");
            }

            var deletedNutritionPlan = await _nutritionPlanRepository.GetNutritionPlanByIdAsync(planId);

            return nutritionPlanToDelete != null ? _nutritionPlanMapper.MapToDto(nutritionPlanToDelete) : null;
        }

        public Task<ICollection<NutritionPlansDTO>> GetMyNutritionPlansAsync(int pageNr, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<NutritionPlansDTO?> GetNutritionPlanByIdAsync(int planId)
        {
            throw new NotImplementedException();
        }

        public Task<NutritionPlansDTO?> UpdateNutritionPlanAsync(NutritionPlansDTO nutritionPlansDTO, int planId, int loggedinUser)
        {
            throw new NotImplementedException();
        }
    }
}
