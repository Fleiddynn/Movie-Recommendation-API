using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication1.Entitites;
using WebApplication1.UserData;
using BCrypt.Net;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        public UserController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            List<UserDTO> userDTOs = new List<UserDTO>();
            if (users == null || !users.Any())
            {
                return NotFound("Sistemde hiç kullanıcı bulunmadı.");
            }
            foreach (var user in users){
                userDTOs.Add(new UserDTO(user));
            }
            return Ok(userDTOs);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            UserDTO userDTO = new UserDTO(user);
            if (userDTO == null)
            {
                return NotFound($"Aradığınız kullanıcı bulunamadı.");
            }
            return Ok(userDTO);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.email))
            {
                return BadRequest("Bu e-posta adresiyle zaten bir kullanıcı kayıtlı.");
            }

            var user = new User
            {
                email = dto.email,
                first_name = dto.first_name,
                last_name = dto.last_name,
                password = BCrypt.Net.BCrypt.HashPassword(dto.password),
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                if (id.ToString() != updatedUser.Id)
                {
                    return BadRequest("Kullanıcı ID'leri değiştirilemez.");
                }
                _context.Entry(updatedUser).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Users.AnyAsync(e => e.Id == id.ToString()))
                {
                    return NotFound($"{id} ye sahip kullanıcı bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Güncelleme sırasında bir hata oluştu: {ex.Message}");
            }
            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateUser(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"Aradığınız kullanıcı bulunamadı.");
            }
            patchDoc.ApplyTo(user, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Users.AnyAsync(e => e.Id== id.ToString()))
                {
                    return NotFound($"{id} ye sahip kullanıcı bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Güncelleme sırasında bir hata oluştu: {ex.Message}");
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"Aradığınız kullanıcı bulunamadı.");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
