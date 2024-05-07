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
    public class FitnessGoalsService : IFitnessGoalsService
    {
        private readonly IFitnessGoalsRepository _fitnessGoalsRepository;
        private readonly IMapper<FitnessGoals, FitnessGoalsDTO> _fitnessGoalsMapper;
        private readonly ILogger<FitnessGoalsService> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


		public FitnessGoalsService(
			IFitnessGoalsRepository fitnessGoalsRepository,
			IMapper<FitnessGoals, FitnessGoalsDTO> fitnessGoalsMapper,
			ILogger<FitnessGoalsService> logger,
			UserManager<AppUser> userManager,
			IHttpContextAccessor httpContextAccessor)
		{
			_fitnessGoalsRepository = fitnessGoalsRepository;
			_fitnessGoalsMapper = fitnessGoalsMapper;
			_logger = logger;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<FitnessGoalsDTO?> CreateFitnessGoalAsync(FitnessGoalsDTO fitnessGoals)
        {
            
            var goalToAdd = _fitnessGoalsMapper.MapToModel(fitnessGoals);

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

			goalToAdd.User = inloggedUser;

            var addedGoal = await _fitnessGoalsRepository.CreateFitnessGoalAsync(goalToAdd);

            return addedGoal != null ? _fitnessGoalsMapper.MapToDto(addedGoal) : null;
        }

		public async Task<FitnessGoalsDTO?> UpdateFitnessGoalAsync(FitnessGoalsDTO fitnessGoalsDto, int goalId)
		{
			var goalToUpdate = await _fitnessGoalsRepository.GetFitnessGoalByIdAsync(goalId);

			if (goalToUpdate == null)
			{
				_logger?.LogError("Fitnessmålet med ID {goalId} ble ikke funnet for oppdatering eller mangler tilknyttet bruker", goalId);
				return null;
			}

			string userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;

			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("Ugyldig bruker.");
			}

			var inloggedUser = await _userManager.FindByIdAsync(userId);

			if (inloggedUser is null)
			{
				throw new UnauthorizedAccessException("Innlogget bruker ble ikke funnet.");
			}

			if (goalToUpdate.User != inloggedUser)
			{
				throw new UnauthorizedAccessException("Du har ikke tilgang til å oppdatere dette fitnessmålet.");
			}

			goalToUpdate.User = inloggedUser;

			var updatedEvent = await _fitnessGoalsRepository.UpdateFitnessGoalAsync(_fitnessGoalsMapper.MapToModel(fitnessGoalsDto), goalId);

			if (updatedEvent != null)
			{
				_logger?.LogInformation("Fitnessmålet med ID {goalId} ble oppdatert vellykket", goalId);
				return _fitnessGoalsMapper.MapToDto(updatedEvent);
			}

			return null;
		}


		public async Task<FitnessGoalsDTO?> DeleteFitnessGoalAsync(int goalId)
		{
			var goalToDelete = await _fitnessGoalsRepository.GetFitnessGoalByIdAsync(goalId);

			if (goalToDelete == null)
			{
				_logger?.LogError("Fitnessmålet med ID {goalId} ble ikke funnet for sletting", goalId);
				return null;
			}

			var userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;


			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("Ugyldig bruker.");
			}
			var inloggedUser = await _userManager.FindByIdAsync(userId);

			if (inloggedUser is null)
			{
				throw new UnauthorizedAccessException("Innlogget bruker ble ikke funnet.");
			}


			if (goalToDelete.User != inloggedUser)
			{
				throw new UnauthorizedAccessException("Du har ikke tilgang til å oppdatere dette fitnessmålet.");
			}

			goalToDelete.User = inloggedUser;


			var deletedGoal = await _fitnessGoalsRepository.DeleteFitnessGoalAsync(goalId);

			return deletedGoal != null ? _fitnessGoalsMapper.MapToDto(deletedGoal) : null;
		}


		public async Task<FitnessGoalsDTO?> GetFitnessGoalByIdAsync(int goalId)
		{
			var goalToGet = await _fitnessGoalsRepository.GetFitnessGoalByIdAsync(goalId);

			if (goalToGet == null)
			{
				_logger?.LogError("Fitnessmålet med ID {goalId} ble ikke funnet.", goalId);
				return null;
			}

			var userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;

			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("Ugyldig bruker.");
			}

			var loggedInUser = await _userManager.FindByIdAsync(userId);

			if (goalToGet.User != loggedInUser)
			{
				throw new UnauthorizedAccessException("Du har ikke tilgang til å vise dette fitnessmålet.");
			}

			return _fitnessGoalsMapper.MapToDto(goalToGet);
		}


		public async Task<ICollection<FitnessGoalsDTO>> GetMyFitnessGoalsAsync(string userId, int pageNr, int pageSize)
        {
			var goals = await _fitnessGoalsRepository.GetMyFitnessGoalsAsync(userId, pageNr, pageSize);

			if (goals == null)
				return null;
			return goals.Select(exerciseSessions => _fitnessGoalsMapper.MapToDto(exerciseSessions)).ToList();

		}

		public async Task<ICollection<FitnessGoalsDTO>> GetPageAsync(int pageNr, int pageSize)
        {
            var res = await _fitnessGoalsRepository.GetPageAsync(pageNr, pageSize);

            _logger?.LogInformation("Forsøker å hente side {PageNr} med størrelse {PageSize} av brukere", pageNr, pageSize);

            return res.Select(pages => _fitnessGoalsMapper.MapToDto(pages)).ToList();
        }

		public async Task<ICollection<FitnessGoalsDTO>> GetAllFitnessGoalsAsync(int pageNr, int pageSize)
		{
			var goals = await _fitnessGoalsRepository.GetAllFitnessGoalsAsync(pageNr, pageSize);

			return goals.Select(ExerciseSessions => _fitnessGoalsMapper.MapToDto(ExerciseSessions)).ToList();
		}
	}
}
