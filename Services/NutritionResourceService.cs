using FitnessPartner.Data;
using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FitnessPartner.Services
{
    public class NutritionResourceService : INutritionResourceService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<NutritionResources, NutritionResourcesDTO> _nutritionResourcesMapper;
        private readonly INutritionResourcesRepository _nutritionResourcesRepository;
        private readonly ILogger<NutritionResourceService> _logger;
        private readonly FitnessPartnerDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContnextAccessor;
        private readonly UserManager<AppUser> _userManager;

		public NutritionResourceService(IUserRepository userRepository, 
            IMapper<NutritionResources, 
                NutritionResourcesDTO> nutritionResourcesMapper, 
            INutritionResourcesRepository nutritionResourcesRepository, 
            ILogger<NutritionResourceService> logger, FitnessPartnerDbContext dbContext, 
            IHttpContextAccessor httpContnextAccessor, 
            UserManager<AppUser> userManager)
		{
			_userRepository = userRepository;
			_nutritionResourcesMapper = nutritionResourcesMapper;
			_nutritionResourcesRepository = nutritionResourcesRepository;
			_logger = logger;
			_dbContext = dbContext;
			_httpContnextAccessor = httpContnextAccessor;
			_userManager = userManager;
		}

		public async Task<NutritionResourcesDTO?> CreateNutritionResourceAsync(NutritionResourcesDTO nutritionResources)
        {

			var nutritionResourcesToAdd = _nutritionResourcesMapper.MapToModel(nutritionResources);


			string userId = _httpContnextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException();
			}

			var inloggedUser = await _userManager.FindByIdAsync(userId);
			if (inloggedUser is null)
			{
				throw new UnauthorizedAccessException();
			}

			nutritionResourcesToAdd.User = inloggedUser;

            var addedNutritionResources = await _nutritionResourcesRepository.CreateNutritionResourceAsync(nutritionResourcesToAdd);

            return addedNutritionResources != null ? _nutritionResourcesMapper.MapToDto(addedNutritionResources) : null;
        }

        public async Task<NutritionResourcesDTO?> DeleteNutritionResourceAsync(int resourcesId)
        {
            var nutritionResourceToDelete = await _nutritionResourcesRepository.GetNutritionResourceByIdAsync(resourcesId);

            if (nutritionResourceToDelete == null)
            {
                _logger?.LogError("NutritionLog med ID {NutritionLogId} ble ikke funnet for sletting", resourcesId);
                return null;
            }

			string userId = _httpContnextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException();
			}

			var inloggedUser = await _userManager.FindByIdAsync(userId);
			if (inloggedUser is null)
			{
				throw new UnauthorizedAccessException();
			}

			nutritionResourceToDelete.User = inloggedUser;

			var deletedNutritionResources = await _nutritionResourcesRepository.GetNutritionResourceByIdAsync(resourcesId);

            return deletedNutritionResources != null ? _nutritionResourcesMapper.MapToDto(deletedNutritionResources) : null;
        }

        public async Task<ICollection<NutritionResourcesDTO>> GetMyNutritionResourceAsync(int pageNr, int pageSize)
        {
            var nutritionResources = await _nutritionResourcesRepository.GetNutritionResourcesAsync(pageNr, pageSize);

            return nutritionResources.Select(nutritionLogs => _nutritionResourcesMapper.MapToDto(nutritionLogs)).ToList();
        }

        public async Task<NutritionResourcesDTO?> GetNutritionResourceByIdAsync(int resourcesId)
        {
            var nutritionResourceToGet = await _nutritionResourcesRepository.GetNutritionResourceByIdAsync(resourcesId);

            return nutritionResourceToGet != null ? _nutritionResourcesMapper.MapToDto(nutritionResourceToGet) : null;
        }

        public async Task<ICollection<NutritionResourcesDTO>> GetPageAsync(int pageNr, int pageSize)
        {
            var res = await _nutritionResourcesRepository.GetPageAsync(pageNr, pageSize);

            _logger?.LogInformation("Forsøker å hente side {PageNr} med størrelse {PageSize} av brukere", pageNr, pageSize);

            return res.Select(pages => _nutritionResourcesMapper.MapToDto(pages)).ToList();
        }

        public async Task<NutritionResourcesDTO?> UpdateNutritionResourceAsync(NutritionResourcesDTO nutritionResourcesDTO, int resourceId)
        {
            var nutritionResourceToUpd = await _nutritionResourcesRepository.GetNutritionResourceByIdAsync(resourceId);

            if (nutritionResourceToUpd == null)
            {
                _logger?.LogError("NutritionResource med ID {NutritionResourceId} ble ikke funnet for oppdatering", resourceId);
                return null;
            }

			string userId = _httpContnextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException();
			}

			var inloggedAppUser = await _userManager.FindByIdAsync(userId);

			var updatedNutritionResource = await _nutritionResourcesRepository.UpdateNutritionResourceAsync(_nutritionResourcesMapper.MapToModel(nutritionResourcesDTO), resourceId);

            nutritionResourceToUpd.User = inloggedAppUser;

            if (updatedNutritionResource != null)
            {
                _logger?.LogInformation("NutritionResource med ID {NutritionResourceId} ble oppdatert.", resourceId);
                return _nutritionResourcesMapper.MapToDto(updatedNutritionResource);
            }

            return null;
        }
    }
}
