using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Chat chat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Chat? result = await _chatService.CreateChat(chat);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDogChates(string dogId)
        {
            List<Chat> result = await _chatService.GetAllDogChats(dogId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("{chatId}")]
        [Authorize]
        public async Task<IActionResult> GetChatById(int chatId)
        {
            Chat? result = await _chatService.GetChatById(chatId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        // ??? LAMA ???
        [HttpPut("{chatId}")]
        [Authorize]
        public async Task<IActionResult> Put(int chatId, [FromBody] Chat newChat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Chat? result = await _chatService.UpdateChat(chatId, newChat);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        // ??? LAMA ???
        [HttpDelete("{chatId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int chatId)
        {
            Chat? result = await _chatService.DeleteChat(chatId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("message")]
        public async Task<IActionResult> AddMessageToChat([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Message? result = await _chatService.AddMessageToChat(message);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpGet("message/{chatId}")]
        public async Task<IActionResult> GetMessagesByChatId(int chatId)
        {
            List<Message> result = await _chatService.GetMessagesByChatId(chatId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPost("message/{messageId}")]
        // לשנות את הפונקציה לעבוד כפאטץ' מכיוון ועובד מהר יותר
        public async Task<IActionResult> MarkMessageAsRead(int messageId)
        {
            Message? result = await _chatService.MarkMessageAsRead(messageId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
