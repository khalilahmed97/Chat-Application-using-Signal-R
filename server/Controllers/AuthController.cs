using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.DataTransferObject;
using server.Models;
using GenericFileService.Files;
using Microsoft.AspNetCore.Authorization;
namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public DatabaseContext Context { get; }
        public JWTService JwtService { get; }

        public AuthController(DatabaseContext context, JWTService jwtService)
        {
            Context = context;
            JwtService = jwtService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
        {

            bool isNameExists = await Context.Users.AnyAsync(p=>p.Username == request.Username, cancellationToken);

            if (isNameExists)
            {
                return Ok(new { Message = "User Already Exists!", Warning=true });
            }
            /* string avatar = FileService.FileSaveToServer(request.File, "wwwrooot/avatar")*/

            User user = new()
            {
                Name = request.Name,
                Username = request.Username,
                Password = request.Password,
              
                /* Avatar = avatar*/
            };

              await Context.Users.AddAsync(user);
              await Context.SaveChangesAsync();

              return Ok(new {Message = "Registered Successfully.", Warning=false});

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthDto data, CancellationToken cancellationTokena)
        {
            if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
            {
                return BadRequest(new { Message = "Username or Password cannot be empty.", Warning = true });
            }

            // Fetch user data based on username and password
            User userData = await Context.Users
                .SingleOrDefaultAsync(u => u.Username.Equals(data.Username) && u.Password.Equals(data.Password));

            if (userData == null)
            {
                return Ok(new { Message = "Incorrect Credentials.", Warning = true });
            }

            // Uncomment this if you want to use JWT
            var userToken = JwtService.GenerateToken(userData);

            return Ok(new { Data = userToken, Message = "Login Successfully.", Warning = false });
        }


    }
}
