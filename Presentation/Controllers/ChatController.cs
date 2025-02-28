using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Services;
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
            string? dogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == chat.SenderDogId)?.Value;
            if (dogId != null)
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
        public async Task<IActionResult> GetAllDogChates(string dogId)
        {
            string? ClientDogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == dogId)?.Value;
            if (dogId != null)
            {
                List<Chat> result = await _chatService.GetAllDogChats(dogId);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();
            
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

            string? ClientDogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" 
            && (c.Value == result.SenderDogId || c.Value == result.ReciverDogId))?.Value;
            if (ClientDogId != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }

        // ??? LAMA ???
        //[HttpPut("{chatId}")]
        //[Authorize]
        //public async Task<IActionResult> Put(int chatId, [FromBody] Chat newChat)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    Chat? result = await _chatService.UpdateChat(chatId, newChat);
        //    if (result == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(result);
        //}

        // ??? LAMA ???
        [HttpDelete("{chatId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int chatId)
        {
            
            Chat? chatToDelete = await _chatService.GetChatById(chatId);
            if (chatToDelete == null)
            {
                return BadRequest();
            }

            string? ClientDogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" 
            && (c.Value == chatToDelete.SenderDogId || c.Value == chatToDelete.ReciverDogId))?.Value;
            if (ClientDogId != null)
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

        [HttpPost("message")]
        [Authorize]
        public async Task<IActionResult> AddMessageToChat([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Chat? chatToUpdate = await _chatService.GetChatById(message.ChatID);
            if (chatToUpdate == null)
            {
                return BadRequest();
            }
            string? ClientDogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == message.SenderDogId)?.Value;
            if (ClientDogId == null || (ClientDogId != chatToUpdate.SenderDogId || ClientDogId != chatToUpdate.ReciverDogId)) return Unauthorized();   

            Message? result = await _chatService.AddMessageToChat(message);
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


        [HttpPatch("message/{messageId}")]
        [Authorize]
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
