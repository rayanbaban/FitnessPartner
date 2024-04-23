using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;

namespace FitnessPartner.Services
{
    public class FitnessGoalsService : IFitnessGoalsService
    {
        private readonly IFitnessGoalsRepository _fitnessGoalsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper<FitnessGoals, FitnessGoalsDTO> _fitnessGoalsMapper;
        private readonly IMapper<AppUser, UserDTO> _UserMapper;
        private readonly ILogger<FitnessGoalsService> _logger;


        public FitnessGoalsService(
            IFitnessGoalsRepository fitnessGoalsRepository,
            IUserRepository userRepository,
            IMapper<FitnessGoals, FitnessGoalsDTO> fitnessGoalsMapper,
            IMapper<AppUser, UserDTO> userMapper,
            ILogger<FitnessGoalsService> logger)
        {
            _fitnessGoalsRepository = fitnessGoalsRepository;
            _userRepository = userRepository;
            _fitnessGoalsMapper = fitnessGoalsMapper;
            _UserMapper = userMapper;
            _logger = logger;
        }

        public async Task<FitnessGoalsDTO?> CreateFitnessGoalAsync(FitnessGoalsDTO fitnessGoals, int loggedinUser)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(loggedinUser);

            var goalToAdd = _fitnessGoalsMapper.MapToModel(fitnessGoals);
            goalToAdd.AppUserId = loggedinUser;

            var addedGoal = await _fitnessGoalsRepository.CreateFitnessGoalAsync(goalToAdd);

            return addedGoal != null ? _fitnessGoalsMapper.MapToDto(addedGoal) : null;
        }

        public async Task<FitnessGoalsDTO?> DeleteFitnessGoalAsync(int userId, int goalId)
        {
            var goalToDelete = await _fitnessGoalsRepository.GetFitnessGoalByIdAsync(goalId);

            if (goalToDelete == null)
            {
                _logger?.LogError("Fitness gaol med ID {EventId} ble ikke funnet for sletting", goalId);
                return null;
            }

            if (!(userId == goalToDelete.AppUserId || (goalToDelete.User != null && goalToDelete.User.IsAdminUser)))
            {
                _logger?.LogError("User {userId} har ikke tilgang til å slette dette fitness goalet", userId);
                throw new UnauthorizedAccessException($"User {userId} har ikke tilgang til å slette fitness goalet");
            }

            var deletedGoal = await _fitnessGoalsRepository.DeleteFitnessGoalAsync(goalId);

            return deletedGoal != null ? _fitnessGoalsMapper.MapToDto(deletedGoal) : null;
        }

        public async Task<FitnessGoalsDTO?> GetFitnessGoalByIdAsync(int goalId)
        {
            var goalToGet = await _fitnessGoalsRepository.GetFitnessGoalByIdAsync(goalId);
            return goalToGet != null ? _fitnessGoalsMapper.MapToDto(goalToGet) : null;
        }

        public async Task<ICollection<FitnessGoalsDTO?>> GetMyFitnessGoalsAsync(int pageNr, int pageSize)
        {
            var fitnessGoals = await _fitnessGoalsRepository.GetMyFitnessGoalsAsync(pageNr, pageSize);

            return fitnessGoals.Select(fitnessGoals => _fitnessGoalsMapper.MapToDto(fitnessGoals)).ToList();
        }

        public async Task<ICollection<FitnessGoalsDTO>> GetPageAsync(int pageNr, int pageSize)
        {
            var res = await _fitnessGoalsRepository.GetPageAsync(pageNr, pageSize);

            _logger?.LogInformation("Forsøker å hente side {PageNr} med størrelse {PageSize} av brukere", pageNr, pageSize);

            return res.Select(pages => _fitnessGoalsMapper.MapToDto(pages)).ToList();
        }

        public async Task<FitnessGoalsDTO> UpdateFitnessGoalAsync(FitnessGoalsDTO fitnessGoalsDto, int goalId, int loggedinUser)
        {
            var goalToUpdate = await _fitnessGoalsRepository.GetFitnessGoalByIdAsync(goalId);

            if (goalToUpdate == null || goalToUpdate.User == null)
            {
                _logger?.LogError("fitness goal med ID {goalId} ble ikke funnet for oppdatering eller mangler tilknyttet user", goalId);
                return null;
            }

            if (loggedinUser != goalToUpdate.AppUserId && !goalToUpdate.User.IsAdminUser)
            {
                _logger?.LogError("User {LoggedInUserId} har ikke tilgang til å oppdatere goal", loggedinUser);
                _logger?.LogError($"Detaljer: LoggedInUserId: {loggedinUser}, fitnessgoalId: {goalToUpdate.AppUserId}, IsAdminMember: {goalToUpdate.User.IsAdminUser}");

                throw new UnauthorizedAccessException($"User {loggedinUser} har ikke tilgang til å oppdatere fitness goalet");
            }

            var updatedEvent = await _fitnessGoalsRepository.UpdateFitnessGoalAsync(_fitnessGoalsMapper.MapToModel(fitnessGoalsDto), goalId);

            if (updatedEvent != null)
            {
                _logger?.LogInformation("fitness goal med ID {goalId} ble oppdatert vellykket", goalId);
                return _fitnessGoalsMapper.MapToDto(updatedEvent);
            }

            return null;
        }
    }
}
