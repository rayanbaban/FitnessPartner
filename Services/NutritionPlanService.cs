using FitnessPartner.Mappers;
using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FitnessPartner.Services
{
    public class NutritionPlanService : INutritionPlanService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<NutritionPlans, NutritionPlansDTO> _nutritionPlanMapper;
        private readonly INutritionPlansRepository _nutritionPlanRepository;
        private readonly ILogger<NutritionPlanService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

		public NutritionPlanService(IUserRepository userRepository,
			IMapper<NutritionPlans, NutritionPlansDTO> nutritionPlanMapper,
			INutritionPlansRepository nutritionPlanRepository,
			ILogger<NutritionPlanService> logger,
			IHttpContextAccessor httpContextAccessor,
			UserManager<AppUser> userManager)
		{
			_userRepository = userRepository;
			_nutritionPlanMapper = nutritionPlanMapper;
			_nutritionPlanRepository = nutritionPlanRepository;
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
		}

		public async Task<NutritionPlansDTO?> CreateNutritionPlanAsync(NutritionPlansDTO nutritionPlan)
        {

            var nutritionPlanToAdd = _nutritionPlanMapper.MapToModel(nutritionPlan);
			
            string userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;


			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException();
			}

			var inloggedUser = await _userManager.FindByIdAsync(userId);

			nutritionPlanToAdd.User = inloggedUser;
			var addedNutritionPlan = await _nutritionPlanRepository.CreateNutritionPlanAsync(nutritionPlanToAdd);

            return addedNutritionPlan != null ? _nutritionPlanMapper.MapToDto(addedNutritionPlan) : null;
			
			

		}

        public async Task<NutritionPlansDTO?> DeleteNutritionPlanAsync(int planId)
        {
            var nutritionPlanToDelete = await _nutritionPlanRepository.GetNutritionPlanByIdAsync(planId);

            if (nutritionPlanToDelete == null)
            {
                _logger?.LogError("NutritionPlan med ID {NutritionPlanId} ble ikke funnet for sletting", planId);
                return null;
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

        public async Task<NutritionPlansDTO?> UpdateNutritionPlanAsync(NutritionPlansDTO nutritionPlansDTO, int planId)
        {
            var nutritionPlanToUpd = await _nutritionPlanRepository.GetNutritionPlanByIdAsync(planId);

            if (nutritionPlanToUpd == null)
            {
                _logger?.LogError("NutritionPlan med ID {NutritionPlanId} ble ikke funnet for oppdatering", planId);
                return null;
            }

            var updatedNutritionPlan = await _nutritionPlanRepository.UpdateNutritionPlanAsync(_nutritionPlanMapper.MapToModel(nutritionPlansDTO), planId);

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
