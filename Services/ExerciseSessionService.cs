using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace FitnessPartner.Services
{
	public class ExerciseSessionService : IExerciseSessionService
	{
		private readonly IExersiceSessionRepository _exerciseSessionRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper<ExerciseSession, ExerciseSessionDTO> _exerciseSessionMapper;
		private readonly IMapper<User, UserDTO> _UserMapper;
		private readonly ILogger<ExerciseSessionService> _logger;

		public ExerciseSessionService(
			IExersiceSessionRepository exerciseSessionRepository,
			IUserRepository userRepository, 
			IMapper<ExerciseSession, ExerciseSessionDTO> exerciseSessionMapper,
			IMapper<User, UserDTO> userMapper, 
			ILogger<ExerciseSessionService> logger)
		{
			_exerciseSessionRepository = exerciseSessionRepository;
			_userRepository = userRepository;
			_exerciseSessionMapper = exerciseSessionMapper;
			_UserMapper = userMapper;
			_logger = logger;
		}

		public async Task<ExerciseSessionDTO?> AddSessionAsync(ExerciseSessionDTO exerciseSessionDTO, int inloggedUser)
		{
			var loggedInUser = await _userRepository.GetUserByIdAsync(inloggedUser);

			var exerciseToAdd = _exerciseSessionMapper.MapToModel(exerciseSessionDTO);
			exerciseSessionDTO.UserId = inloggedUser;

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

			if (!(userId == sessionToDelete.UserId || (sessionToDelete.UserId != null && sessionToDelete.User.IsAdminUser)))
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

		public async Task<ExerciseSessionDTO?> UpdateSessionAsync(ExerciseSessionDTO exerciseSession, int inloggedUser ,int id)
		{
			var sessionToUpdtate = await _exerciseSessionRepository.GetSessionsByIdAsync(id);

			if (sessionToUpdtate == null || sessionToUpdtate.User == null)
			{
				_logger?.LogError("ExerciseSession med Id {exerciseId} ble ikke funnet for oppdatering.", id);
				return null;
			}


			if (inloggedUser != sessionToUpdtate.UserId && !sessionToUpdtate.User.IsAdminUser)
			{
				_logger?.LogError("User {LoggedInUserId} har ikke tilgang til å oppdatere denne exercise sessionen", inloggedUser);
				_logger?.LogError($"Detaljer: LoggedInUserId: {inloggedUser}, ExerciseSesUserId: {sessionToUpdtate.UserId}, IsAdminUser: {sessionToUpdtate.User.IsAdminUser}");

				throw new UnauthorizedAccessException($"User {inloggedUser} har ikke tilgang til å oppdatere Exercise Session");
			}

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
