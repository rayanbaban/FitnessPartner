using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FitnessPartner.Services
{
	public class ExerciseSessionService : IExerciseSessionService
	{
		private readonly IExersiceSessionRepository _exerciseSessionRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper<ExerciseSession, ExerciseSessionDTO> _exerciseSessionMapper;
		private readonly IMapper<AppUser, UserDTO> _userMapper;
		private readonly ILogger<ExerciseSessionService> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<AppUser> _usermanager;

		public ExerciseSessionService(
			IExersiceSessionRepository exerciseSessionRepository,
			IUserRepository userRepository,
			IMapper<ExerciseSession, ExerciseSessionDTO> exerciseSessionMapper,
			IMapper<AppUser, UserDTO> userMapper,
			ILogger<ExerciseSessionService> logger, IHttpContextAccessor httpContextAccessor,
			UserManager<AppUser> usermanager)
		{
			_exerciseSessionRepository = exerciseSessionRepository;
			_userRepository = userRepository;
			_exerciseSessionMapper = exerciseSessionMapper;
			_userMapper = userMapper;
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
			_usermanager = usermanager;
		}

		public async Task<ExerciseSessionDTO?> AddSessionAsync(ExerciseSessionDTO exerciseSessionDTO)
		{

			var exerciseToAdd = _exerciseSessionMapper.MapToModel(exerciseSessionDTO);

			string userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException();
			}

			var inloggedAppUser = await _usermanager.FindByIdAsync(userId);
			if (inloggedAppUser is null)
			{
				throw new UnauthorizedAccessException();
			}

			exerciseToAdd.User = inloggedAppUser;
			exerciseToAdd.AppUserId = inloggedAppUser.AppUserId;
			
			var addedExercise = await _exerciseSessionRepository.AddSessionAsync(exerciseToAdd);


			return addedExercise != null ? _exerciseSessionMapper.MapToDto(addedExercise) : null;
		}




		public async Task<ExerciseSessionDTO?> DeleteSessionByIdAsync(int id, int userId)
		{
			var sessionToDelete = await _exerciseSessionRepository.GetSessionsByIdAsync(id);

			if (sessionToDelete == null)
			{
				_logger?.LogError("ExersiceSession med ID {ExerciseId} ble ikke funnet for sletting", id);
				return null;
			}

			if (!(userId == sessionToDelete.AppUserId || (sessionToDelete.AppUserId != null && sessionToDelete.User.IsAdminUser)))
			{
				_logger?.LogError("User {UserId} har ikke tilgang til å slette denne Exercisesession", userId);
				throw new UnauthorizedAccessException($"User {userId} har ikke tilgang til å slette Exercisesession");
			}

			var deletedExerciseSession = await _exerciseSessionRepository.DeleteSessionsAsync(id);

			return deletedExerciseSession != null ? _exerciseSessionMapper.MapToDto(deletedExerciseSession) : null;

		}


		public async Task<ICollection<ExerciseSessionDTO>> GetAllSessionsAsync(int pageNr, int pageSize)
		{
			var sessions = await _exerciseSessionRepository.GetAllSessionsAsync(pageNr, pageSize);

			return sessions.Select(ExerciseSessions => _exerciseSessionMapper.MapToDto(ExerciseSessions)).ToList();


		}

		public async Task<ExerciseSessionDTO?> GetSessionByIdAsync(int id)
		{
			var sessionToGet = await _exerciseSessionRepository.GetSessionsByIdAsync(id);

			return sessionToGet != null ? _exerciseSessionMapper.MapToDto(sessionToGet) : null;
		}

		public async Task<ExerciseSessionDTO?> UpdateSessionAsync(ExerciseSessionDTO exerciseSession, int id)
		{
			var sessionToUpdtate = await _exerciseSessionRepository.GetSessionsByIdAsync(id);

			string userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException();
			}


			if (sessionToUpdtate == null || sessionToUpdtate.User == null)
			{
				_logger?.LogError("ExerciseSession med Id {exerciseId} ble ikke funnet for oppdatering.", id);
				return null;
			}


			if (userId != sessionToUpdtate.User.Id)
			{
				_logger?.LogError("User {LoggedInUserId} har ikke tilgang til å oppdatere denne exercise sessionen", userId);
				_logger?.LogError($"Detaljer: LoggedInUserId: {userId}, ExerciseSesUserId: {sessionToUpdtate.User.Id}");

				throw new UnauthorizedAccessException($"User {userId} har ikke tilgang til å oppdatere Exercise Session");
			}
			userId = sessionToUpdtate.User.Id;

			var updatedExerciseSession = await _exerciseSessionRepository.UpdateSessionsAsync(_exerciseSessionMapper.MapToModel(exerciseSession), id);

			if (updatedExerciseSession != null)
			{
				_logger?.LogInformation("Exercise session med ID {ExerciseSesId} ble oppdatert vellykket", id);
				return _exerciseSessionMapper.MapToDto(updatedExerciseSession);
			}

			return null;
		}
	}
}
