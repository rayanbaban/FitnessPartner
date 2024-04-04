using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FitnessPartner.Services
{
	public class ExerciseLibraryService : IExerciseLibraryService
	{
		private readonly IExerciseLibraryRepository _exerciseLibraryRepo;
		private readonly IMapper<ExerciseLibrary, ExerciseLibraryDTO> _exerciseLibraryMapper;
		private readonly ILogger<ExerciseLibraryService> _logger;
		private readonly IUserRepository _userRepository;

		public ExerciseLibraryService(IExerciseLibraryRepository exerciseLibraryRepo, 
			IMapper<ExerciseLibrary,
			ExerciseLibraryDTO> exerciseLibraryMapper, 
			ILogger<ExerciseLibraryService> logger, IUserRepository userRepository)
		{
			_exerciseLibraryRepo = exerciseLibraryRepo;
			_exerciseLibraryMapper = exerciseLibraryMapper;
			_logger = logger;
			_userRepository = userRepository;
		}

		public async Task<ExerciseLibraryDTO?> AddExerciseLibraryAsync(/*int inloggedUser,*/ ExerciseLibraryDTO exerciseDTO)
		{
			//var loggedInMember = await _userRepository.GetUserByIdAsync(inloggedUser);

            var exerciseToAdd = _exerciseLibraryMapper.MapToModel(exerciseDTO);

            var addedExercise = await _exerciseLibraryRepo.CreateExerciseAsync(exerciseToAdd);

            return addedExercise != null ? _exerciseLibraryMapper.MapToDto(addedExercise) : null;
        }

		public async Task<ExerciseLibraryDTO?> DeleteExerciseAsync(int exerciseId, int userId)
		{
			var exerciseToDelete = await _exerciseLibraryRepo.GetExerciseByIdAsync(exerciseId);

			if (exerciseToDelete == null)
			{
				_logger?.LogError("Exercise med ID {ExerciseId} ble ikke funnet for sletting", exerciseId);
				return null;
			}


			var deletedExercise = await _exerciseLibraryRepo.DeleteExerciseAsync(exerciseId);

			return deletedExercise != null ? _exerciseLibraryMapper.MapToDto(deletedExercise) : null;
		}

		public async Task<ICollection<ExerciseLibraryDTO>> GetAllExerciesAsync(int pageNr, int pageSize)
		{
			var exercises = await _exerciseLibraryRepo.GetAllExercisesAsync(pageNr, pageSize);

			return exercises.Select(exercises => _exerciseLibraryMapper.MapToDto(exercises)).ToList();
		}

		public async Task<ExerciseLibraryDTO?> GetExerciseByIdAsync(int id)
		{
			var ExerciseToGet = await _exerciseLibraryRepo.GetExerciseByIdAsync(id);
			return ExerciseToGet != null ? _exerciseLibraryMapper.MapToDto(ExerciseToGet) : null;
		}

		public async Task<ExerciseLibraryDTO?> GetExerciseByNameAsync(string name)
		{
			var exerciseToGet = await _exerciseLibraryRepo.GetExerciseByMuscleNameAsync(name);

			return exerciseToGet != null ? _exerciseLibraryMapper.MapToDto(exerciseToGet) : null;
		}

		public async Task<ExerciseLibraryDTO?> UpdateExerciseAsync(int exerciseId, /*int userId,*/ ExerciseLibraryDTO exerciseDTO)
		{
			var exerciseToUpd = await _exerciseLibraryRepo.GetExerciseByIdAsync(exerciseId);
			
			if (exerciseToUpd == null)
			{
				_logger?.LogError("Exercise med ID {ExerciseId} ble ikke funnet for oppdatering", exerciseId);
				return null;
			}

			//if (memberId != exerciseToUpd.MemberID && !eventToUpdate.Member.IsAdminMember)
			//{
			//	_logger?.LogError("Medlem {LoggedInUserId} har ikke tilgang til å oppdatere dette arrangementet", loggedInMember);
			//	_logger?.LogError($"Detaljer: LoggedInMemberId: {loggedInMember}, EventMemberId: {eventToUpdate.MemberID}, IsAdminMember: {eventToUpdate.Member.IsAdminMember}");

			//	throw new UnauthorizedAccessException($"Medlem {loggedInMember} har ikke tilgang til å oppdatere arrangementet");
			//}

			var updatedExercise = await _exerciseLibraryRepo.UpdateExerciseAsync(_exerciseLibraryMapper.MapToModel(exerciseDTO), exerciseId);

			if (updatedExercise != null)
			{
				_logger?.LogInformation("Exercise med ID {exerciseId} ble oppdatert vellykket", exerciseId);
				return _exerciseLibraryMapper.MapToDto(updatedExercise);
			}
			return null;


		}
	}
}
