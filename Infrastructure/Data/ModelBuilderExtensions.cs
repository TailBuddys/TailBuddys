//using Microsoft.EntityFrameworkCore;
//using TailBuddys.Core.Models;

//namespace TailBuddys.Infrastructure.Data
//{
//    public static class ModelBuilderExtensions
//    {
//        public static void Seed(this ModelBuilder modelBuilder)
//        {
//            var random = new Random();

//            // ====== USERS ======
//            var users = Enumerable.Range(1, 200).Select(i => new User
//            {
//                Id = i,
//                FirstName = $"User{i}",
//                LastName = $"Last{i}",
//                Email = $"user{i}@mail.com",
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            }).ToList();
//            modelBuilder.Entity<User>().HasData(users);

//            // ====== DOGS ======
//            int dogId = 1;
//            var dogs = new List<Dog>();
//            foreach (var user in users)
//            {
//                int numDogs = random.Next(1, 3);
//                for (int i = 0; i < numDogs; i++)
//                {
//                    dogs.Add(new Dog
//                    {
//                        Id = dogId,
//                        UserId = user.Id,
//                        Name = $"Dog{dogId}",
//                        Type = (DogType)random.Next(0, 10),
//                        Size = (DogSize)random.Next(0, 3),
//                        Gender = random.Next(0, 2) == 0,
//                        Vaccinated = true,
//                        Lat = 32.0 + random.NextDouble(),
//                        Lon = 34.0 + random.NextDouble(),
//                        CreatedAt = DateTime.UtcNow,
//                        UpdatedAt = DateTime.UtcNow
//                    });
//                    dogId++;
//                }
//            }
//            modelBuilder.Entity<Dog>().HasData(dogs);

//            // ====== PARKS ======
//            var parks = Enumerable.Range(1, 500).Select(i => new Park
//            {
//                Id = i,
//                Name = $"Park {i}",
//                Description = "Great park!",
//                Address = $"Address {i}",
//                Lat = 32.0 + random.NextDouble(),
//                Lon = 34.0 + random.NextDouble(),
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            }).ToList();
//            modelBuilder.Entity<Park>().HasData(parks);

//            // ====== DEMO MATCHES/CHATS/MESSAGES FOR 10 DOGS ======
//            var demoDogs = dogs.OrderBy(_ => random.Next()).Take(10).ToList();
//            int matchId = 1, chatId = 1, messageId = 1;
//            var matches = new List<Match>();
//            var chats = new List<Chat>();
//            var messages = new List<Message>();

//            foreach (var sender in demoDogs)
//            {
//                var receivers = dogs.Where(d => d.Id != sender.Id).OrderBy(_ => random.Next()).Take(random.Next(2, 11)).ToList();
//                foreach (var receiver in receivers)
//                {
//                    matches.Add(new Match
//                    {
//                        Id = matchId++,
//                        SenderDogId = sender.Id,
//                        ReceiverDogId = receiver.Id,
//                        IsLike = true,
//                        IsMatch = true,
//                        CreatedAt = DateTime.UtcNow,
//                        UpdatedAt = DateTime.UtcNow
//                    });

//                    var chat = new Chat
//                    {
//                        Id = chatId++,
//                        SenderDogId = sender.Id,
//                        ReceiverDogId = receiver.Id
//                    };
//                    chats.Add(chat);

//                    for (int m = 0; m < random.Next(2, 11); m++)
//                    {
//                        messages.Add(new Message
//                        {
//                            Id = messageId++,
//                            ChatID = chat.Id,
//                            SenderDogId = m % 2 == 0 ? sender.Id : receiver.Id,
//                            Content = $"Message {messageId} from {(m % 2 == 0 ? "Sender" : "Receiver")}",
//                            CreatedAt = DateTime.UtcNow,
//                            IsRead = random.Next(0, 2) == 1
//                        });
//                    }
//                }
//            }

//            modelBuilder.Entity<Match>().HasData(matches);
//            modelBuilder.Entity<Chat>().HasData(chats);
//            modelBuilder.Entity<Message>().HasData(messages);
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var random = new Random();
        var now = DateTime.UtcNow;

        string[] dogDescriptions = {
            "Energetic and playful pup who loves long walks, belly rubs, squeaky toys, and chasing balls at the park.",
            "Gentle and affectionate dog who enjoys quiet evenings, soft beds, and making new furry and human friends.",
            "Highly intelligent and curious dog who learns tricks fast and loves solving puzzle toys with treats inside.",
            "Playful companion with a big heart, enjoys fetch, sunbathing in the yard, and cuddles on the couch.",
            "Alert and loyal, always ready for adventure or a game of tug-of-war followed by a good nap.",
            "Easy-going temperament, great with kids and other dogs, loves weekend hikes and swimming in shallow creeks.",
            "Mischievous but sweet, has a habit of stealing socks, hiding them under the couch, and acting innocent.",
            "Shy at first but opens up with treats and patience, loves head scratches and walks in quiet areas.",
            "Goofy personality, loves to roll in the grass and bark at butterflies while wagging non-stop.",
            "Protective and brave, always alert to visitors, but sweet and loving once introduced properly.",
            "Loves to nap in sunny spots by the window and snuggle under blankets during stormy weather.",
            "Adventurous spirit, always ready to explore new parks, meet new friends, and chase flying leaves.",
            "Very food-motivated, knows sit, stay, shake, and will do a dance for a piece of cheese.",
            "Loyal and obedient, follows commands quickly, enjoys running beside a bike and sleeping at your feet.",
            "Charming and talkative, communicates with happy howls and expressive eyes that melt everyone's heart.",
            "A senior soul with a calm presence, enjoys slow strolls and warm hugs from familiar hands.",
            "Loves wearing sweaters in winter, prances proudly during walks, and greets every dog with a tail wag.",
            "Can’t resist mud puddles and playing in the rain, often needs a bath after every adventure.",
            "Perfect family dog, gentle with toddlers, patient with chaos, and full of love for everyone.",
            "Enjoys doggy playdates, group walks, and riding in the backseat with ears flapping in the breeze.",
            "Sassy and opinionated, growls at vacuum cleaners but happily cuddles after expressing disapproval.",
            "Eager to please, easy to train, and always carries a toy around like it’s treasure.",
            "Big personality in a tiny body, fearless around larger dogs and climbs onto every lap in reach.",
            "Strong and athletic, loves obstacle courses and shows off jumping over small fences like a champ.",
            "Clingy but adorable, follows you from room to room and insists on sharing your pillow at night.",
            "Always excited for car rides, especially when they lead to beach runs and sunset fetch sessions.",
            "Cuddles on demand, sleeps like a log, and dreams of chasing cats and eating bacon.",
            "Very expressive, tilts head in curiosity to every word and barks to join the conversation.",
            "Wild zoomies in the morning, calm naps in the afternoon, and affectionate kisses at bedtime.",
            "Has a habit of collecting toys and creating secret stash zones behind furniture.",
            "Rolls on the floor to get attention and gives high-fives when you ask nicely.",
            "Often caught staring out the window, daydreaming about squirrels and sausages.",
            "Responds best to kindness and cheese, gets nervous at loud sounds but recovers fast.",
            "Knows how to open drawers, sneak snacks, and look completely innocent afterwards.",
            "Gentle soul, has helped children and seniors feel safe and loved through therapy programs.",
            "Good with cats, birds, rabbits, and sometimes even turtles. Everyone is a friend.",
            "Snores loudly, loves back rubs, and greets you like you’ve been gone for years after 5 minutes.",
            "Lives for snow days, rolls in the snow, and eats snowflakes with pure joy.",
            "Great swimmer, dives into lakes and pools like a golden bullet chasing tennis balls.",
            "Terrified of balloons, but otherwise confident, strong, and happy-go-lucky."
        };

        string[] parkDescriptions = {
            "A large park filled with shaded trails, open meadows, and multiple fenced areas for dogs of all sizes.",
            "Popular with locals, features dog fountains, agility equipment, and benches for owners to relax and socialize.",
            "Peaceful urban retreat with grassy fields, paved walkways, and plenty of space for off-leash fun.",
            "Fenced play zones separate big and small dogs, ensuring a fun and safe experience for all visitors.",
            "Has a calm, welcoming atmosphere with play tunnels, dog ramps, and grassy areas to roll around in.",
            "Open year-round, provides grassy play zones, snow-safe trails in winter, and covered benches for owners.",
            "Hosts weekend adoption drives, charity walks, and social events that welcome all dog breeds.",
            "A hidden gem filled with colorful flowers, peaceful benches, and hidden nooks for relaxed sniffing.",
            "Built for exploration, with narrow trails, wide fields, and a loop that circles a peaceful lake.",
            "Pet-friendly all the way, with friendly signage, community boards, and waste bags always available.",
            "Not overly crowded, but always friendly faces and polite, responsible owners respecting park etiquette.",
            "Special events like dog birthday parties and costume contests add excitement to regular visits.",
            "One of the few parks with a swimming zone for water-loving dogs to splash safely.",
            "Feels like a slice of wilderness in the middle of town, perfect for dogs that love to roam.",
            "Complete with maps, fenced paths, and signs to keep dogs safe while off-leash.",
            "Excellent place to teach recall, with natural distractions and enough space to test your dog’s focus.",
            "Safe for evening visits, with lampposts, security cameras, and a warm atmosphere after sundown.",
            "Ideal for both training and playtime, with spaces to run freely or work on obedience drills.",
            "Newly renovated with eco-friendly designs, agility sets, and separate exercise fields for energetic pups.",
            "Has dog-friendly signage, snack vendors, and emergency kits for peace of mind while you play.",
            "Daily dog walkers make this their go-to, citing trust, community, and happy tail wags everywhere."
        };

        string[] dogNames = { "Buddy", "Bella", "Charlie", "Lucy", "Max", "Daisy", "Rocky", "Lola", "Toby", "Molly" };
        string[] dogAddresses = Enumerable.Range(10, 100).Select(i => $"{i} Bark Street").ToArray();
        string[] parkNames = Enumerable.Range(1, 500).Select(i => $"Park #{i}").ToArray();

        // === USERS ===
        var users = Enumerable.Range(1, 30).Select(i => new User
        {
            Id = i,
            FirstName = $"User{i}",
            LastName = $"Last{i}",
            Email = $"user{i}@mail.com",
            Gender = (Gender)random.Next(0, 3),
            BirthDate = DateTime.UtcNow.AddYears(-random.Next(18, 70)).AddDays(random.Next(365)),
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();
        modelBuilder.Entity<User>().HasData(users);

        // === DOGS ===
        var dogs = new List<Dog>();
        int dogId = 1;
        foreach (var user in users)
        {
            int count = random.Next(1, 2);
            for (int i = 0; i < count; i++)
            {
                dogs.Add(new Dog
                {
                    Id = dogId,
                    UserId = user.Id,
                    Name = dogNames[random.Next(dogNames.Length)] + dogId,
                    Description = dogDescriptions[random.Next(dogDescriptions.Length)],
                    Address = dogAddresses[random.Next(dogAddresses.Length)],
                    Type = (DogType)random.Next(0, 10),
                    Size = (DogSize)random.Next(0, 3),
                    Gender = random.Next(0, 2) == 0,
                    Vaccinated = random.NextDouble() < 0.8,
                    Lat = 32.0 + random.NextDouble(),
                    Lon = 34.7 + random.NextDouble(),
                    CreatedAt = now,
                    UpdatedAt = now
                });
                dogId++;
            }
        }
        modelBuilder.Entity<Dog>().HasData(dogs);

        // === PARKS ===
        var parks = Enumerable.Range(1, 50).Select(i => new Park
        {
            Id = i,
            Name = parkNames[i - 1],
            Description = parkDescriptions[random.Next(parkDescriptions.Length)],
            Address = $"Address {i}",
            Lat = 32.0 + random.NextDouble(),
            Lon = 34.7 + random.NextDouble(),
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();
        modelBuilder.Entity<Park>().HasData(parks);
    }
}

