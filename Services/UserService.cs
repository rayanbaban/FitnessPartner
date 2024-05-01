using FitnessPartner.Data;
using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FitnessPartner.Services
{
    public class UserService : IUserService
    {
        private readonly FitnessPartnerDbContext _dbContext;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper<AppUser, UserDTO> _userMapper;
        private readonly IUserRepository _userRepository;
        private readonly IMapper<AppUser, UserRegDTO> _userRegMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;



		public UserService(FitnessPartnerDbContext dbContext,
			ILogger<UserRepository> logger,
			IMapper<AppUser, UserDTO> userMapper,
			IUserRepository userRepository, IMapper<AppUser,
			UserRegDTO> userRegMapper, IHttpContextAccessor httpContext, UserManager<AppUser> userManager)
		{
			_dbContext = dbContext;
			_logger = logger;
			_userMapper = userMapper;
			_userRepository = userRepository;
			_userRegMapper = userRegMapper;
			_httpContextAccessor = httpContext;
			_userManager = userManager;
		}

		public async Task<UserDTO?> DeleteUserAsync(int deleteUserId)
        {

            try
            {

                var loginUser = await _userRepository.GetUserByIdAsync(deleteUserId);

				if (loginUser == null || deleteUserId == null)
                {
                    // Hvis inlogget bruker som skal slettes ikke er funnet, kast en feil
                    _logger?.LogError("Inlogget Bruker som skal slettes ble ikke funnet. Inlogget bruker:" +
                        " {@LoginUser}, Bruker som skal slettes: {@UserToDelete}", loginUser, deleteUserId);
                    throw new InvalidOperationException("Inlogget bruker som skal slettes ble ikke funnet.");
                }

				var userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;
				if (string.IsNullOrEmpty(userId))
				{
					throw new UnauthorizedAccessException("Ugyldig bruker.");
				}

				var loggedInUser = await _userManager.FindByIdAsync(userId);

				if (loginUser.Id != loggedInUser.Id)
				{
					throw new UnauthorizedAccessException("Du har ikke tilgang til å slette denne NutritionLog");
				}

				var deletedUser = await _userRepository.DeleteUserByIdAsync(deleteUserId);

                _logger?.LogInformation("Bruker med ID {DeleteUserId} ble slettet av Bruker med ID {InloggedUser}", deleteUserId, userId);

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


        public async Task<UserLoginDTO?> GetAuthenticatedIdAsync(string userName, string password)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            if (user == null) return null;

            // prøver å verifisere passordet mot lagret hash-verdi
            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new UserLoginDTO
                {
                    PasswordHash = user.PasswordHash,
                    Username = user.UserName,
                };
            }
            return null;
        }


        public async Task<IEnumerable<UserDTO>> GetPageAsync(int pageNr, int pageSize)
        {
            var res = await _userRepository.GetPageAsync(pageNr, pageSize);

            _logger?.LogInformation("Forsøker å hente side {PageNr} med størrelse {PageSize} av brukere", pageNr, pageSize);

            return res.Select(pages => _userMapper.MapToDto(pages)).ToList();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var res = await _userRepository.GetUserByIdAsync(userId);

            _logger?.LogInformation("Forsøker å hente Bruker med ID {@brukerId}", userId);

            return res != null ? _userMapper.MapToDto(res) : null;
        }


        public async Task<UserDTO?> RegisterUserAsync(UserRegDTO userRegDTO)
        {
            var user = _userRegMapper.MapToModel(userRegDTO);

            //Lage salt og hashverdier
            user.Salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegDTO.Password, user.Salt);

            // legge til ny bruker
            var newUser = await _userRepository.AddUserAsync(user);

            // logge registreringen av den nye brukeren

            _logger.LogInformation($"Et nytt medlem har registrert seg: {user}");

            return _userMapper.MapToDto(newUser);
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UserDTO userDTO)
        {
            try
            {

                var userToUpdate = await _userRepository.GetUserByIdAsync(id);

				if (userToUpdate == null)
				{

					_logger?.LogError("NutritionLog med ID {NutritionLogId} ble ikke funnet for oppdatering", id);
					return null;
				}

				var userId = _httpContextAccessor!.HttpContext!.Items["UserId"]!.ToString() ?? string.Empty;

				if (string.IsNullOrEmpty(userId))
				{

					throw new UnauthorizedAccessException("Ugyldig bruker.");
				}


				var loggedInUser = await _userManager.FindByIdAsync(userId);
				if (userToUpdate.Id != loggedInUser.Id)
				{
					throw new UnauthorizedAccessException("Du har ikke tilgang til å oppdatere denne NutritionLog");
				}

				_logger?.LogInformation("Inlogget Bruker: {@LoginUser}", userId);
                _logger?.LogInformation("Bruker som skal oppdateres: {@UserToUpdate}", userToUpdate);


                var updatedUser = _userMapper.MapToModel(userDTO);
                updatedUser.Id = userId;


                await _userRepository.UpdateUserAsync(id, userToUpdate);


                var result = await _userRepository.GetUserByIdAsync(id);

                _logger?.LogInformation("Bruker med ID {BrukerId} ble oppdatert av Bruker med ID {LoginUserId}", id, userId);

                return result != null ? _userMapper.MapToDto(result) : null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Feil ved oppdatering av bruker med ID {MemberId}", id);
                throw;
            }
        }
    }
}
