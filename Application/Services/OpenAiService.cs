using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Services;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.SubModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class OpenAiService : IOpenAiService
{
    private readonly ILogger<OpenAiService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _breedSizePrompt = "You will receive an image that may contain one or more dogs. If multiple dogs are present, randomly choose one. Match the dog's breed to the list below. If no match is found or no dogs are present, respond with:\r\n{\r\n  \"breed\": -1,\r\n  \"size\": -1\r\n}\r\nWhere:\r\n\r\nbreed is the enum index of DogType (starting from 0).\r\n\r\nsize is the index of size:\r\n\r\n0 = Small\r\n\r\n1 = Medium\r\n\r\n2 = Large\r\n\r\nProvide your answer strictly in the following JSON format:\r\n\r\n{\r\n  \"breed\": {breed_enum_index},\r\n  \"size\": {size_enum_index}\r\n}\r\nDo not include any explanations or extra text. Only return the answer in the specified format.\r\n\r\nHere is the list of dog breeds (enum DogType), starting from index 0:\r\n\r\n0: Afghan_Hound\r\n1: Akita\r\n2: Alaskan_Malamute\r\n3: American_Bulldog\r\n4: American_Eskimo_Dog\r\n5: American_Foxhound\r\n6: American_Staffordshire_Terrier\r\n7: Anatolian_Shepherd\r\n8: Australian_Cattle_Dog\r\n9: Australian_Shepherd\r\n10: Basenji\r\n11: Basset_Hound\r\n12: Beagle\r\n13: Beauceron\r\n14: Belgian_Malinois\r\n15: Belgian_Sheepdog\r\n16: Belgian_Tervuren\r\n17: Bernese_Mountain_Dog\r\n18: Bichon_Frise\r\n19: Black_and_Tan_Coonhound\r\n20: Bloodhound\r\n21: Border_Collie\r\n22: Border_Terrier\r\n23: Borzoi\r\n24: Boston_Terrier\r\n25: Bouvier_des_Flandres\r\n26: Boxer\r\n27: Briard\r\n28: Brittany_Spaniel\r\n29: Bulldog\r\n30: Bullmastiff\r\n31: Cairn_Terrier\r\n32: Cane_Corso\r\n33: Cavalier_King_Charles_Spaniel\r\n34: Chesapeake_Bay_Retriever\r\n35: Chihuahua\r\n36: Chinese_Crested\r\n37: Chinese_Shar_Pei\r\n38: Chow_Chow\r\n39: Clumber_Spaniel\r\n40: Cocker_Spaniel\r\n41: Collie\r\n42: Curly_Coated_Retriever\r\n43: Dachshund\r\n44: Dalmatian\r\n45: Dandie_Dinmont_Terrier\r\n46: Doberman_Pinscher\r\n47: English_Cocker_Spaniel\r\n48: English_Foxhound\r\n49: English_Setter\r\n50: English_Springer_Spaniel\r\n51: English_Toy_Spaniel\r\n52: Flat_Coated_Retriever\r\n53: French_Bulldog\r\n54: German_Pinscher\r\n55: German_Shepherd\r\n56: German_Shorthaired_Pointer\r\n57: German_Wirehaired_Pointer\r\n58: Giant_Schnauzer\r\n59: Golden_Retriever\r\n60: Gordon_Setter\r\n61: Great_Dane\r\n62: Great_Pyrenees\r\n63: Greyhound\r\n64: Havanese\r\n65: Irish_Setter\r\n66: Irish_Terrier\r\n67: Irish_Water_Spaniel\r\n68: Irish_Wolfhound\r\n69: Italian_Greyhound\r\n70: Jack_Russell_Terrier\r\n71: Japanese_Chin\r\n72: Keeshond\r\n73: Kerry_Blue_Terrier\r\n74: Komondor\r\n75: Kuvasz\r\n76: Labrador_Retriever\r\n77: Lakeland_Terrier\r\n78: Leonberger\r\n79: Lhasa_Apso\r\n80: Maltese\r\n81: Manchester_Terrier\r\n82: Mastiff\r\n83: Miniature_Bull_Terrier\r\n84: Miniature_Pinscher\r\n85: Miniature_Schnauzer\r\n86: Mixed\r\n87: Neapolitan_Mastiff\r\n88: Newfoundland\r\n89: Norfolk_Terrier\r\n90: Norwegian_Elkhound\r\n91: Norwich_Terrier\r\n92: Old_English_Sheepdog\r\n93: Otterhound\r\n94: Papillon\r\n95: Pekingese\r\n96: Pembroke_Welsh_Corgi\r\n97: Petit_Basset_Griffon_Vendeen\r\n98: Pharaoh_Hound\r\n99: Pointer\r\n100: Polish_Lowland_Sheepdog\r\n101: Pomeranian\r\n102: Poodle\r\n103: Portuguese_Water_Dog\r\n104: Pug\r\n105: Puli\r\n106: Rhodesian_Ridgeback\r\n107: Rottweiler\r\n108: Saint_Bernard\r\n109: Saluki\r\n110: Samoyed\r\n111: Schipperke\r\n112: Scottish_Terrier\r\n113: Sealyham_Terrier\r\n114: Shetland_Sheepdog\r\n115: Shiba_Inu\r\n116: Shih_Tzu\r\n117: Siberian_Husky\r\n118: Silky_Terrier\r\n119: Skye_Terrier\r\n120: Soft_Coated_Wheaten_Terrier\r\n121: Staffordshire_Bull_Terrier\r\n122: Standard_Schnauzer\r\n123: Sussex_Spaniel\r\n124: Tibetan_Mastiff\r\n125: Tibetan_Spaniel\r\n126: Tibetan_Terrier\r\n127: Toy_Fox_Terrier\r\n128: Toy_Manchester_Terrier\r\n129: Toy_Poodle\r\n130: Vizsla\r\n131: Weimaraner\r\n132: Welsh_Springer_Spaniel\r\n133: Welsh_Terrier\r\n134: West_Highland_White_Terrier\r\n135: Whippet\r\n136: Wirehaired_Fox_Terrier\r\n137: Wirehaired_Pointing_Griffon\r\n138: Xoloitzcuintli\r\n139: Yorkshire_Terrier\r\n140: Other\r\n\r\nSize mapping:\r\n\r\n0: Small\r\n1: Medium\r\n2: Large\r\n";
    private readonly string? _openAIApiKey;
    public OpenAiService(ILogger<OpenAiService> logger, IConfiguration config)
    {
        _openAIApiKey = config["OpenAI:ApiKey"];
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAIApiKey);
        _logger = logger;
    }
    public async Task<DogBreedSize> GetDogBreedFromImageUrlAsync(string publicImageUrl)
    {
        try
        {
            var requestBody = new
            {
                model = "gpt-4-turbo",
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = _breedSizePrompt },
                            new
                            {
                                type = "image_url",
                                image_url = new
                                {
                                    url = publicImageUrl
                                }
                            }
                        }
                    }
                },
                max_tokens = 100
            };

            string jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            _logger.LogInformation("Bot response status for get breed size-  {response.StatusCode} .", response.StatusCode);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            dynamic? result = JsonConvert.DeserializeObject(responseBody);
            string reply = result.choices[0].message.content;

            DogBreedSize? breedSize = JsonConvert.DeserializeObject<DogBreedSize>(reply);
            
            if (breedSize != null)
            {
                return breedSize;
            }
            return new DogBreedSize
            {
                Breed = -1,
                Size = -1
            };
        }
        catch (Exception ex)
        {
            return new DogBreedSize
            {
                Breed = -1,
                Size = -1
            };
        }
    }

    private string BuildConversationPrompt(User user, Dog userDog, User otherOwner, Dog otherDog, Chat chat)
    {
        string FormatUser(User u) =>
            $"{u.FirstName} {u.LastName}, {(u.Gender?.ToString() ?? "Unknown Gender")}, born on {u.BirthDate?.ToString("yyyy-MM-dd") ?? "Unknown Birth Date"}";

        string FormatDog(Dog d) =>
            $"{d.Name}, a {d.Size?.ToString().ToLower() ?? "unknown size"} " +
            $"{(d.Gender.HasValue ? (d.Gender.Value ? "male" : "female") : "gender unknown")} " +
            $"{d.Type?.ToString().Replace("_", " ") ?? "dog"} " +
            $"born on {d.BirthDate?.ToString("yyyy-MM-dd") ?? "unknown date"}, " +
            $"{(d.Vaccinated == true ? "vaccinated" : "not vaccinated")}, lives at {d.Address ?? "unknown location"}";

        string lastMessage = chat?.Messages?.LastOrDefault()?.Content ?? "No recent message.";


        return $@"
            You are {FormatUser(user)}, the owner of a dog named {FormatDog(userDog)}.
            You are currently chatting with {FormatUser(otherOwner)}, who owns {FormatDog(otherDog)}.

            Here's a summary of your conversation so far:
            {chat.Messages}

            The other owner just sent a new message:
            ""{lastMessage}""

            Please reply with a friendly and natural message that sounds like a real dog owner responding to that last message.

            ✳️ Keep your reply:
            - Focused on the most recent message
            - Friendly and short (1–2 sentences)
            - Personalized, casual, and fun
            - Related to your dog when relevant

            ✅ Only return your new reply as a plain string. Do not add any formatting, explanation, or roles.";

    }
    public async Task<string> GetDogChatBotReplyAsync(User user, Dog userDog, User otherOwner, Dog otherDog, Chat chat)
    {
        try
        {
            string prompt = BuildConversationPrompt(user, userDog, otherOwner, otherDog, chat);

            var requestBody = new
            {
                model = "gpt-4-turbo",
                messages = new object[]
                {
            new { role = "user", content = prompt }
                },
                max_tokens = 100
            };

            string jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);
            string? reply = json["choices"]?[0]?["message"]?["content"]?.ToString();

            return reply?.Trim() ?? string.Empty;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while sending openAI image for dog analys"); 
            return string.Empty;
        }
    }
}