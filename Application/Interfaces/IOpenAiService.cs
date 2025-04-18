using TailBuddys.Core.Models;
using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Application.Interfaces
{
    public interface IOpenAiService
    {
        public Task<DogBreedSize> GetDogBreedFromImageUrlAsync(string publicImageUrl);
        public Task<string> GetDogChatBotReplyAsync(User user, Dog userDog, User otherOwner, Dog otherDog, Chat chat);

    }
}
