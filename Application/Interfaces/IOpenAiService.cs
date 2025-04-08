using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Application.Interfaces
{
    public interface IOpenAiService
    {
        public Task<DogBreedSize> GetDogBreedFromImageUrlAsync(string publicImageUrl);

    }
}
