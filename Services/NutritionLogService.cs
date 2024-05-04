using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FitnessPartner.Services
{
    public class NutritionLogService : INutritionLogService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<NutritionLog, NutritionLogDTO> _nutritionLogMapper;
        private readonly INutritionLogRepository _nutritionLogRepository;
        private readonly ILogger<NutritionLogService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<AppUser> _userManager;


		public NutritionLogService(IUserRepository userRepository, IMapper<NutritionLog, NutritionLogDTO> nutritionLogMapper, INutritionLogRepository nutritionLogRepository, ILogger<NutritionLogService> logger, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
		{
			_userRepository = userRepository;
			_nutritionLogMapper = nutritionLogMapper;
			_nutritionLogRepository = nutritionLogRepository;
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
		}

		public async Task<NutritionLogDTO?> CreateNutritionLogAsync(NutritionLogDTO nutritionLog)
        {

			var nutritionLogToAdd = _nutritionLogMapper.MapToModel(nutritionLog);

			string userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException();
			}

			var inloggedUser = await _userManager.FindByIdAsync(userId);
			if (inloggedUser is null)
			{
				throw new UnauthorizedAccessException();
			}

            nutritionLogToAdd.User = inloggedUser;


			var addedNutritionLog = await _nutritionLogRepository.CreateNutritionLogAsync(nutritionLogToAdd);

            return addedNutritionLog != null ? _nutritionLogMapper.MapToDto(addedNutritionLog) : null;
        }

		public async Task<NutritionLogDTO?> DeleteNutritionLogAsync(int logId)
		{
			var nutritionLogToDelete = await _nutritionLogRepository.GetNutritionLogByIdAsync(logId);

			if (nutritionLogToDelete == null)
			{
				_logger?.LogError("NutritionLog med ID {NutritionLogId} ble ikke funnet for sletting", logId);
				return null;
			}

			var userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;

			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("Ugyldig bruker.");
			}

			var loggedInUser = await _userManager.FindByIdAsync(userId);
				
			if (nutritionLogToDelete.User != loggedInUser)
			{
				throw new UnauthorizedAccessException("Du har ikke tilgang til å slette denne NutritionLog");
			}

			var deletedNutritionLog = await _nutritionLogRepository.DeleteNutritionLogAsync(logId);

			return deletedNutritionLog != null ? _nutritionLogMapper.MapToDto(deletedNutritionLog) : null;
		}


		public async Task<ICollection<NutritionLogDTO>> GetMyNutritionLogsAsync(int pageNr, int pageSize)
        {
            var nutritionLogs = await _nutritionLogRepository.GetMyNutritionLogsAsync(pageNr, pageSize);

            return nutritionLogs.Select(nutritionLogs => _nutritionLogMapper.MapToDto(nutritionLogs)).ToList();
        }

        public async Task<NutritionLogDTO?> GetNutritionLogByIdAsync(int logId)
        {
            var nutritionLogToGet = await _nutritionLogRepository.GetNutritionLogByIdAsync(logId);

			if (nutritionLogToGet == null)
			{
				_logger?.LogError("Fitnessmålet med ID {goalId} ble ikke funnet.", logId);
				return null;
			}

			var userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;

			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("Ugyldig bruker.");
			}

			var loggedInUser = await _userManager.FindByIdAsync(userId);

			if (nutritionLogToGet.User != loggedInUser)
			{
				throw new UnauthorizedAccessException("Du har ikke tilgang til å vise dette fitnessmålet.");
			}

			return nutritionLogToGet != null ? _nutritionLogMapper.MapToDto(nutritionLogToGet) : null;
        }

        public async Task<ICollection<NutritionLogDTO>> GetPageAsync(int pageNr, int pageSize)
        {
            var res = await _nutritionLogRepository.GetPageAsync(pageNr, pageSize);

            _logger?.LogInformation("Forsøker å hente side {PageNr} med størrelse {PageSize} av brukere", pageNr, pageSize);

            return res.Select(pages => _nutritionLogMapper.MapToDto(pages)).ToList();
        }

		public async Task<NutritionLogDTO?> UpdateNutritionLogAsync(NutritionLogDTO nutritionLogDTO, int logId)
		{
			var nutritionLogToUpdate = await _nutritionLogRepository.GetNutritionLogByIdAsync(logId);

			if (nutritionLogToUpdate == null)
			{
				
				_logger?.LogError("NutritionLog med ID {NutritionLogId} ble ikke funnet for oppdatering", logId);
				return null;
			}

			var userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;

			if (string.IsNullOrEmpty(userId))
			{
				
				throw new UnauthorizedAccessException("Ugyldig bruker.");
			}

			var loggedInUser = await _userManager.FindByIdAsync(userId);
			
			if (nutritionLogToUpdate.User != loggedInUser)
			{
				throw new UnauthorizedAccessException("Du har ikke tilgang til å oppdatere denne NutritionLog");
			}
			
			var updatedNutritionLog = await _nutritionLogRepository.UpdateNutritionLogAsync(_nutritionLogMapper.MapToModel(nutritionLogDTO), logId);
			
			if (updatedNutritionLog != null)
			{
				_logger?.LogInformation("NutritionLog med ID {NutritionLogId} ble oppdatert.", logId);
				return _nutritionLogMapper.MapToDto(updatedNutritionLog);
			}

			return null;
		}

	}
}
