using FitnessPartner.Models.DTOs;
using FitnessPartner.OtherObjects;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]



	// Implementere delete, update
	public class UserController : ControllerBase
    {
        private readonly IUserService _usersService;
        private readonly ILogger<UserController> _logger;

		public UserController(IUserService usersService, ILogger<UserController> logger)
		{
			_usersService = usersService;
			_logger = logger;
		}

		[HttpGet(Name = "GetAllUsers")]
		[Authorize(Roles = StaticUserRoles.USER)]
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

            var updatedUser = await _usersService.UpdateUserAsync(id, userDTO);

            return updatedUser != null ?
                       Ok(updatedUser) :
                       NotFound($"Klarte ikke å oppdatere bruker med ID: {id}");
        }

        [HttpDelete(Name = "DeleteUser")]
        public async Task<ActionResult<UserDTO>> DeleteUserAsync(int id)
        {
            try
            {

                var userToDelete = await _usersService.GetUserByIdAsync(id);


                var deletedUser = await _usersService.DeleteUserAsync(id);
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
        }
    }
}
