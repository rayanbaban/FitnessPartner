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

        public async Task<ICollection<NutritionPlansDTO>> GetMyNutritionPlanAsync(int pageNr, int pageSize)
        {
            var nutritionPlans = await _nutritionPlanRepository.GetMyNutritionPlanAsync(pageNr, pageSize);

            return nutritionPlans.Select(nutritionPlans => _nutritionPlanMapper.MapToDto(nutritionPlans)).ToList();
        }

        public async Task<NutritionPlansDTO?> GetNutritionPlanByIdAsync(int planId)
        {
            var nutritionPlanToGet = await _nutritionPlanRepository.GetNutritionPlanByIdAsync(planId);

            return nutritionPlanToGet != null ? _nutritionPlanMapper.MapToDto(nutritionPlanToGet) : null;
        }

        public async Task<NutritionPlansDTO?> UpdateNutritionPlanAsync(NutritionPlansDTO nutritionPlansDTO, int planId, int loggedinUser)
        {
            var nutritionPlanToUpd = await _nutritionPlanRepository.GetNutritionPlanByIdAsync(planId);

            if (nutritionPlanToUpd == null)
            {
                _logger?.LogError("NutritionPlan med ID {NutritionPlanId} ble ikke funnet for oppdatering", planId);
                return null;
            }


            //if (memberId != exerciseToUpd.MemberID && !eventToUpdate.Member.IsAdminMember)
            //{
            //	_logger?.LogError("Medlem {LoggedInUserId} har ikke tilgang til å oppdatere dette arrangementet", loggedInMember);
            //	_logger?.LogError($"Detaljer: LoggedInMemberId: {loggedInMember}, EventMemberId: {eventToUpdate.MemberID}, IsAdminMember: {eventToUpdate.Member.IsAdminMember}");

            //	throw new UnauthorizedAccessException($"Medlem {loggedInMember} har ikke tilgang til å oppdatere arrangementet");
            //}

            var updatedNutritionPlan = await _nutritionPlanRepository.UpdateNutritionPlanAsync(_nutritionPlanMapper.MapToModel(nutritionPlansDTO), loggedinUser, planId);

            if (updatedNutritionPlan != null)
            {
                _logger?.LogInformation("NutritionPlan med ID {NutritionPlanId} ble oppdatert.", planId);
                return _nutritionPlanMapper.MapToDto(updatedNutritionPlan);
            }

            return null;
        }

        public async Task<ICollection<NutritionPlansDTO>> GetPageAsync(int pageNr, int pageSize)
        {
            var res = await _nutritionPlanRepository.GetPageAsync(pageNr, pageSize);

            _logger?.LogInformation("Forsøker å hente side {PageNr} med størrelse {PageSize} av brukere", pageNr, pageSize);

            return res.Select(pages => _nutritionPlanMapper.MapToDto(pages)).ToList();
        }
    }
}
