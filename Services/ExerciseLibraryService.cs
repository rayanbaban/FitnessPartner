using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FitnessPartner.Services
{
	public class ExerciseLibraryService : IExerciseLibraryService
	{
		private readonly IExerciseLibraryRepository _exerciseLibraryRepo;
		private readonly IMapper<ExerciseLibrary, ExerciseLibraryDTO> _exerciseLibraryMapper;
		private readonly ILogger<ExerciseLibraryService> _logger;
		private readonly IUserRepository _userRepository;
		
		public async Task<ExerciseLibraryDTO?> AddExerciseLibraryAsync(int inloggedUser, ExerciseLibraryDTO exerciseDTO)
		{
			var loggedInMember = await _userRepository.GetUserByIdAsync(inloggedUser);

			var exerciseToAdd = _exerciseLibraryMapper.MapToModel(exerciseDTO);

			var addedExercise = await _exerciseLibraryRepo.CreateExerciseAsync(exerciseToAdd);

			return addedExercise != null ? _exerciseLibraryMapper.MapToDto(addedExercise) : null;
		}

		public async Task<ExerciseLibraryDTO?> DeleteExerciseAsync(int exerciseId, int userId)
		{
			var eventToDelete = await _exerciseLibraryRepo.GetExerciseByIdAsync(exerciseId);

			if (eventToDelete == null)
			{
				_logger?.LogError("Exercise med ID {ExerciseId} ble ikke funnet for sletting", exerciseId);
				return null;
			}

			if (!(userId == eventToDelete.id || (eventToDelete.Member != null && eventToDelete.Member.IsAdminMember)))
			{
				_logger?.LogError("Medlem {MemberId} har ikke tilgang til å slette dette arrangementet", memberId);
				throw new UnauthorizedAccessException($"Medlem {memberId} har ikke tilgang til å slette arrangementet");
			}

			var deletedEvent = await _eventRepository.DeleteEventByIdAsync(eventId);

			return deletedEvent != null ? _eventMapper.MapToDTO(deletedEvent) : null;
		}

		public Task<ICollection<ExerciseLibraryDTO?>> GetAllExerciesAsync(int pageNr, int pageSize)
		{
			throw new NotImplementedException();
		}

		public Task<ExerciseLibraryDTO?> GetExerciseByNameAsync(string name)
		{
			throw new NotImplementedException();
		}

		public Task<ExerciseLibraryDTO?> UpdateExerciseAsync(int exerciseId, int memberId, ExerciseLibraryDTO exerciseDTO)
		{
			throw new NotImplementedException();
		}
	}
}
