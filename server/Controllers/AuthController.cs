using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.DataTransferObject;
namespace server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController (DatabaseContext context): ControllerBase

    {
        [HttpPost("/Login")]
        public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
        {

            bool isNameExists = await context.Users.AnyAsync(p=>p.Name == request.Name, cancellationToken);
            return NoContent();

        }
    }
}
