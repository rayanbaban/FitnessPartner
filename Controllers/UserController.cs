using FitnessPartner.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPartner.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Dette er bare en dummy-brukerliste for demonstrasjonsformål.
        private static List<User> users = new List<User>
        {
            new User { Id = 1, UserName = "user1", PasswordHash = "hashedPassword1" },
            new User { Id = 2, UserName = "user2", PasswordHash = "hashedPassword2" }
        };

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = users.Find(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var index = users.FindIndex(u => u.Id == id);
            if (index == -1)
            {
                return NotFound();
            }

            users[index] = updatedUser;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = users.Find(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            users.Remove(user);
            return NoContent();
        }
    }
}
