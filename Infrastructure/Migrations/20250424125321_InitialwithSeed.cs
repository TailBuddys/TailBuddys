using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TailBuddys.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialwithSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lon = table.Column<double>(type: "float", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lon = table.Column<double>(type: "float", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Vaccinated = table.Column<bool>(type: "bit", nullable: true),
                    IsBot = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DogId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    UnreadCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatNotifications_Dogs_DogId",
                        column: x => x.DogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderDogId = table.Column<int>(type: "int", nullable: false),
                    ReceiverDogId = table.Column<int>(type: "int", nullable: false),
                    SenderDogArchive = table.Column<bool>(type: "bit", nullable: false),
                    ReceiverDogArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_Dogs_ReceiverDogId",
                        column: x => x.ReceiverDogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chats_Dogs_SenderDogId",
                        column: x => x.SenderDogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DogParks",
                columns: table => new
                {
                    DogLikesId = table.Column<int>(type: "int", nullable: false),
                    FavParksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogParks", x => new { x.DogLikesId, x.FavParksId });
                    table.ForeignKey(
                        name: "FK_DogParks_Dogs_DogLikesId",
                        column: x => x.DogLikesId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DogParks_Parks_FavParksId",
                        column: x => x.FavParksId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DogId = table.Column<int>(type: "int", nullable: true),
                    ParkId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Dogs_DogId",
                        column: x => x.DogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderDogId = table.Column<int>(type: "int", nullable: false),
                    ReceiverDogId = table.Column<int>(type: "int", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    IsMatch = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Dogs_ReceiverDogId",
                        column: x => x.ReceiverDogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Dogs_SenderDogId",
                        column: x => x.SenderDogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatID = table.Column<int>(type: "int", nullable: false),
                    SenderDogId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatID",
                        column: x => x.ChatID,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DogId = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchNotification_Dogs_DogId",
                        column: x => x.DogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchNotification_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Parks",
                columns: new[] { "Id", "Address", "CreatedAt", "Description", "Lat", "Lon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Yarkon St 35, Tel Aviv", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Spacious riverside park with biking trails, dog agility zones, and open fields for frisbee fun.", 32.097499999999997, 34.814700000000002, "Gan HaYarkon", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 2, "Peace Ave 12, Ramat Gan", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Lush garden escape featuring fountains, shaded benches, and dog socializing corners.", 32.068100000000001, 34.823099999999997, "Park HaShalom", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 3, "Shabazi St 44, Tel Aviv", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Stylish park in historic Neve Tzedek with cozy paths and plenty of sniffs to discover.", 32.060099999999998, 34.7637, "Neve Tzedek Park", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 4, "Palmachim Rd 5, Rishon LeZion", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Sandy beach park with calm waters, perfect for swimming dogs and sunset walks.", 31.930399999999999, 34.709400000000002, "Palmachim Beach Park", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 5, "Levi Eshkol Blvd 102, Tel Aviv", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Modern urban greenery with well-lit paths, grassy slopes, and weekend dog events.", 32.1113, 34.798499999999997, "Ramat Aviv Green", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 6, "Bialik St 9, Holon", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Iconic Holon spot surrounded by art sculptures, dog-safe ponds, and picnic areas.", 32.0152, 34.772300000000001, "Gan Bialik", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 7, "HaHagana St 77, Herzliya", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Hilly retreat with nature vibes, off-leash zones, and bird-watching for curious canines.", 32.161299999999997, 34.836100000000002, "Herzliya Hills Park", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 8, "Riverside Rd 23, Bnei Brak", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Flowing river paths, chirping birds, and wide grassy knolls to roll around in.", 32.104799999999997, 34.828499999999998, "Yarkon River View", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 9, "Weizmann St 90, Modiin", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Clean and structured with benches every few meters, poop bag stations, and training zones.", 31.8948, 35.007100000000001, "Gan Modiin", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 10, "Sderot HaAtzmaut 18, Sderot", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Wide fields, native trees, and relaxing woodland trails for mindful mutt strolls.", 31.5215, 34.592599999999997, "Sderot Woodland", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 11, "Hazayit St 5, Givat Shmuel", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Neighborhood gem with small-dog sections, toddler-proof gates, and shaded pavilions.", 32.072299999999998, 34.846299999999999, "Givat Shmuel Park", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 12, "Herzl St 112, Netanya", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Central green space with water sprinklers, doggy obstacle ramps, and snack vendors.", 32.3294, 34.857599999999998, "Park Netanya Center", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 13, "Route 90, Ein Gedi", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Natural reserve vibe with desert trees, natural springs, and paw-friendly terrain.", 31.459700000000002, 35.392499999999998, "Ein Gedi Natural Spot", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 14, "Re'im Blvd 2, Be'er Sheva", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Desert-style park with resilient plants, sand dunes, and dog-friendly shaded huts.", 31.251300000000001, 34.7913, "Gan Re'im", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 15, "Ma'ale St 11, Ma'ale Adumim", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Peaceful terraces with views, natural stone trails, and olive trees everywhere.", 31.773199999999999, 35.296799999999998, "Ma'ale Adumim Retreat", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 16, "Lakeside Rd 8, Tiberias", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Lakeside breeze, open-air paths, and splash-ready ramps for water-loving pups.", 32.789299999999997, 35.531500000000001, "Tiberias Lakeside Park", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 17, "Bayview St 21, Haifa", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Botanical-style gardens, beach-like gravel areas, and easy hilltop lookout trails.", 32.827199999999998, 35.008600000000001, "Haifa Bay Gardens", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 18, "Nature Trail 4, Ganei Tikva", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Hidden forest grove for off-leash exploration and community gathering events.", 32.059800000000003, 34.880899999999997, "Ganei Tikva Nature Park", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 19, "Marina Rd 1, Ashdod", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Ocean views, clean benches, and occasional dog fairs along the marina boulevard.", 31.8002, 34.641800000000003, "Ashdod Marina Park", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 20, "Olive Grove St 33, Jerusalem", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Olive trees, quiet picnic spots, and ancient vibes mixed with modern comfort.", 31.771899999999999, 35.213500000000003, "Jerusalem Olive Grove", new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "Email", "FirstName", "Gender", "GoogleId", "IsAdmin", "LastName", "PasswordHash", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1991, 4, 10, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(294), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user1@tail.com", "User1", 0, null, false, "Last1", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 2, new DateTime(1978, 9, 12, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(314), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user2@tail.com", "User2", 1, null, false, "Last2", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 3, new DateTime(1986, 1, 25, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(317), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user3@tail.com", "User3", 1, null, false, "Last3", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 4, new DateTime(1996, 8, 26, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(319), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user4@tail.com", "User4", 1, null, false, "Last4", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 5, new DateTime(1985, 4, 14, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(323), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user5@tail.com", "User5", 2, null, false, "Last5", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 6, new DateTime(1990, 8, 9, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(325), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user6@tail.com", "User6", 1, null, false, "Last6", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 7, new DateTime(2006, 2, 5, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(328), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user7@tail.com", "User7", 1, null, false, "Last7", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 8, new DateTime(2005, 4, 10, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(331), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user8@tail.com", "User8", 1, null, false, "Last8", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 9, new DateTime(2003, 5, 11, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(333), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user9@tail.com", "User9", 1, null, false, "Last9", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 10, new DateTime(2000, 1, 13, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(336), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user10@tail.com", "User10", 2, null, false, "Last10", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 11, new DateTime(2001, 4, 27, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(339), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user11@tail.com", "User11", 2, null, false, "Last11", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 12, new DateTime(1993, 4, 15, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(341), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user12@tail.com", "User12", 2, null, false, "Last12", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 13, new DateTime(1979, 3, 2, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(392), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user13@tail.com", "User13", 2, null, false, "Last13", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 14, new DateTime(2004, 8, 21, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(395), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user14@tail.com", "User14", 1, null, false, "Last14", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 15, new DateTime(1989, 10, 24, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(398), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user15@tail.com", "User15", 2, null, false, "Last15", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 16, new DateTime(1985, 5, 25, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(401), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user16@tail.com", "User16", 1, null, false, "Last16", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 17, new DateTime(2005, 3, 17, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(403), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user17@tail.com", "User17", 0, null, false, "Last17", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 18, new DateTime(1988, 6, 16, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(406), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user18@tail.com", "User18", 2, null, false, "Last18", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 19, new DateTime(1981, 12, 15, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(409), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user19@tail.com", "User19", 2, null, false, "Last19", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) },
                    { 20, new DateTime(1982, 4, 2, 12, 53, 21, 466, DateTimeKind.Utc).AddTicks(412), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "user20@tail.com", "User20", 0, null, false, "Last20", null, null, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741) }
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "Description", "Gender", "IsBot", "Lat", "Lon", "Name", "Size", "Type", "UpdatedAt", "UserId", "Vaccinated" },
                values: new object[,]
                {
                    { 1, "73 King David St, Kiryat Gat", new DateTime(2023, 5, 12, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Charlie.", true, false, 31.187829569560702, 35.387501743110263, "Abby", 0, 6, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 1, true },
                    { 2, "12 King David St, Nazareth", new DateTime(2020, 1, 10, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Benji.", false, false, 30.446753376307694, 34.8573968289571, "Stella", 1, 8, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 2, true },
                    { 3, "145 HaShalom St, Bat Yam", new DateTime(2016, 6, 11, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Maggie.", false, true, 31.038647219008098, 35.457686766419101, "Coco", 1, 1, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 3, false },
                    { 4, "191 Yitzhak Rabin St, Jerusalem", new DateTime(2019, 2, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Lucky.", false, false, 30.710175088276177, 34.963310965804233, "Oscar", 2, 2, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 4, true },
                    { 5, "112 King David St, Ashkelon", new DateTime(2023, 11, 11, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Milo.", false, false, 32.862447788085731, 35.044485769396815, "Abby", 1, 3, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 5, true },
                    { 6, "75 Allenby St, Ashkelon", new DateTime(2016, 5, 6, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Ellie.", false, true, 31.558656693084444, 35.344004197286964, "Sky", 2, 0, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 6, true },
                    { 7, "181 Weizmann St, Kiryat Gat", new DateTime(2021, 12, 12, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Maggie.", true, false, 32.088553825282467, 35.249911042447572, "Marley", 2, 1, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 7, true },
                    { 8, "129 HaShalom St, Eilat", new DateTime(2019, 2, 15, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Lola.", true, true, 32.374740099228816, 35.08179369509886, "Oscar", 2, 8, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 8, true },
                    { 9, "109 Herzl St, Kiryat Gat", new DateTime(2019, 10, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Charlie.", false, false, 30.843504972037849, 35.305863122190139, "Bear", 1, 4, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 9, true },
                    { 10, "4 Yitzhak Rabin St, Rehovot", new DateTime(2020, 6, 21, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Luna.", true, false, 31.716876784444167, 35.392368289920057, "Luna", 2, 5, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 10, true },
                    { 11, "147 Yitzhak Rabin St, Modiin", new DateTime(2017, 7, 10, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Lola.", true, false, 31.747160291713396, 35.351967900719636, "Zoe", 0, 5, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 11, true },
                    { 12, "79 Dizengoff St, Netanya", new DateTime(2017, 5, 1, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Bruno.", false, false, 31.096803111589693, 35.288679841912973, "Maple", 2, 8, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 12, true },
                    { 13, "12 Aharonovitch St, Rehovot", new DateTime(2021, 7, 3, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Ellie.", false, false, 31.689561962087009, 35.090371324662605, "Milo", 0, 3, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 13, true },
                    { 14, "155 Arlozorov St, Beit Shemesh", new DateTime(2018, 8, 23, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Penny.", true, false, 30.801904341232756, 34.842824764999492, "Simba", 0, 8, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 14, true },
                    { 15, "12 Aharonovitch St, Rehovot", new DateTime(2015, 5, 22, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Stella.", true, false, 32.545077065832871, 34.929301665864422, "Pepper", 0, 8, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 15, true },
                    { 16, "105 Dizengoff St, Netanya", new DateTime(2025, 2, 11, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Maple.", false, false, 30.558892554724281, 35.287574301691976, "Buddy", 1, 2, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 16, true },
                    { 17, "104 Weizmann St, Eilat", new DateTime(2022, 4, 25, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Abby.", true, false, 29.925866224648423, 34.97063079512354, "Sky", 2, 9, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 17, true },
                    { 18, "25 HaShalom St, Haifa", new DateTime(2021, 10, 22, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Benji.", true, false, 30.091128140884681, 35.105518850969581, "Honey", 0, 5, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 18, false },
                    { 19, "66 HaShalom St, Beit Shemesh", new DateTime(2017, 9, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Nala.", true, false, 30.044093473749676, 34.832191755322981, "Remy", 1, 1, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 19, false },
                    { 20, "128 Arlozorov St, Jerusalem", new DateTime(2020, 5, 7, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), "Friendly dog that loves Sasha.", true, false, 32.079767897553985, 35.054777782818398, "Bruno", 0, 0, new DateTime(2025, 4, 24, 12, 53, 21, 465, DateTimeKind.Utc).AddTicks(9741), 20, true }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "DogId", "Order", "ParkId", "Url" },
                values: new object[,]
                {
                    { 1, null, 0, 1, "https://storage.googleapis.com/tail_buddys_bucket1/park1o1.jpg" },
                    { 2, null, 1, 1, "https://storage.googleapis.com/tail_buddys_bucket1/park1o2.jpg" },
                    { 3, null, 2, 1, "https://storage.googleapis.com/tail_buddys_bucket1/park1o3.jpg" },
                    { 4, null, 3, 1, "https://storage.googleapis.com/tail_buddys_bucket1/park1o4.jpg" },
                    { 5, null, 4, 1, "https://storage.googleapis.com/tail_buddys_bucket1/park1o5.jpg" },
                    { 6, null, 0, 2, "https://storage.googleapis.com/tail_buddys_bucket1/park2o1.jpg" },
                    { 7, null, 1, 2, "https://storage.googleapis.com/tail_buddys_bucket1/park2o2.jpg" },
                    { 8, null, 2, 2, "https://storage.googleapis.com/tail_buddys_bucket1/park2o3.jpg" },
                    { 9, null, 3, 2, "https://storage.googleapis.com/tail_buddys_bucket1/park2o4.jpg" },
                    { 10, null, 4, 2, "https://storage.googleapis.com/tail_buddys_bucket1/park2o5.jpg" },
                    { 11, null, 0, 3, "https://storage.googleapis.com/tail_buddys_bucket1/park3o1.jpg" },
                    { 12, null, 1, 3, "https://storage.googleapis.com/tail_buddys_bucket1/park3o2.jpg" },
                    { 13, null, 2, 3, "https://storage.googleapis.com/tail_buddys_bucket1/park3o3.jpg" },
                    { 14, null, 3, 3, "https://storage.googleapis.com/tail_buddys_bucket1/park3o4.jpg" },
                    { 15, null, 0, 4, "https://storage.googleapis.com/tail_buddys_bucket1/park4o1.jpg" },
                    { 16, null, 1, 4, "https://storage.googleapis.com/tail_buddys_bucket1/park4o2.jpg" },
                    { 17, null, 2, 4, "https://storage.googleapis.com/tail_buddys_bucket1/park4o3.jpg" },
                    { 18, null, 3, 4, "https://storage.googleapis.com/tail_buddys_bucket1/park4o4.jpg" },
                    { 19, null, 0, 5, "https://storage.googleapis.com/tail_buddys_bucket1/park5o1.jpg" },
                    { 20, null, 1, 5, "https://storage.googleapis.com/tail_buddys_bucket1/park5o2.jpg" },
                    { 21, null, 2, 5, "https://storage.googleapis.com/tail_buddys_bucket1/park5o3.jpg" },
                    { 22, null, 0, 6, "https://storage.googleapis.com/tail_buddys_bucket1/park6o1.jpg" },
                    { 23, null, 1, 6, "https://storage.googleapis.com/tail_buddys_bucket1/park6o2.jpg" },
                    { 24, null, 2, 6, "https://storage.googleapis.com/tail_buddys_bucket1/park6o3.jpg" },
                    { 25, null, 3, 6, "https://storage.googleapis.com/tail_buddys_bucket1/park6o4.jpg" },
                    { 26, null, 4, 6, "https://storage.googleapis.com/tail_buddys_bucket1/park6o5.jpg" },
                    { 27, null, 0, 7, "https://storage.googleapis.com/tail_buddys_bucket1/park7o1.jpg" },
                    { 28, null, 1, 7, "https://storage.googleapis.com/tail_buddys_bucket1/park7o2.jpg" },
                    { 29, null, 2, 7, "https://storage.googleapis.com/tail_buddys_bucket1/park7o3.jpg" },
                    { 30, null, 0, 8, "https://storage.googleapis.com/tail_buddys_bucket1/park8o1.jpg" },
                    { 31, null, 1, 8, "https://storage.googleapis.com/tail_buddys_bucket1/park8o2.jpg" },
                    { 32, null, 2, 8, "https://storage.googleapis.com/tail_buddys_bucket1/park8o3.jpg" },
                    { 33, null, 0, 9, "https://storage.googleapis.com/tail_buddys_bucket1/park9o1.jpg" },
                    { 34, null, 1, 9, "https://storage.googleapis.com/tail_buddys_bucket1/park9o2.jpg" },
                    { 35, null, 2, 9, "https://storage.googleapis.com/tail_buddys_bucket1/park9o3.jpg" },
                    { 36, null, 3, 9, "https://storage.googleapis.com/tail_buddys_bucket1/park9o4.jpg" },
                    { 37, null, 4, 9, "https://storage.googleapis.com/tail_buddys_bucket1/park9o5.jpg" },
                    { 38, null, 0, 10, "https://storage.googleapis.com/tail_buddys_bucket1/park10o1.jpg" },
                    { 39, null, 1, 10, "https://storage.googleapis.com/tail_buddys_bucket1/park10o2.jpg" },
                    { 40, null, 2, 10, "https://storage.googleapis.com/tail_buddys_bucket1/park10o3.jpg" },
                    { 41, null, 3, 10, "https://storage.googleapis.com/tail_buddys_bucket1/park10o4.jpg" },
                    { 42, null, 0, 11, "https://storage.googleapis.com/tail_buddys_bucket1/park11o1.jpg" },
                    { 43, null, 1, 11, "https://storage.googleapis.com/tail_buddys_bucket1/park11o2.jpg" },
                    { 44, null, 2, 11, "https://storage.googleapis.com/tail_buddys_bucket1/park11o3.jpg" },
                    { 45, null, 3, 11, "https://storage.googleapis.com/tail_buddys_bucket1/park11o4.jpg" },
                    { 46, null, 4, 11, "https://storage.googleapis.com/tail_buddys_bucket1/park11o5.jpg" },
                    { 47, null, 0, 12, "https://storage.googleapis.com/tail_buddys_bucket1/park12o1.jpg" },
                    { 48, null, 1, 12, "https://storage.googleapis.com/tail_buddys_bucket1/park12o2.jpg" },
                    { 49, null, 2, 12, "https://storage.googleapis.com/tail_buddys_bucket1/park12o3.jpg" },
                    { 50, null, 3, 12, "https://storage.googleapis.com/tail_buddys_bucket1/park12o4.jpg" },
                    { 51, null, 0, 13, "https://storage.googleapis.com/tail_buddys_bucket1/park13o1.jpg" },
                    { 52, null, 1, 13, "https://storage.googleapis.com/tail_buddys_bucket1/park13o2.jpg" },
                    { 53, null, 2, 13, "https://storage.googleapis.com/tail_buddys_bucket1/park13o3.jpg" },
                    { 54, null, 3, 13, "https://storage.googleapis.com/tail_buddys_bucket1/park13o4.jpg" },
                    { 55, null, 0, 14, "https://storage.googleapis.com/tail_buddys_bucket1/park14o1.jpg" },
                    { 56, null, 1, 14, "https://storage.googleapis.com/tail_buddys_bucket1/park14o2.jpg" },
                    { 57, null, 2, 14, "https://storage.googleapis.com/tail_buddys_bucket1/park14o3.jpg" },
                    { 58, null, 0, 15, "https://storage.googleapis.com/tail_buddys_bucket1/park15o1.jpg" },
                    { 59, null, 1, 15, "https://storage.googleapis.com/tail_buddys_bucket1/park15o2.jpg" },
                    { 60, null, 2, 15, "https://storage.googleapis.com/tail_buddys_bucket1/park15o3.jpg" },
                    { 61, null, 3, 15, "https://storage.googleapis.com/tail_buddys_bucket1/park15o4.jpg" },
                    { 62, null, 0, 16, "https://storage.googleapis.com/tail_buddys_bucket1/park16o1.jpg" },
                    { 63, null, 1, 16, "https://storage.googleapis.com/tail_buddys_bucket1/park16o2.jpg" },
                    { 64, null, 2, 16, "https://storage.googleapis.com/tail_buddys_bucket1/park16o3.jpg" },
                    { 65, null, 0, 17, "https://storage.googleapis.com/tail_buddys_bucket1/park17o1.jpg" },
                    { 66, null, 1, 17, "https://storage.googleapis.com/tail_buddys_bucket1/park17o2.jpg" },
                    { 67, null, 2, 17, "https://storage.googleapis.com/tail_buddys_bucket1/park17o3.jpg" },
                    { 68, null, 0, 18, "https://storage.googleapis.com/tail_buddys_bucket1/park18o1.jpg" },
                    { 69, null, 1, 18, "https://storage.googleapis.com/tail_buddys_bucket1/park18o2.jpg" },
                    { 70, null, 2, 18, "https://storage.googleapis.com/tail_buddys_bucket1/park18o3.jpg" },
                    { 71, null, 0, 19, "https://storage.googleapis.com/tail_buddys_bucket1/park19o1.jpg" },
                    { 72, null, 1, 19, "https://storage.googleapis.com/tail_buddys_bucket1/park19o2.jpg" },
                    { 73, null, 2, 19, "https://storage.googleapis.com/tail_buddys_bucket1/park19o3.jpg" },
                    { 74, null, 0, 20, "https://storage.googleapis.com/tail_buddys_bucket1/park20o1.jpg" },
                    { 75, null, 1, 20, "https://storage.googleapis.com/tail_buddys_bucket1/park20o2.jpg" },
                    { 76, null, 2, 20, "https://storage.googleapis.com/tail_buddys_bucket1/park20o3.jpg" },
                    { 77, null, 3, 20, "https://storage.googleapis.com/tail_buddys_bucket1/park20o4.jpg" }
                });

            migrationBuilder.InsertData(
                table: "DogParks",
                columns: new[] { "DogLikesId", "FavParksId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 15 },
                    { 1, 17 },
                    { 1, 18 },
                    { 1, 20 },
                    { 2, 3 },
                    { 2, 8 },
                    { 2, 10 },
                    { 2, 16 },
                    { 2, 17 },
                    { 3, 4 },
                    { 3, 12 },
                    { 3, 14 },
                    { 3, 20 },
                    { 4, 4 },
                    { 4, 20 },
                    { 5, 3 },
                    { 5, 7 },
                    { 5, 19 },
                    { 6, 2 },
                    { 6, 9 },
                    { 7, 6 },
                    { 7, 12 },
                    { 7, 19 },
                    { 7, 20 },
                    { 8, 8 },
                    { 8, 19 },
                    { 8, 20 },
                    { 9, 3 },
                    { 9, 15 },
                    { 9, 18 },
                    { 9, 19 },
                    { 9, 20 },
                    { 10, 9 },
                    { 10, 10 },
                    { 10, 17 },
                    { 11, 7 },
                    { 11, 11 },
                    { 11, 18 },
                    { 11, 19 },
                    { 12, 3 },
                    { 12, 4 },
                    { 12, 10 },
                    { 12, 12 },
                    { 12, 15 },
                    { 13, 1 },
                    { 13, 6 },
                    { 13, 20 },
                    { 14, 1 },
                    { 14, 3 },
                    { 14, 10 },
                    { 14, 20 },
                    { 15, 1 },
                    { 15, 5 },
                    { 15, 14 },
                    { 16, 6 },
                    { 16, 8 },
                    { 16, 10 },
                    { 16, 15 },
                    { 17, 8 },
                    { 17, 10 },
                    { 17, 11 },
                    { 17, 20 },
                    { 18, 3 },
                    { 18, 12 },
                    { 18, 15 },
                    { 18, 16 },
                    { 18, 19 },
                    { 19, 6 },
                    { 19, 9 },
                    { 19, 16 },
                    { 19, 19 },
                    { 20, 1 },
                    { 20, 5 },
                    { 20, 19 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatNotifications_DogId",
                table: "ChatNotifications",
                column: "DogId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ReceiverDogId",
                table: "Chats",
                column: "ReceiverDogId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SenderDogId_ReceiverDogId",
                table: "Chats",
                columns: new[] { "SenderDogId", "ReceiverDogId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DogParks_FavParksId",
                table: "DogParks",
                column: "FavParksId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_UserId",
                table: "Dogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_DogId",
                table: "Images",
                column: "DogId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ParkId",
                table: "Images",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ReceiverDogId",
                table: "Matches",
                column: "ReceiverDogId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SenderDogId",
                table: "Matches",
                column: "SenderDogId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchNotification_DogId",
                table: "MatchNotification",
                column: "DogId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchNotification_MatchId",
                table: "MatchNotification",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatID",
                table: "Messages",
                column: "ChatID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatNotifications");

            migrationBuilder.DropTable(
                name: "DogParks");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "MatchNotification");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Parks");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Dogs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
