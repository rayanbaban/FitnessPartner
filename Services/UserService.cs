using FitnessPartner.Data;
using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;

namespace FitnessPartner.Services
{
    public class UserService : IUserService
    {
        private readonly FitnessPartnerDbContext _dbContext;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper<User, UserDTO> _userMapper;
        private readonly IUserRepository _userRepository;

        public async Task<UserDTO?> DeleteUserAsync(int deleteUserId, int inloggedUser)
        {
            try
            {
                _logger?.LogInformation("Forsøker å slette bruker med ID {DeleteUserId} av Bruker med ID {InloggedUser}", DeleteUserAsync, inloggedUser);

                // Sjekk om brukeren som prøver å slette er admin eller eier av kontoen som skal slettes
                var loginUser = await _userRepository.GetUserByIdAsync(inloggedUser);
                var userToDelete = await _userRepository.GetUserByIdAsync(deleteUserId);

                if (loginUser == null || userToDelete == null)
                {
                    // Hvis inlogget bruker som skal slettes ikke er funnet, kast en feil
                    _logger?.LogError("Inlogget Bruker som skal slettes ble ikke funnet. Inlogget bruker:" +
                        " {@LoginUser}, Bruker som skal slettes: {@UserToDelete}", loginUser, userToDelete);
                    throw new InvalidOperationException("Inlogget bruker som skal slettes ble ikke funnet.");
                }

                if (inloggedUser != deleteUserId && !loginUser.IsAdminUser)
                {
                    // Hvis inlogget bruker ikke er admin og prøver å slette en annen bruker, meld en feil
                    _logger?.LogError("Ikke autorisert: Bruker {InloggedBruker} har ikke tilgang til å slette bruker" +
                        " {DeleteUserId}", inloggedUser, deleteUserId);
                    throw new UnauthorizedAccessException($"Bruker {inloggedUser} har ikke tilgang til å slette Bruker {deleteUserId}");
                }

                // Slett bruker fra databasen
                var deletedUser = await _userRepository.DeleteUserByIdAsync(deleteUserId);

                _logger?.LogInformation("Bruker med ID {DeleteUserId} ble slettet av Bruker med ID {InloggedUser}", deleteUserId, inloggedUser);

                return deletedUser != null ? _userMapper.MapToDto(deletedUser) : null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Feil ved sletting av medlem med ID {DeleteMemberId}", deleteUserId);
                throw;
            }
        }

        public async Task<ICollection<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            _logger?.LogInformation("Hentet alle medlemmer");
            //Mapping
            var dtos = users.Select(user => _userMapper.MapToDto(user)).ToList();
            return dtos;
        }

        public Task<int> GetAuthenticatedIdAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> UpdateUserAsync(int id, UserDTO userDTO, int inloggedUser)
        {
            throw new NotImplementedException();
        }
    }
}
