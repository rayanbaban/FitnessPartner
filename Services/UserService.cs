﻿using FitnessPartner.Data;
using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FitnessPartner.Services
{
    public class UserService : IUserService
    {
        private readonly FitnessPartnerDbContext _dbContext;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper<User, UserDTO> _userMapper;
        private readonly IUserRepository _userRepository;
        private readonly IMapper<User, UserRegDTO> _userRegMapper;
        private readonly HttpContext httpContext;

        public UserService(FitnessPartnerDbContext dbContext,
			ILogger<UserRepository> logger,
			IMapper<User, UserDTO> userMapper,
			IUserRepository userRepository, IMapper<User,
			UserRegDTO> userRegMapper, HttpContext httpContext)
		{
			_dbContext = dbContext;
			_logger = logger;
			_userMapper = userMapper;
			_userRepository = userRepository;
			_userRegMapper = userRegMapper;
			this.httpContext = httpContext;
		}

		public async Task<UserDTO?> DeleteUserAsync(int deleteUserId, int inloggedUser)
        {
				try
				{
					var loggedInUserId = GetLoggedInUserIdFromToken(); // Få brukerens ID fra JWT-tokenet

					if (loggedInUserId == null)
					{
						_logger?.LogWarning("UserId er ikke satt i HttpContext.Items");
						return Unauthorized("Ikke autentisert");
					}

					var userToDelete = await _userRepository.GetUserByIdAsync(id);

					if (userToDelete == null)
					{
						return NotFound($"Fant ikke bruker med ID: {id}");
					}

					// Sjekk om den autentiserte brukeren er admin eller eieren av kontoen som skal slettes
					if (IsAuthorized(loggedInUserId.Value, userToDelete))
					{
						var deletedUser = await _userService.DeleteUserAsync(id, loggedInUserId.Value);
						_logger?.LogInformation("Deleted user: {@DeletedUser}", deletedUser);
						return deletedUser != null ? Ok(deletedUser) : NotFound($"Klarte ikke å slette bruker med ID: {id}");
					}
					else
					{
						_logger?.LogWarning("Bruker {UserId} har ikke tilgang til å slette bruker med ID: {DeleteUserId}", loggedInUserId, id);
						return Forbid(); // Returner 403 Forbidden hvis brukeren ikke har tilgang
					}
				}
				catch (Exception ex)
				{
					_logger?.LogError(ex, "Feil ved sletting av bruker");
					return BadRequest($"Feil ved sletting av bruker: {ex.Message}");
				}

			//try
			//{
			//    _logger?.LogInformation("Forsøker å slette bruker med ID {DeleteUserId} av Bruker med ID {InloggedUser}", DeleteUserAsync, inloggedUser);

			//    // Sjekk om brukeren som prøver å slette er admin eller eier av kontoen som skal slettes
			//    var loginUser = await _userRepository.GetUserByIdAsync(inloggedUser);
			//    var userToDelete = await _userRepository.GetUserByIdAsync(deleteUserId);

			//    if (loginUser == null || userToDelete == null)
			//    {
			//        // Hvis inlogget bruker som skal slettes ikke er funnet, kast en feil
			//        _logger?.LogError("Inlogget Bruker som skal slettes ble ikke funnet. Inlogget bruker:" +
			//            " {@LoginUser}, Bruker som skal slettes: {@UserToDelete}", loginUser, userToDelete);
			//        throw new InvalidOperationException("Inlogget bruker som skal slettes ble ikke funnet.");
			//    }

			//    if (inloggedUser != deleteUserId && !loginUser.IsAdminUser)
			//    {
			//        // Hvis inlogget bruker ikke er admin og prøver å slette en annen bruker, meld en feil
			//        _logger?.LogError("Ikke autorisert: Bruker {InloggedBruker} har ikke tilgang til å slette bruker" +
			//            " {DeleteUserId}", inloggedUser, deleteUserId);
			//        throw new UnauthorizedAccessException($"Bruker {inloggedUser} har ikke tilgang til å slette Bruker {deleteUserId}");
			//    }

			//    // Slett bruker fra databasen
			//    var deletedUser = await _userRepository.DeleteUserByIdAsync(deleteUserId);

			//    _logger?.LogInformation("Bruker med ID {DeleteUserId} ble slettet av Bruker med ID {InloggedUser}", deleteUserId, inloggedUser);

			//    return deletedUser != null ? _userMapper.MapToDto(deletedUser) : null;
			//}
			//catch (Exception ex)
			//{
			//    _logger?.LogError(ex, "Feil ved sletting av medlem med ID {DeleteMemberId}", deleteUserId);
			//    throw;
			//}
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


  //      public async Task<int> GetAuthenticatedIdAsync(string userName, string password)
  //      {

		//	var user = await _userRepository.GetUserByNameAsync(userName);
		//	if (user == null) return 0;

		//	// prøver å verifisere passordet mot lagret hash-verdi
		//	if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
		//	{
		//		return user.UserId;
		//	}
		//          return 0;
		//}

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

        public async Task<UserDTO> UpdateUserAsync(int id, UserDTO userDTO, int inloggedUser)
        {
            try
            {
                _logger?.LogInformation("Forsøker å oppdatere bruker med ID {BrukerId} av Bruker med ID {LoginUserId}", id, inloggedUser);

                // Sjekk om brukeren som prøver å oppdatere er admin eller eier av kontoen som skal oppdateres
                var loginUser = await _userRepository.GetUserByIdAsync(inloggedUser);
                var userToUpdate = await _userRepository.GetUserByIdAsync(id);

                _logger?.LogInformation("Inlogget Bruker: {@LoginUser}", loginUser);
                _logger?.LogInformation("Bruker som skal oppdateres: {@UserToUpdate}", userToUpdate);


                if (loginUser == null || userToUpdate == null)
                {
                    // Hvis enten inlogget bruker eller bruker som skal oppdateres ikke er funnet, meld en feil
                    _logger?.LogError("Inlogget Bruker som skal oppdateres ble ikke funnet. Inlogget Bruker: {@LoginUser}, " +
                        "Bruker som skal oppdateres: {@BrukerToUpdate}", loginUser, userToUpdate);
                    throw new InvalidOperationException("Inlogget Bruker som skal oppdateres ble ikke funnet.");
                }

                if (id != inloggedUser && !loginUser.IsAdminUser)
                {
                    // Hvis inlogget bruker ikke er admin og prøver å oppdatere en annen bruker, meld en feil
                    _logger?.LogError("Ikke autorisert: Bruker {LoginUserId} har ikke tilgang til å oppdatere Bruker {BrukerId}", inloggedUser, id);
                    throw new UnauthorizedAccessException($"Bruker {inloggedUser} har ikke tilgang til å oppdatere Bruker {id}");
                }

                // Oppdater brukeren med de nye verdiene
                var updatedUser = _userMapper.MapToModel(userDTO);
                updatedUser.UserId = id;


                // Oppdater brukeren i databasen
                await _userRepository.UpdateUserAsync(id, userToUpdate);


                // Hent den oppdaterte brukeren fra databasen
                var result = await _userRepository.GetUserByIdAsync(id);

                _logger?.LogInformation("Bruker med ID {BrukerId} ble oppdatert av Bruker med ID {LoginUserId}", id, inloggedUser);

                return result != null ? _userMapper.MapToDto(result) : null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Feil ved oppdatering av bruker med ID {MemberId}", id);
                throw;
            }
        }
		private bool IsAuthorized(int loginUserId, UserDTO user)
		{
			// Sjekk om brukeren har tilgang til å slette den angitte brukeren
			return loginUserId == user.UserId || user.IsUserAdmin;
		}
	}
}
