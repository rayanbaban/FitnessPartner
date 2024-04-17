using FitnessPartner.Data;
using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;

namespace FitnessPartner.Services
{
    public class NutritionResourcesService : INutritionResourcesService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<NutritionResources, NutritionResourcesDTO> _nutritionResourcesMapper;
        private readonly INutritionResourcesRepository _nutritionResourcesRepository;
        private readonly ILogger _logger;
        private readonly FitnessPartnerDbContext _dbContext;

        public async Task<NutritionResourcesDTO?> CreateNutritionResourceAsync(NutritionResourcesDTO nutritionResources, int loggedinUser)
        {
            var inloggedUser = await _userRepository.GetUserByIdAsync(loggedinUser);

            var nutritionResourcesToAdd = _nutritionResourcesMapper.MapToModel(nutritionResources);
            nutritionResources.ResourceId = loggedinUser;

            var addedNutritionResources = await _nutritionResourcesRepository.CreateNutritionResourceAsync(nutritionResourcesToAdd, loggedinUser);

            return addedNutritionResources != null ? _nutritionResourcesMapper.MapToDto(addedNutritionResources) : null;
        }

        public async Task<NutritionResourcesDTO?> DeleteNutritionResourceAsync(int userId, int resourcesId)
        {
            var nutritionResourceToDelete = await _nutritionResourcesRepository.GetNutritionResourceByIdAsync(resourcesId);

            if (nutritionResourceToDelete == null)
            {
                _logger?.LogError("NutritionLog med ID {NutritionLogId} ble ikke funnet for sletting", resourcesId);
                return null;
            }

            if (!(userId == nutritionResourceToDelete.ResourceId || (nutritionResourceToDelete.Content != null && nutritionResourceToDelete.User.IsAdminUser)))
            {
                _logger?.LogError("User {UserId} har ikke tilgang til å slette denne NutritionResources", userId);
                throw new UnauthorizedAccessException($"User {userId} har ikke tilgang til å slette NutritionResources");
            }

            var deletedNutritionResources = await _nutritionResourcesRepository.GetNutritionResourceByIdAsync(resourcesId);

            return nutritionResourceToDelete != null ? _nutritionResourcesMapper.MapToDto(nutritionResourceToDelete) : null;
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

        public async Task<NutritionResourcesDTO?> UpdateNutritionResourceAsync(NutritionResourcesDTO nutritionResourcesDTO, int resourceId, int loggedinUser)
        {
            var nutritionResourceToUpd = await _nutritionResourcesRepository.GetNutritionResourceByIdAsync(resourceId);

            if (nutritionResourceToUpd == null)
            {
                _logger?.LogError("NutritionResource med ID {NutritionResourceId} ble ikke funnet for oppdatering", resourceId);
                return null;
            }


            //if (memberId != exerciseToUpd.MemberID && !eventToUpdate.Member.IsAdminMember)
            //{
            //	_logger?.LogError("Medlem {LoggedInUserId} har ikke tilgang til å oppdatere dette arrangementet", loggedInMember);
            //	_logger?.LogError($"Detaljer: LoggedInMemberId: {loggedInMember}, EventMemberId: {eventToUpdate.MemberID}, IsAdminMember: {eventToUpdate.Member.IsAdminMember}");

            //	throw new UnauthorizedAccessException($"Medlem {loggedInMember} har ikke tilgang til å oppdatere arrangementet");
            //}

            var updatedNutritionResource = await _nutritionResourcesRepository.UpdateNutritionResourceAsync(_nutritionResourcesMapper.MapToModel(nutritionResourcesDTO), loggedinUser, resourceId);

            if (updatedNutritionResource != null)
            {
                _logger?.LogInformation("NutritionResource med ID {NutritionResourceId} ble oppdatert.", resourceId);
                return _nutritionResourcesMapper.MapToDto(updatedNutritionResource);
            }

            return null;
        }
    }
}
