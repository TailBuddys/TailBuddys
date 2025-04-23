using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;

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
            int dogId;
            int.TryParse(HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == "DogId" && c.Value == chat.SenderDogId.ToString())?.Value, out dogId);

            if (dogId != 0)
            {
                Chat? result = await _chatService.CreateChat(chat);

                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDogChats(int dogId)
        {
            int ClientDogId;
            int.TryParse(HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == "DogId" && c.Value == dogId.ToString())?.Value, out ClientDogId);

            if (dogId != 0)
            {
                List<ChatDTO> result = await _chatService.GetAllDogChats(dogId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            return Unauthorized();

        }

        [HttpGet("{chatId}")]
        [Authorize]
        public async Task<IActionResult> GetChatById(int chatId)
        {
            FullChatDTO? result = await _chatService.GetChatById(chatId);
            if (result == null)
            {
                return NotFound();
            }

            int ClientDogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId"
            && (c.Value == result.SenderDog.Id.ToString() || c.Value == result.ReceiverDog.Id.ToString()))?.Value, out ClientDogId);

            if (ClientDogId != 0)
            {
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpPut("{chatId}")]
        [Authorize]
        public async Task<IActionResult> Put(int chatId, bool isArchive)
        {
            FullChatDTO? result = await _chatService.GetChatById(chatId);
            if (result == null)
            {
                return NotFound();
            }
            int ClientDogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId"
            && (c.Value == result.SenderDog.Id.ToString() || c.Value == result.ReceiverDog.Id.ToString()))?.Value, out ClientDogId);

            if (!ModelState.IsValid || ClientDogId == 0)
                return Unauthorized();

            ChatDTO? updatedChat = await _chatService.UpdateChat(chatId, isArchive, ClientDogId);
            if (updatedChat == null)
            {
                return NotFound();
            }
            return Ok(updatedChat);
        }

        // ??? LAMA ???
        [HttpDelete("{chatId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int chatId)
        {

            Chat? chatToDelete = await _chatService.GetChatDetailsById(chatId);
            if (chatToDelete == null)
            {
                return NotFound();
            }

            int ClientDogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId"
            && (c.Value == chatToDelete.SenderDogId.ToString() || c.Value == chatToDelete.ReceiverDogId.ToString()))?.Value, out ClientDogId);

            if (ClientDogId != 0)
            {
                Chat? result = await _chatService.DeleteChat(chatId);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();
        }

        // האם נקרא = כן במידה והכלב מחובר להאב
        [HttpPost("message")]
        [Authorize]
        public async Task<IActionResult> AddMessageToChat([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Chat? chatToUpdate = await _chatService.GetChatDetailsById(message.ChatID);
            if (chatToUpdate == null)
            {
                return NotFound();
            }
            int ClientDogId;
            int.TryParse(HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == "DogId" && c.Value == message.SenderDogId.ToString())?.Value, out ClientDogId);

            if (ClientDogId == 0 || (ClientDogId != chatToUpdate.SenderDogId && ClientDogId != chatToUpdate.ReceiverDogId)) return Unauthorized();

            Message? result = await _chatService.AddMessageToChat(chatToUpdate, message);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        // should be in the GetById function 

        //[HttpGet("message/{chatId}")]
        //[Authorize]
        //public async Task<IActionResult> GetMessagesByChatId(int chatId)
        //{
        //    List<Message> result = await _chatService.GetMessagesByChatId(chatId);
        //    if (result == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(result);
        //}


        //[HttpPatch("message/{messageId}")]
        //[Authorize]
        //// לשנות את הפונקציה לעבוד כפאטץ' מכיוון ועובד מהר יותר
        //public async Task<IActionResult> MarkMessageAsRead(int messageId)
        //{
        //    Message? result = await _chatService.MarkMessageAsRead(messageId);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result);
        //}

        // GPT review

        // לסנן את כל ההודעות שנקראו בצ'אט ולא אחת
        //[HttpPatch("message/{messageId}")]
        //[Authorize]
        //public async Task<IActionResult> MarkMessageAsRead(int messageId)
        //{
        //    Message? result = await _chatService.MarkMessageAsRead(messageId);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    FullChatDTO? chat = await _chatService.GetChatById(result.ChatID);
        //    if (chat == null)
        //    {
        //        return NotFound();
        //    }
        //    int clientDogId;
        //    int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId"
        //        && (c.Value == chat.SenderDog.Id.ToString() || c.Value == chat.ReceiverDog.Id.ToString()))?.Value, out clientDogId);
        //    if (clientDogId == 0)
        //    {
        //        return Unauthorized();
        //    }
        //    return Ok(result);
        //}
    }
}
