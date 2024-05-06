using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;
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
				throw new UnauthorizedAccessException("Må være innlogget");
			}

			var inloggedAppUser = await _usermanager.FindByIdAsync(userId);
			if (inloggedAppUser is null)
			{
				throw new UnauthorizedAccessException();
			}

			exerciseToAdd.User = inloggedAppUser;
			
			var addedExercise = await _exerciseSessionRepository.AddSessionAsync(exerciseToAdd);


			return addedExercise != null ? _exerciseSessionMapper.MapToDto(addedExercise) : null;
		}




		public async Task<ExerciseSessionDTO?> DeleteSessionByIdAsync(int id)
		{
			string userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException();
			}

			var inloggedAppUser = await _usermanager.FindByIdAsync(userId);
			if (inloggedAppUser == null)
			{
				throw new UnauthorizedAccessException();
			}

			var sessionToDelete = await _exerciseSessionRepository.GetSessionsByIdAsync(id);
			if (sessionToDelete == null)
			{
				_logger?.LogError("ExersiceSession med ID {ExerciseId} ble ikke funnet for sletting", id);
				return null;
			}

			// Sjekk om økten tilhører den innloggede brukeren
			if (sessionToDelete.User?.Id != inloggedAppUser.Id)
			{
				throw new UnauthorizedAccessException("Du har ikke tilgang til å slette dette");
			}

			// Slett økten og returner DTO
			var deletedExerciseSession = await _exerciseSessionRepository.DeleteSessionsAsync(id);
			return deletedExerciseSession != null ? _exerciseSessionMapper.MapToDto(deletedExerciseSession) : null;
		}



		public async Task<ExerciseSessionDTO?> UpdateSessionAsync(ExerciseSessionDTO exerciseSession, int id)
		{
			var sessionToUpdtate = await _exerciseSessionRepository.GetSessionsByIdAsync(id);


			string userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;

			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("Må være inlogget");
			}


			if (sessionToUpdtate == null)
			{
				_logger?.LogError("ExerciseSession med Id {exerciseId} ble ikke funnet for oppdatering.", id);
				throw new UnauthorizedAccessException($"ExerciseSession med Id {id} ble ikke funnet for oppdatering");
			}

			var inloggedAppUser = await _usermanager.FindByIdAsync(userId);
			if (sessionToUpdtate?.User?.Id != inloggedAppUser?.Id)
			{
				throw new UnauthorizedAccessException("Ingen tilgang til denne handlingen");

			}


			var updatedExerciseSession = await _exerciseSessionRepository.UpdateSessionsAsync(_exerciseSessionMapper.MapToModel(exerciseSession), id);

			if (updatedExerciseSession != null)
			{
				_logger?.LogInformation("Exercise session med ID {ExerciseSesId} ble oppdatert vellykket", id);
				return _exerciseSessionMapper.MapToDto(updatedExerciseSession);
			}

			return null;
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

		public async Task<ICollection<ExerciseSessionDTO>> GetSessionsByUserIdAsync(string userId, int pageNr, int pageSize)
		{
			var sessions = await _exerciseSessionRepository.GetSessionsByUserId(userId, pageNr, pageSize);

			if (sessions == null)
				return null;
			return sessions.Select(exerciseSessions => _exerciseSessionMapper.MapToDto(exerciseSessions)).ToList();
				
		}
	}
}
