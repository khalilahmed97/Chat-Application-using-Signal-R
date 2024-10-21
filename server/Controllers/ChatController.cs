using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.DataTransferObject;
using server.Models;
using GenericFileService.Files;
using Microsoft.AspNetCore.Authorization;
using server;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private JwtService _jwtService { get; }
        private DatabaseContext _databaseContext { get; }

        public ChatController(JWTService jwt, DatabaseContext databaseContext)
        {
            _jwtService = jwt;
            _databaseContext = databaseContext
        }
        [Authorize]
        [HttpPost("Chats")]

        public async Task<IActionResult> GetChats(Guid userId, Guid toUserId, CancellationToken cancellationToken)
        {
            List<Chat> chats = await _databaseContext.Chats.Where(person => person.UserId.Equals(userId)
            && person.ToUserId.Equals(toUserId) || person.UserId.Equals(toUserId) && person.ToUserId.Equals(userId))
                .OrderBy(person => person.Date).ToListAsync(cancellationToken);

            Ok(chats);
            
        }

        [Authorize]
        [HttpGet("AddChat")]

        public async Task<IActionResult> SendMessage(ChatDto request, CancellationToken cancellationToken )
        {
            Chat chat new Chat()
            {
                UserId = request.UserId,
                ToUserId = request.ToUserId,
                Message = request.Message,
                Date = DateTime.Now
            };
            await _databaseContext.Chats.AddAsync(chat, cancellationToken);
            _databaseContext.SaveChangesAsync(cancellationToken);

            Ok(chat);


        }
    }

}
