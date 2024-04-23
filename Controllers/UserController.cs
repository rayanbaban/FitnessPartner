using FitnessPartner.Models.DTOs;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]


    // Implementere delete, update
    public class UserController : ControllerBase
    {
        private readonly IUserService _usersService;
        private readonly ILogger<UserController> _logger;
        private readonly IHttpContextAccessor httpContextAccessor;

		public UserController(IUserService usersService, ILogger<UserController> logger, IHttpContextAccessor httpContextAccessor)
		{
			_usersService = usersService;
			_logger = logger;
			this.httpContextAccessor = httpContextAccessor;
		}

		[HttpGet(Name = "GetAllUsers")]
        public async Task<ActionResult<ICollection<UserDTO>>> GetAllUsersAsync(int pageNr = 1, int pageSize = 10)
        {
            return Ok(await _usersService.GetPageAsync(pageNr, pageSize));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int id)
        {
            var res = await _usersService.GetUserByIdAsync(id);
            return res != null ? Ok(res) : NotFound("Fant Ikke Bruker");
        }


        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task<ActionResult<UserDTO>> UpdateUserAsync(int id, UserDTO userDTO)
        {
            int loginUserId = (int)HttpContext.Items["UserId"]!;

            var updatedUser = await _usersService.UpdateUserAsync(id, userDTO, loginUserId);

            return updatedUser != null ?
                       Ok(updatedUser) :
                       NotFound($"Klarte ikke å oppdatere bruker med ID: {id}");
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<ActionResult<UserDTO>> DeleteUserAsync(int id)
        {
            try
            {
                // Sjekk om brukeren er autentisert
                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    _logger?.LogWarning("UserId er ikke satt i HttpContext.Items");
                    throw new UnauthorizedAccessException();
                }

                // Hent ID-en til den påloggede brukeren fra JWT-tokenet
                int? loginUserId = (int?)HttpContext.Items["UserId"];

                // Hent informasjon om brukeren som skal slettes
                var userToDelete = await _usersService.GetUserByIdAsync(id);

                // Sjekk om den påloggede brukeren har tilgang til å slette den angitte brukeren
                if (!IsAuthorized(loginUserId.Value, userToDelete))
                {
                    _logger?.LogWarning("Bruker {UserId} har ikke tilgang til å slette bruker med ID: {DeleteUserId}", loginUserId, id);
                    throw new UnauthorizedAccessException("Ikke autorisert");
                }

                // Fortsett med sletting av brukeren
                var deletedUser = await _usersService.DeleteUserAsync(id, loginUserId.Value);
                _logger?.LogInformation("Deleted user: {@DeletedUser}", deletedUser);

                return deletedUser != null ?
                           deletedUser :
                           throw new Exception($"Klarte ikke å slette bruker med ID: {id}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Feil ved sletting av bruker");
                throw new Exception($"Feil ved sletting av bruker: {ex.Message}");
            }
            //try
            //{
            //    if (!HttpContext.Items.ContainsKey("UserId"))
            //    {
            //        _logger?.LogWarning("UserId er ikke satt i HttpContext.Items");
            //        return Unauthorized("Ikke autentisert");
            //    }

            //    int? loginUserId = (int?)HttpContext.Items["UserId"];

            //    if (!loginUserId.HasValue || loginUserId == 0)
            //    {
            //        _logger?.LogWarning("UserId er ugyldig: {UserId}", loginUserId);
            //        return Unauthorized("Ikke autentisert");
            //    }

            //    var deletedUser = await _usersService.DeleteUserAsync(id, loginUserId.Value);
            //    _logger?.LogInformation("Deleted user: {@DeletedUser}", deletedUser);

            //    return deletedUser != null ?
            //               Ok(deletedUser) :
            //               NotFound($"Klarte ikke å slette bruker med ID: {id}");
            //}
            //catch (Exception ex)
            //{
            //    _logger?.LogError(ex, "Feil ved sletting av bruker");
            //    return BadRequest($"Feil ved sletting av bruker: {ex.Message}");
            //}
        }
        private bool IsAuthorized(int loginUserId, UserDTO user)
        {
            // Sjekk om brukeren har tilgang til å slette den angitte brukeren
            return loginUserId == user.UserId || user.IsUserAdmin;
        }
    }
}
