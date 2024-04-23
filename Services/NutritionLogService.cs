using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;

namespace FitnessPartner.Services
{
    public class NutritionLogService : INutritionLogService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<NutritionLog, NutritionLogDTO> _nutritionLogMapper;
        private readonly INutritionLogRepository _nutritionLogRepository;
        private readonly ILogger<NutritionLogService> _logger;

        public NutritionLogService(IUserRepository userRepository, IMapper<NutritionLog, NutritionLogDTO> nutritionLogMapper, INutritionLogRepository nutritionLogRepository, ILogger<NutritionLogService> logger)
        {
            _userRepository = userRepository;
            _nutritionLogMapper = nutritionLogMapper;
            _nutritionLogRepository = nutritionLogRepository;
            _logger = logger;
        }

        public async Task<NutritionLogDTO?> CreateNutritionLogAsync(NutritionLogDTO nutritionLog, int loggedinUser)
        {
            var inloggedUser = await _userRepository.GetUserByIdAsync(loggedinUser);

            var nutritionLogToAdd = _nutritionLogMapper.MapToModel(nutritionLog);
            nutritionLog.UserId = loggedinUser;

            var addedNutritionLog = await _nutritionLogRepository.CreateNutritionLogAsync(nutritionLogToAdd, loggedinUser);

            return addedNutritionLog != null ? _nutritionLogMapper.MapToDto(addedNutritionLog) : null;
        }

        public async Task<NutritionLogDTO?> DeleteNutritionLogAsync(int userId, int logId)
        {
            var nutritionLogToDelete = await _nutritionLogRepository.GetNutritionLogByIdAsync(logId);

            if (nutritionLogToDelete == null)
            {
                _logger?.LogError("NutritionLog med ID {NutritionLogId} ble ikke funnet for sletting", logId);
                return null;
            }

            if (!(userId == nutritionLogToDelete.AppUserId || (nutritionLogToDelete.AppUserId != null && nutritionLogToDelete.User.IsAdminUser)))
            {
                _logger?.LogError("User {UserId} har ikke tilgang til å slette denne NutritionLog", userId);
                throw new UnauthorizedAccessException($"User {userId} har ikke tilgang til å slette NutritionLog");
            }

            var deletedNutritionLog = await _nutritionLogRepository.GetNutritionLogByIdAsync(logId);

            return nutritionLogToDelete != null ? _nutritionLogMapper.MapToDto(nutritionLogToDelete) : null;

        }

        public async Task<ICollection<NutritionLogDTO>> GetMyNutritionLogsAsync(int pageNr, int pageSize)
        {
            var nutritionLogs = await _nutritionLogRepository.GetMyNutritionLogsAsync(pageNr, pageSize);

            return nutritionLogs.Select(nutritionLogs => _nutritionLogMapper.MapToDto(nutritionLogs)).ToList();
        }

        public async Task<NutritionLogDTO?> GetNutritionLogByIdAsync(int logId)
        {
            var nutritionLogToGet = await _nutritionLogRepository.GetNutritionLogByIdAsync(logId);

            return nutritionLogToGet != null ? _nutritionLogMapper.MapToDto(nutritionLogToGet) : null;
        }

        public async Task<ICollection<NutritionLogDTO>> GetPageAsync(int pageNr, int pageSize)
        {
            var res = await _nutritionLogRepository.GetPageAsync(pageNr, pageSize);

            _logger?.LogInformation("Forsøker å hente side {PageNr} med størrelse {PageSize} av brukere", pageNr, pageSize);

            return res.Select(pages => _nutritionLogMapper.MapToDto(pages)).ToList();
        }

        public async Task<NutritionLogDTO?> UpdateNutritionLogAsync(NutritionLogDTO nutritionLogDTO, int logId, int loggedinUser)
        {
            var nutritionLogToUpd = await _nutritionLogRepository.GetNutritionLogByIdAsync(logId);

            if (nutritionLogToUpd == null)
            {
                _logger?.LogError("NutritionLog med ID {NutritionLogId} ble ikke funnet for oppdatering", logId);
                return null;
            }


            //if (memberId != exerciseToUpd.MemberID && !eventToUpdate.Member.IsAdminMember)
            //{
            //	_logger?.LogError("Medlem {LoggedInUserId} har ikke tilgang til å oppdatere dette arrangementet", loggedInMember);
            //	_logger?.LogError($"Detaljer: LoggedInMemberId: {loggedInMember}, EventMemberId: {eventToUpdate.MemberID}, IsAdminMember: {eventToUpdate.Member.IsAdminMember}");

            //	throw new UnauthorizedAccessException($"Medlem {loggedInMember} har ikke tilgang til å oppdatere arrangementet");
            //}

            var updatedNutritionLog = await _nutritionLogRepository.UpdateNutritionLogAsync(_nutritionLogMapper.MapToModel(nutritionLogDTO), loggedinUser, logId);

            if (updatedNutritionLog != null)
            {
                _logger?.LogInformation("NutritionLog med ID {NutritionLogId} ble oppdatert.", logId);
                return _nutritionLogMapper.MapToDto(updatedNutritionLog);
            }

            return null;

        }
    }
}
