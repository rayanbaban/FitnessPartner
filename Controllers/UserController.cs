using FitnessPartner.Models.DTOs;
using FitnessPartner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]


    // Implementere delete, update
    public class UserController : ControllerBase
    {
        private readonly IUserService _usersService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService usersservice, ILogger<UserController> logger)
        {
            _usersService = usersservice;
            _logger = logger;
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
                if (!HttpContext.Items.ContainsKey("UserId"))
                {
                    _logger?.LogWarning("UserId er ikke satt i HttpContext.Items");
                    return Unauthorized("Ikke autentisert");
                }

                int? loginUserId = (int?)HttpContext.Items["UserId"];

                if (!loginUserId.HasValue || loginUserId == 0)
                {
                    _logger?.LogWarning("UserId er ugyldig: {UserId}", loginUserId);
                    return Unauthorized("Ikke autentisert");
                }

                var deletedUser = await _usersService.DeleteUserAsync(id, loginUserId.Value);
                _logger?.LogInformation("Deleted user: {@DeletedUser}", deletedUser);

                return deletedUser != null ?
                           Ok(deletedUser) :
                           NotFound($"Klarte ikke å slette bruker med ID: {id}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Feil ved sletting av bruker");
                return BadRequest($"Feil ved sletting av bruker: {ex.Message}");
            }
        }

    }

}
