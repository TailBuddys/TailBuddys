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


        string[] dogNames = { "Bella", "Luna", "Max", "Charlie", "Lucy", "Rocky", "Daisy", "Toby", "Milo", "Sadie",
            "Buddy", "Coco", "Bailey", "Zoe", "Bear", "Stella", "Jack", "Penny", "Duke", "Lola",
            "Finn", "Chloe", "Leo", "Ellie", "Riley", "Ruby", "Oscar", "Maggie", "Sam", "Nala",
            "Zeus", "Gracie", "Harley", "Rosie", "Marley", "Jasper", "Sasha", "Winnie", "Ginger", "Scout",
            "Boomer", "Abby", "Olive", "Simba", "Moose", "Hazel", "Benji", "Remy", "Pepper", "Rex",
            "Bruno", "Shadow", "Honey", "Nova", "Ace", "Lucky", "Sky", "Ziggy", "Otis", "Maple" };

        string[] parkNames = { "Gan Meir", "Independence Park", "Yarkon Park", "Sacher Park", "Ramat Gan National Park", "HaPisga Garden",
            "Canada Park", "Ashdod Yam Park", "Gan Ha'ir", "Armon Hanatziv Promenade", "Liberty Bell Park", "Lachish Park",
            "Ariel Sharon Park", "Park Giv'atayim", "Herzliya Park", "Park Ashkelon", "Beit She'an Park", "Ein Gedi Reserve",
            "Neot Kedumim", "Ma'ayan Harod", "Timna Park", "Alona Park", "Park Kiryat Motzkin", "Bat Galim Promenade", };
        //"Park Carmiel", "Gan Binyamin", "Park Rehovot", "Raanana Park", "Ramat Hasharon Grove", "Hadera Forest",
        //"Gan Hapsalim", "Park Modi'in", "Nahal Alexander", "Yavne Park", "Holon Eco Park", "Ashkelon National Park",
        //"Tel Afek", "Palmachim Beach Park", "Ein Hemed", "Park Eshkol", "Mount Carmel Park", "Jaffa Park",
        //"Netanya Iris Reserve", "Ga'ash Cliff Trail", "Ashdod Dunes", "Hula Valley Park", "Zikhron Ya'akov Promenade",
        //"Kiryat Ata Park", "Sderot Forest", "Jerusalem Gazelle Valley" };

        string[] dogAddresses = Enumerable.Range(0, 100)
             .Select(_ => $"{random.Next(1, 200)} {streets[random.Next(streets.Length)]} St, {cities[random.Next(cities.Length)]}")
             .ToArray();

        string[] parkAddresses = Enumerable.Range(0, 100)
            .Select(_ => $"{random.Next(1, 200)} {streets[random.Next(streets.Length)]} St, {cities[random.Next(cities.Length)]}")
            .ToArray();

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

        var parks = Enumerable.Range(1, parkNames.Length).Select(i =>
        {
            var (lat, lon) = GetRandomCoordinateInPolygon(IsraelPolygon);
            return new Park
            {
                Id = i,
                Name = parkNames[i - 1],
                Description = $"Beautiful park for dogs. Great for playing and resting.",
                Address = parkAddresses[random.Next(parkAddresses.Length)],
                Lat = lat,
                Lon = lon,
                CreatedAt = now,
                UpdatedAt = now
            };
        }).ToList();
        modelBuilder.Entity<Park>().HasData(parks);

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