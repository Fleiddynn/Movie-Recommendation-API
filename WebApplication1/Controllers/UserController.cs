using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication1.Entitites;
using WebApplication1.DbContexts.UserData;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication1.DbContexts;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(AllDbContext context)
        {
            _userRepository = new UserRepository(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
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
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            UserDTO userDTO = new UserDTO(user);
            if (userDTO == null)
            {
                return NotFound($"Aradığınız kullanıcı bulunamadı.");
            }
            return Ok(userDTO);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var user = await _userRepository.GetUsersAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(u => u.email == dto.email));
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.password, user.password))
            {
                return Unauthorized("E-posta veya şifre yanlış.");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.email),
                new Claim("first_name", user.first_name),
                new Claim("last_name", user.last_name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Issuer"],
                audience: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new UserDTO(user)
            });
        }
        [HttpGet("login/google")]
        public IActionResult GoogleLogin()
        {
            var returnUrl = "/";
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse", new { returnUrl }) };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("/login/google-response")]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Cookies");
            if (!authenticateResult.Succeeded)
                return BadRequest();

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var firstName = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastName = authenticateResult.Principal.FindFirst(ClaimTypes.Surname)?.Value;

            var user = await _userRepository.GetUsersAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(u => u.email == email));
            if (user == null)
            {
                user = new User
                {
                    email = email,
                    first_name = firstName ?? "",
                    last_name = lastName ?? "",
                    password = "",
                    social_login_provider = "Google",
                    created_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow
                };
                _userRepository.Create(user);
                await _userRepository.Update(user);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.email),
                new Claim("first_name", user.first_name),
                new Claim("last_name", user.last_name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Issuer"],
                audience: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1000),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new UserDTO(user)
            });
        }
        [HttpGet("facebook-login")]
        public IActionResult FacebookLogin(string returnUrl = "/")
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse", new { returnUrl }) };
            return Challenge(properties, "Facebook");
        }

        [HttpGet("facebook-response")]
        public async Task<IActionResult> FacebookResponse(string returnUrl = "/")
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return BadRequest();

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;

            string firstName = "";
            string lastName = "";
            if (!string.IsNullOrEmpty(name))
            {
                var parts = name.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                firstName = parts.Length > 0 ? parts[0] : "";
                lastName = parts.Length > 1 ? parts[1] : "";
            }

            var user = await _userRepository.GetUsersAsync().ContinueWith(t => t.Result.FirstOrDefault(u => u.email == email));
            if (user == null)
            {
                user = new User
                {
                    email = email,
                    first_name = firstName,
                    last_name = lastName,
                    password = "",
                    social_login_provider = "Facebook",
                    created_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow
                };
                _userRepository.Create(user);
                await _userRepository.Update(user);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.email),
                new Claim("first_name", user.first_name),
                new Claim("last_name", user.last_name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Issuer"],
                audience: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1000),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new UserDTO(user)
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserChDTO dto)
        {
            if (await _userRepository.GetUsersAsync()
                .ContinueWith(t => t.Result.Any(u => u.email == dto.email)))
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

            _userRepository.Create(user);
            await _userRepository.Update(user);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserChDTO dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Aradığınız kullanıcı bulunamadı.");
            }

            if (!string.IsNullOrEmpty(dto.email) && dto.email != user.email)
            {
                if (await _userRepository.GetUsersAsync()
                    .ContinueWith(t => t.Result.Any(u => u.email == dto.email)))
                {
                    return BadRequest("Bu e-posta adresiyle zaten bir kullanıcı kayıtlı.");
                }
                user.email = dto.email;
            }

            if (!string.IsNullOrEmpty(dto.first_name))
                user.first_name = dto.first_name;

            if (!string.IsNullOrEmpty(dto.last_name))
                user.last_name = dto.last_name;

            if (!string.IsNullOrEmpty(dto.password))
                user.password = BCrypt.Net.BCrypt.HashPassword(dto.password);

            user.updated_at = DateTime.UtcNow;

            try
            {
                await _userRepository.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userRepository.GetUsersAsync()
                    .ContinueWith(t => t.Result.Any(e => e.Id == id)))
                {
                    return NotFound($"{id} idsine sahip kullanıcı bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Güncelleme sırasında bir hata oluştu: {ex.Message}");
            }
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateUser(Guid id, [FromBody] UserChDTO dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Aradığınız kullanıcı bulunamadı.");
            }

            if (!string.IsNullOrEmpty(dto.email) && dto.email != user.email)
            {
                if (await _userRepository.GetUsersAsync()
                    .ContinueWith(t => t.Result.Any(u => u.email == dto.email)))
                {
                    return BadRequest("Bu e-posta adresiyle zaten bir kullanıcı kayıtlı.");
                }
                user.email = dto.email;
            }

            if (!string.IsNullOrEmpty(dto.first_name))
                user.first_name = dto.first_name;

            if (!string.IsNullOrEmpty(dto.last_name))
                user.last_name = dto.last_name;

            if (!string.IsNullOrEmpty(dto.password))
                user.password = BCrypt.Net.BCrypt.HashPassword(dto.password);

            user.updated_at = DateTime.UtcNow;

            try
            {
                await _userRepository.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userRepository.GetUsersAsync().ContinueWith(t => t.Result.Any(e => e.Id == id)))
                {
                    return NotFound($"{id} idsine sahip kullanıcı bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Güncelleme sırasında bir hata oluştu: {ex.Message}");
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Aradığınız kullanıcı bulunamadı.");
            }
            await _userRepository.Delete(id);
            await _userRepository.Update(user);
            return NoContent();

        }
    }
}
