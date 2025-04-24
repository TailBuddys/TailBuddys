using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Models;

public static class ModelBuilderExtensions
{
    private static (double lat, double lon) GetRandomCoordinateInPolygon(List<(double lat, double lon)> polygon)
    {
        var rand = new Random();
        double minLat = polygon.Min(p => p.lat);
        double maxLat = polygon.Max(p => p.lat);
        double minLon = polygon.Min(p => p.lon);
        double maxLon = polygon.Max(p => p.lon);

        while (true)
        {
            double lat = rand.NextDouble() * (maxLat - minLat) + minLat;
            double lon = rand.NextDouble() * (maxLon - minLon) + minLon;
            var point = (lat, lon);
            if (IsPointInPolygon(point, polygon))
                return point;
        }
    }

    private static bool IsPointInPolygon((double lat, double lon) point, List<(double lat, double lon)> polygon)
    {
        int n = polygon.Count;
        bool inside = false;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            double xi = polygon[i].lon, yi = polygon[i].lat;
            double xj = polygon[j].lon, yj = polygon[j].lat;
            double px = point.lon, py = point.lat;

            bool intersect = ((yi > py) != (yj > py)) &&
                             (px < (xj - xi) * (py - yi) / ((yj - yi) + double.Epsilon) + xi);
            if (intersect)
                inside = !inside;
        }
        return inside;
    }

    private static readonly string[] streets = {
        "Herzl", "Ben Gurion", "Rothschild", "King David", "Menachem Begin", "Yitzhak Rabin", "Dizengoff",
        "Jabotinsky", "HaPalmach", "HaShalom", "Hagana", "Allenby", "Weizmann", "Aharonovitch", "Arlozorov"
    };

    private static readonly string[] cities = {
        "Tel Aviv", "Jerusalem", "Haifa", "Beer Sheva", "Netanya", "Ashdod", "Eilat", "Ramat Gan", "Holon", "Nazareth",
        "Herzliya", "Bat Yam", "Petah Tikva", "Rehovot", "Hadera", "Tiberias", "Modiin", "Ashkelon", "Kiryat Gat", "Beit Shemesh"
    };
    private static readonly List<(double lat, double lon)> IsraelPolygon = new()
        {
            (33.1074, 35.5139),  // North-East (Metula)
            (33.1000, 35.2500),
            (32.8000, 35.1000),
            (32.5000, 35.2000),
            (31.8000, 35.4000),
            (31.0000, 35.5000),
            (30.6000, 35.4000),
            (30.3000, 35.2500),
            (29.9000, 35.1000),
            (29.5500, 34.9500),  // South-East edge
            (29.5500, 34.9300),  // Southern tip near Eilat
            (29.6000, 34.8000),
            (30.3000, 34.7000),
            (30.8000, 34.7000),
            (31.1000, 34.8000),
            (32.3000, 34.9000),
            (32.8000, 34.9000),
            (33.0500, 34.9500),
            (33.1000, 35.0500),
            (33.1074, 35.1000)   // North-West back to top
        };
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



        string[] dogNames = { "Bella", "Luna", "Max", "Charlie", "Lucy", "Rocky", "Daisy", "Toby", "Milo", "Sadie",
            "Buddy", "Coco", "Bailey", "Zoe", "Bear", "Stella", "Jack", "Penny", "Duke", "Lola",
            "Finn", "Chloe", "Leo", "Ellie", "Riley", "Ruby", "Oscar", "Maggie", "Sam", "Nala",
            "Zeus", "Gracie", "Harley", "Rosie", "Marley", "Jasper", "Sasha", "Winnie", "Ginger", "Scout",
            "Boomer", "Abby", "Olive", "Simba", "Moose", "Hazel", "Benji", "Remy", "Pepper", "Rex",
            "Bruno", "Shadow", "Honey", "Nova", "Ace", "Lucky", "Sky", "Ziggy", "Otis", "Maple" };

      
        //"Park Carmiel", "Gan Binyamin", "Park Rehovot", "Raanana Park", "Ramat Hasharon Grove", "Hadera Forest",
        //"Gan Hapsalim", "Park Modi'in", "Nahal Alexander", "Yavne Park", "Holon Eco Park", "Ashkelon National Park",
        //"Tel Afek", "Palmachim Beach Park", "Ein Hemed", "Park Eshkol", "Mount Carmel Park", "Jaffa Park",
        //"Netanya Iris Reserve", "Ga'ash Cliff Trail", "Ashdod Dunes", "Hula Valley Park", "Zikhron Ya'akov Promenade",
        //"Kiryat Ata Park", "Sderot Forest", "Jerusalem Gazelle Valley" };

        var availableImages = new HashSet<string>
        {
            "park1o1.jpg", "park1o2.jpg", "park1o3.jpg", "park1o4.jpg", "park1o5.jpg",
            "park2o1.jpg", "park2o2.jpg", "park2o3.jpg", "park2o4.jpg", "park2o5.jpg",
            "park3o1.jpg", "park3o2.jpg", "park3o3.jpg", "park3o4.jpg",
            "park4o1.jpg", "park4o2.jpg", "park4o3.jpg", "park4o4.jpg",
            "park5o1.jpg", "park5o2.jpg", "park5o3.jpg",
            "park6o1.jpg", "park6o2.jpg", "park6o3.jpg", "park6o4.jpg", "park6o5.jpg",
            "park7o1.jpg", "park7o2.jpg", "park7o3.jpg",
            "park8o1.jpg", "park8o2.jpg", "park8o3.jpg",
            "park9o1.jpg", "park9o2.jpg", "park9o3.jpg", "park9o4.jpg", "park9o5.jpg",
            "park10o1.jpg", "park10o2.jpg", "park10o3.jpg", "park10o4.jpg",
            "park11o1.jpg", "park11o2.jpg", "park11o3.jpg", "park11o4.jpg", "park11o5.jpg",
            "park12o1.jpg", "park12o2.jpg", "park12o3.jpg", "park12o4.jpg",
            "park13o1.jpg", "park13o2.jpg", "park13o3.jpg", "park13o4.jpg",
            "park14o1.jpg", "park14o2.jpg", "park14o3.jpg",
            "park15o1.jpg", "park15o2.jpg", "park15o3.jpg", "park15o4.jpg",
            "park16o1.jpg", "park16o2.jpg", "park16o3.jpg",
            "park17o1.jpg", "park17o2.jpg", "park17o3.jpg",
            "park18o1.jpg", "park18o2.jpg", "park18o3.jpg",
            "park19o1.jpg", "park19o2.jpg", "park19o3.jpg",
            "park20o1.jpg", "park20o2.jpg", "park20o3.jpg", "park20o4.jpg"
        };

        string[] dogAddresses = Enumerable.Range(0, 100)
             .Select(_ => $"{random.Next(1, 200)} {streets[random.Next(streets.Length)]} St, {cities[random.Next(cities.Length)]}")
             .ToArray();

        string[] parkAddresses = Enumerable.Range(0, 100)
            .Select(_ => $"{random.Next(1, 200)} {streets[random.Next(streets.Length)]} St, {cities[random.Next(cities.Length)]}")
            .ToArray();
        

        var fixedParkDescriptions = new[]
        {
            "Spacious riverside park with biking trails, dog agility zones, and open fields for frisbee fun.",
            "Lush garden escape featuring fountains, shaded benches, and dog socializing corners.",
            "Stylish park in historic Neve Tzedek with cozy paths and plenty of sniffs to discover.",
            "Sandy beach park with calm waters, perfect for swimming dogs and sunset walks.",
            "Modern urban greenery with well-lit paths, grassy slopes, and weekend dog events.",
            "Iconic Holon spot surrounded by art sculptures, dog-safe ponds, and picnic areas.",
            "Hilly retreat with nature vibes, off-leash zones, and bird-watching for curious canines.",
            "Flowing river paths, chirping birds, and wide grassy knolls to roll around in.",
            "Clean and structured with benches every few meters, poop bag stations, and training zones.",
            "Wide fields, native trees, and relaxing woodland trails for mindful mutt strolls.",
            "Neighborhood gem with small-dog sections, toddler-proof gates, and shaded pavilions.",
            "Central green space with water sprinklers, doggy obstacle ramps, and snack vendors.",
            "Natural reserve vibe with desert trees, natural springs, and paw-friendly terrain.",
            "Desert-style park with resilient plants, sand dunes, and dog-friendly shaded huts.",
            "Peaceful terraces with views, natural stone trails, and olive trees everywhere.",
            "Lakeside breeze, open-air paths, and splash-ready ramps for water-loving pups.",
            "Botanical-style gardens, beach-like gravel areas, and easy hilltop lookout trails.",
            "Hidden forest grove for off-leash exploration and community gathering events.",
            "Ocean views, clean benches, and occasional dog fairs along the marina boulevard.",
            "Olive trees, quiet picnic spots, and ancient vibes mixed with modern comfort."
        };
        var fixedParks = new[]
        {
            new { Name = "Gan HaYarkon", Address = "Yarkon St 35, Tel Aviv", Lat = 32.0975, Lon = 34.8147 },
            new { Name = "Park HaShalom", Address = "Peace Ave 12, Ramat Gan", Lat = 32.0681, Lon = 34.8231 },
            new { Name = "Neve Tzedek Park", Address = "Shabazi St 44, Tel Aviv", Lat = 32.0601, Lon = 34.7637 },
            new { Name = "Palmachim Beach Park", Address = "Palmachim Rd 5, Rishon LeZion", Lat = 31.9304, Lon = 34.7094 },
            new { Name = "Ramat Aviv Green", Address = "Levi Eshkol Blvd 102, Tel Aviv", Lat = 32.1113, Lon = 34.7985 },
            new { Name = "Gan Bialik", Address = "Bialik St 9, Holon", Lat = 32.0152, Lon = 34.7723 },
            new { Name = "Herzliya Hills Park", Address = "HaHagana St 77, Herzliya", Lat = 32.1613, Lon = 34.8361 },
            new { Name = "Yarkon River View", Address = "Riverside Rd 23, Bnei Brak", Lat = 32.1048, Lon = 34.8285 },
            new { Name = "Gan Modiin", Address = "Weizmann St 90, Modiin", Lat = 31.8948, Lon = 35.0071 },
            new { Name = "Sderot Woodland", Address = "Sderot HaAtzmaut 18, Sderot", Lat = 31.5215, Lon = 34.5926 },
            new { Name = "Givat Shmuel Park", Address = "Hazayit St 5, Givat Shmuel", Lat = 32.0723, Lon = 34.8463 },
            new { Name = "Park Netanya Center", Address = "Herzl St 112, Netanya", Lat = 32.3294, Lon = 34.8576 },
            new { Name = "Ein Gedi Natural Spot", Address = "Route 90, Ein Gedi", Lat = 31.4597, Lon = 35.3925 },
            new { Name = "Gan Re'im", Address = "Re'im Blvd 2, Be'er Sheva", Lat = 31.2513, Lon = 34.7913 },
            new { Name = "Ma'ale Adumim Retreat", Address = "Ma'ale St 11, Ma'ale Adumim", Lat = 31.7732, Lon = 35.2968 },
            new { Name = "Tiberias Lakeside Park", Address = "Lakeside Rd 8, Tiberias", Lat = 32.7893, Lon = 35.5315 },
            new { Name = "Haifa Bay Gardens", Address = "Bayview St 21, Haifa", Lat = 32.8272, Lon = 35.0086 },
            new { Name = "Ganei Tikva Nature Park", Address = "Nature Trail 4, Ganei Tikva", Lat = 32.0598, Lon = 34.8809 },
            new { Name = "Ashdod Marina Park", Address = "Marina Rd 1, Ashdod", Lat = 31.8002, Lon = 34.6418 },
            new { Name = "Jerusalem Olive Grove", Address = "Olive Grove St 33, Jerusalem", Lat = 31.7719, Lon = 35.2135 }
        };


        var users = Enumerable.Range(1, 20).Select(i => new User
        {
            Id = i,
            FirstName = $"User{i}",
            LastName = $"Last{i}",
            Email = $"user{i}@tail.com",
            Gender = (Gender)random.Next(0, 3),
            BirthDate = DateTime.UtcNow.AddYears(-random.Next(18, 50)).AddDays(random.Next(365)),
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();
        modelBuilder.Entity<User>().HasData(users);

        var dogs = new List<Dog>();
        int dogId = 1;
        foreach (var user in users)
        {
            int count = random.Next(1, 2);
            for (int i = 0; i < count; i++)
            {
                var (lat, lon) = GetRandomCoordinateInPolygon(IsraelPolygon);

                dogs.Add(new Dog
                {
                    Id = dogId,
                    UserId = user.Id,
                    Name = dogNames[random.Next(dogNames.Length)],
                    Description = $"Friendly dog that loves {dogNames[random.Next(dogNames.Length)]}.",
                    Address = dogAddresses[random.Next(dogAddresses.Length)],
                    Type = (DogType)random.Next(0, 10),
                    Size = (DogSize)random.Next(0, 3),
                    Gender = random.Next(0, 2) == 0,
                    Vaccinated = random.NextDouble() < 0.85,
                    Lat = lat,
                    Lon = lon,
                    BirthDate = now.AddYears(-random.Next(0, 10)).AddMonths(-random.Next(2, 12)).AddDays(-random.Next(0, 30)),
                    IsBot = random.NextDouble() < 0.15,
                    CreatedAt = now,
                    UpdatedAt = now
                });

                dogId++;
            }
        }
        modelBuilder.Entity<Dog>().HasData(dogs);

        var parks = new List<Park>();
        var images = new List<Image>();
        int imageIdCounter = 1;

        for (int i = 0; i < 20; i++)
        {
            var parkInfo = fixedParks[i];

            var park = new Park
            {
                Id = i + 1,
                Name = parkInfo.Name,
                Description = fixedParkDescriptions[i],
                Address = parkInfo.Address,
                Lat = parkInfo.Lat,
                Lon = parkInfo.Lon,
                CreatedAt = now,
                UpdatedAt = now
            };
            parks.Add(park);

            // Add images per park
            for (int order = 1; order <= 5; order++)
            {
                string imageFileName = $"park{park.Id}o{order}.jpg";
                if (availableImages.Contains(imageFileName))
                {
                    images.Add(new Image
                    {
                        Id = imageIdCounter++,
                        ParkId = park.Id,
                        DogId = null,
                        Order = order - 1, // convert to 0-based
                        Url = $"https://storage.googleapis.com/tail_buddys_bucket1/{imageFileName}"
                    });
                }
            }
        }

        modelBuilder.Entity<Park>().HasData(parks);
        modelBuilder.Entity<Image>().HasData(images);
        // In your Seed() method, after seeding Dogs and Parks:

        int dogCount = dogs.Count;
        int parkCount = parks.Count;
        var dogParkLikes = new List<Dictionary<string, object>>();
        foreach (var dog in dogs)
        {
            var likedParks = Enumerable.Range(0, random.Next(2, 6))
                .Select(_ => parks[random.Next(parkCount)].Id)
                .Distinct();

            foreach (var parkId in likedParks)
            {
                dogParkLikes.Add(new Dictionary<string, object>
                {
                    ["FavParksId"] = parkId,
                    ["DogLikesId"] = dog.Id
                });
            }
        }

        modelBuilder.Entity("DogParks").HasData(dogParkLikes.ToArray());
    }
}