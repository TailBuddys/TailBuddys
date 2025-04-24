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

            // Insert 30 Dogs assigned to Users 1–20 with 1–3 dogs each
            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "UserId", "Name", "Description", "Type", "Size", "Gender", "BirthDate", "Address", "Lon", "Lat", "Vaccinated", "IsBot", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, "Milo", "Energetic and playful pup who loves long walks, belly rubs, squeaky toys, and chasing balls at the park.", 9, 1, false, new DateTime(2022, 4, 8), "Haifa St, Hura, Israel", 34.9476178, 31.2976125, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 2, 1, "Kali", "Gentle and affectionate dog who enjoys quiet evenings, soft beds, and making new furry and human friends.", 86, 0, false, new DateTime(2018, 4, 4), "Hadera St, Tel Aviv-Yafo, Israel", 34.7868701, 32.0856885, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 3, 2, "Bella", "Highly intelligent and curious dog who learns tricks fast and loves solving puzzle toys with treats inside.", 6, 1, true, new DateTime(2018, 4, 11), "Netanya, Israel", 34.853196, 32.321458, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 4, 2, "Kai", "Easy-going temperament, great with kids and other dogs, loves weekend hikes and swimming in shallow creeks.", 55, 2, false, new DateTime(2014, 4, 9), "Tel Aviv District, Israel", 34.8072165, 32.0929075, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 5, 3, "Nena", "Playful companion with a big heart, enjoys fetch, sunbathing in the yard, and cuddles on the couch.", 117, 1, true, new DateTime(2024, 10, 7), "Rishon LeTsiyon, Israel", 34.8020886, 31.9590813, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 6, 3, "Shuggy", "Alert and loyal, always ready for adventure or a game of tug-of-war followed by a good nap", 117, 1, true, new DateTime(2018, 4, 4), "Binyamina-Giv'at Ada, Israel", 34.955096, 32.517078, false, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 7, 4, "Neomi", "Mischievous but sweet, has a habit of stealing socks, hiding them under the couch, and acting innocent.", 117, 1, true, new DateTime(2018, 4, 6), "Hadera, Israel", 34.9196518, 32.4340458, false, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 8, 4, "Bamba", "Shy at first but opens up with treats and patience, loves head scratches and walks in quiet areas.", 140, 0, false, new DateTime(2015, 4, 2), "Ramat David, Israel", 35.204036, 32.678991, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 9, 5, "Mailo", "Goofy personality, loves to roll in the grass and bark at butterflies while wagging non-stop.", 140, 1, false, new DateTime(2021, 4, 23), "Nahariyya, Israel", 35.0980514, 33.0085361, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 10, 5, "Emilia", "Protective and brave, always alert to visitors, but sweet and loving once introduced properly.", 78, 2, true, new DateTime(2021, 4, 8), "Aba Hillel Silver St, Haifa, Israel", 35.0097602, 32.7919908, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 11, 6, "Bamba", "Loves to nap in sunny spots by the window and snuggle under blankets during stormy weather.", 86, 1, true, new DateTime(2018, 4, 11), "Jerusalem, Israel", 35.21371, 31.768319, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 12, 6, "Lichi", "Adventurous spirit, always ready to explore new parks, meet new friends, and chase flying leaves.", 21, 1, false, new DateTime(2022, 4, 8), "Eilat, Israel", 34.951925, 29.557669, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 13, 7, "Nisha", "Very food-motivated, knows sit, stay, shake, and will do a dance for a piece of cheese.", 55, 2, false, new DateTime(2022, 4, 8), "Herzliya, Israel", 34.844675, 32.162413, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 14, 7, "Chicco", "Loyal and obedient, follows commands quickly, enjoys running beside a bike and sleeping at your feet.", 86, 0, false, new DateTime(2023, 4, 10), "Be'er Sheva, Israel", 34.7867691, 31.2521018, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 15, 8, "Gaya", "Charming and talkative, communicates with happy howls and expressive eyes that melt everyone's heart.", 55, 1, true, new DateTime(2022, 4, 13), "Netanya, Israel", 34.853196, 32.321458, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 16, 8, "Boost", "A senior soul with a calm presence, enjoys slow strolls and warm hugs from familiar hands.", 82, 2, false, new DateTime(2021, 4, 21), "Ramat Gan, Israel", 34.824785, 32.068424, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 17, 9, "Lucy", "Loves wearing sweaters in winter, prances proudly during walks, and greets every dog with a tail wag", 87, 2, true, new DateTime(2025, 2, 12), "Gedera, Israel", 34.7774347, 31.8120082, false, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 18, 9, "Zoola", "Can’t resist mud puddles and playing in the rain, often needs a bath after every adventure.", 139, 0, true, new DateTime(2021, 4, 7), "Kefar Sava, Israel", 34.90761, 32.178195, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 19, 10, "Humus", "Perfect family dog, gentle with toddlers, patient with chaos, and full of love for everyone.", 102, 0, false, new DateTime(2021, 4, 15), "Holon, Israel", 34.7706369, 32.0180655, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 20, 10, "Belle", "Enjoys doggy playdates, group walks, and riding in the backseat with ears flapping in the breeze", 86, 1, true, new DateTime(2018, 4, 11), "Tiberias, Israel", 35.530973, 32.795859, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 21, 11, "Fluko", "Sassy and opinionated, growls at vacuum cleaners but happily cuddles after expressing disapproval", 70, 0, false, new DateTime(2023, 2, 10), "Bat Yam, Israel", 34.752885, 32.0151439, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 22, 12, "Amy", "Eager to please, easy to train, and always carries a toy around like it’s treasure.", 116, 0, true, new DateTime(2018, 4, 11), "Petah Tikva, Israel", 34.887762, 32.084041, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 23, 13, "Bombilo", "Big personality in a tiny body, fearless around larger dogs and climbs onto every lap in reach.", 140, 1, false, new DateTime(2023, 4, 11), "Qiryat Shemona, Israel", 35.5699622, 33.20809, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 24, 14, "Mishka", "Strong and athletic, loves obstacle courses and shows off jumping over small fences like a champ", 19, 1, false, new DateTime(2023, 8, 14), "Haifa, Israel", 34.989571, 32.7940463, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 25, 15, "Uzi", "Clingy but adorable, follows you from room to room and insists on sharing your pillow at night.", 70, 0, false, new DateTime(2018, 4, 13), "Ayalon Hwy, Holon, Israel", 34.762263, 32.0150071, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 26, 16, "Hugo", "Always excited for car rides, especially when they lead to beach runs and sunset fetch sessions.", 33, 0, false, new DateTime(2017, 4, 4), "Zikhron Ya'akov, Israel", 34.9520929, 32.5739532, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 27, 17, "Loona", "Cuddles on demand, sleeps like a log, and dreams of chasing cats and eating bacon.", 86, 2, true, new DateTime(2018, 4, 10), "Ramat Hasharon, Israel", 34.840278, 32.137793, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 28, 18, "Luc", "Very expressive, tilts head in curiosity to every word and barks to join the conversation", 140, 1, false, new DateTime(2025, 2, 3), "Bet Shemesh, Israel", 34.9866855, 31.7496628, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 29, 19, "Belle", "Good with cats, birds, rabbits, and sometimes even turtles. Everyone is a friend", 2, 2, true, new DateTime(2017, 4, 23), "Harish, Israel", 35.0478887, 32.4606442, true, false, DateTime.UtcNow, DateTime.UtcNow },
                    { 30, 20, "Tiny", "Terrified of balloons, but otherwise confident, strong, and happy-go-lucky", 35, 0, true, new DateTime(2024, 9, 16), "Zikhron Ya'akov, Israel", 34.9520929, 32.5739532, true, false, DateTime.UtcNow, DateTime.UtcNow },
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
                    { 77, null, 3, 20, "https://storage.googleapis.com/tail_buddys_bucket1/park20o4.jpg" },
                    { 78, 1, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_21_47175904-c747-4365-9eaa-4f59645604c2" },
                    { 79, 1, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_21_c7f3a6e3-003b-48c1-840f-45984c9406d1" },
                    { 80, 1, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_21_925a219d-ec9a-490d-86bf-7a626fbb640d" },
                    { 81, 1, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_21_d66c6418-1fb3-49c8-8bc5-57cfa5975969" },
                    { 82, 1, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_21_e7b564c4-1283-48ce-b3ac-c1c5c81f1a7a" },
                    { 83, 2, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_22_afede407-1fa1-4b86-9e4d-d841ff7d301c" },
                    { 84, 2, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_22_ed62a571-d5e0-4912-ad7f-d6b063f7f68d" },
                    { 85, 3, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_23_35f5e65d-58d6-4648-9590-40cfe0ba06bf" },
                    { 86, 3, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_23_5085ec35-38b8-47fd-9429-9d607587ac67" },
                    { 87, 3, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_23_f850122b-f90c-44d6-a0c7-6c1a43b21b4c" },
                    { 88, 3, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_23_3efd3295-84b3-4195-b96e-2ecc031ca8f1" },
                    { 89, 4, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_25_a6722b1a-7f7a-4250-8a1b-6598b20b2323" },
                    { 90, 4, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_25_aba4323b-1876-4566-904f-40c637ec1181" },
                    { 91, 4, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_25_8916a8d5-4761-40ab-b6fa-dff5bf84eb21" },
                    { 92, 4, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_25_f7923fc1-82b3-46d1-bec3-1b60d870f328" },
                    { 93, 4, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_25_74668f25-afcb-42f0-8e06-e54f7d0514f8" },
                    { 94, 5, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_26_c18cd9b2-5156-4ad6-8be8-869825d0ff17" },
                    { 95, 5, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_26_c6a56fb9-56a8-4d58-bdfa-5417ca68d5e8" },
                    { 96, 5, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_26_9d2be4c3-c892-4ad1-8d8e-be322d3d3a56" },
                    { 97, 5, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_26_e7f37ab5-835e-466a-aa59-42fefddc35db" },
                    { 98, 5, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_26_48d0e6db-522e-4cac-a6e3-43326f4d56b4" },
                    { 99, 6, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_27_cac8b5b5-890d-48a2-84fa-3b8af73523ef" },
                    { 100,6, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_27_24b9fa65-7745-4d33-b0bf-29715cb6daa5" },
                    { 101,6, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_27_80897897-e36d-45dc-8347-8cf709a146ab" },
                    { 102, 7, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_28_3a1c1e2e-d20d-4d52-916a-dce10aa97378" },
                    { 103, 7, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_28_19d3e9c8-7bcb-4c9a-9d91-1d3ae9260dc7" },
                    { 104, 7, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_28_8bc86144-1ddc-4953-84aa-d74ef27af90a" },
                    { 105, 7, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_28_98ceec26-2789-4c9a-8f7b-e0ce3ddc0907" },
                    { 106, 7, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_28_22edf7d0-04e8-4f94-84e5-f568666966ce" },
                    { 107, 8, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_29_8506f657-7136-4b4d-b374-1eaff304887e" },
                    { 108, 8, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_29_72a9068d-bfa6-43d4-833d-1985126fd47b" },
                    { 109, 8, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_29_0bfa1ee9-669a-4622-9dc8-e627f266bb77" },
                    { 110, 8, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_29_d006dd43-cefc-404b-983e-d77b52046ade" },
                    { 111, 8, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_29_1f850362-df3f-4bc5-b9f1-ee7d1072a15f" },
                    { 112, 9, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_30_6ed718fc-cee6-4909-a74f-cd95ddbe95dd" },
                    { 113, 9, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_30_1ba52269-b31e-437e-ae85-ebe42f060861" },
                    { 114, 9, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_30_4680578d-15d8-496d-845e-440252228726" },
                    { 115, 9, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_30_6458f057-df97-4152-b626-0db6afb221ac" },
                    { 116, 9, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_30_4955d514-f0be-460d-933c-f596abb7a55e" },
                    { 117, 10, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_31_9dbc2566-0142-4868-b6b9-4c3ff4344ac9" },
                    { 118, 10, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_31_888475db-6686-434c-8321-afe66540e78d" },
                    { 119, 10, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_31_726d8de6-ea01-4aee-8c14-4e0ec3508adc" },
                    { 120, 10, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_31_91c5fc6e-bd06-4566-8cb6-36bfe5307314" },
                    { 121, 11, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_32_f1868e88-f6dd-4d33-8244-8a586a3ebe72" },
                    { 122, 11, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_32_e5824fc0-57d1-4786-8330-1cdabcd9e87b" },
                    { 123, 11, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_32_feeea119-a475-4612-bfce-eb5c488c91c2" },
                    { 124, 11, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_32_b3a1fdcf-9ccd-4cec-8926-b1c67a82a299" },
                    { 125, 11, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_32_faa17b22-5962-4330-8ac1-67d3cac69dad" },
                    { 126, 12, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_33_291c8ad1-498f-4642-8391-7a6f470c90f3" },
                    { 127, 12, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_33_f1c3cd6f-ac4d-4b9d-9af7-0c947ac1b9fc" },
                    { 128, 12, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_33_4114d99b-8b9c-4f1f-8a6b-44dac1cf7290" },
                    { 129, 12, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_33_740a3de3-0514-4271-8673-809235b491ab" },
                    { 130, 12, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_33_ef1b163c-7cf1-4d4b-82ef-9a5c1be57f79" },
                    { 131, 13, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_34_5f096427-912f-48f2-b16f-0b89eea1a727" },
                    { 132, 13, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_34_8f7cfa53-b68a-450d-9fc8-e7ef707938cf" },
                    { 133, 13, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_34_733900db-be97-4824-bb86-a000ddd31223" },
                    { 134, 13, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_34_7e58bb88-7c12-422c-becf-2018bbd51059" },
                    { 135, 13, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_34_17c531a3-ecf6-4a0d-abe2-346a77da6096" },
                    { 136, 14, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_35_246f13ea-06aa-4e67-8acb-4bac3b3ed3bb" },
                    { 137, 14, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_35_205d1679-5ecf-4472-9ce0-97937b5ba165" },
                    { 138, 14, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_35_3724ff95-9806-4952-84da-caf63c4344f6" },
                    { 139, 14, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_35_56ecd833-19eb-4060-9329-a82b3a209dc2" },
                    { 140, 14, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_35_f419a60e-afeb-41b9-a002-09b501b6aaa1" },
                    { 141, 15, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_36_27af8f42-9530-4e6b-9df4-7f7d904b31e8" },
                    { 142, 15, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_36_9d0ce074-0daa-475a-b549-8c93f6b03904" },
                    { 143, 15, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_36_6b0ad8bd-cca8-4196-9152-bc2cb5f697e9" },
                    { 144, 15, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_36_65d46012-e16e-4038-a10f-ccb70d93932a" },
                    { 145, 15, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_36_4b0fb780-dad9-4d59-bdfe-616e6d7e2e72" },
                    { 146, 16, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_37_a689dc36-e6e9-4cde-ace4-c513e278d780" },
                    { 147, 16, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_37_bdeb0592-54e3-495a-89a6-04cff8c882b1" },
                    { 148, 16, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_37_9e5837ba-a97d-4ce4-9c38-a25f070ea2dc" },
                    { 149, 16, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_37_07e83347-31a2-45ee-8534-24e0ea348bed" },
                    { 150, 16, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_37_a74505a3-c4be-427d-81db-50e930f0bd26" },
                    { 151, 17, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_38_4b88ed8c-6533-4bdd-9bb6-2aee9d42ad0a" },
                    { 152, 17, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_38_62ef7c5c-513e-4619-a31f-52222f874b6d" },
                    { 153, 17, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_38_d63bf08c-6dd1-40be-9482-459bf592d5e8" },
                    { 154, 17, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_38_25950680-e5e6-48c5-8e96-f972472b63e1" },
                    { 155, 17, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_38_cf8c3d3c-b316-41ee-92ac-51b697269bbf" },
                    { 156, 18, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_39_a3470959-4e87-403a-a2a2-31739cdccba9" },
                    { 157, 18, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_39_a5f505f6-becc-4680-9f9b-c3df3d56bf2b" },
                    { 158, 18, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_39_0dd50832-3f2e-47ef-98f2-d4aef51d58d0" },
                    { 159, 18, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_39_c7077d1d-9c54-4af2-a7c2-7303c668b302" },
                    { 160, 18, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_39_39bc882c-d66a-4465-b74b-e71f6a22bcef" },
                    { 161, 19, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_40_f40beec1-c777-43c1-9fde-7fb9a1e51e7c" },
                    { 162, 19, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_40_ab747688-44ba-4a3f-b3e1-9cc751ecd2e7" },
                    { 163, 19, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_40_709df589-a9e4-4e48-a374-0ccab5bfdfe0" },
                    { 164, 20, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_41_72aeb13e-a413-486b-8d85-7ef722311d88" },
                    { 165, 20, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_41_ee96a4ff-32c3-4cf0-8ee8-fe8652e72680" },
                    { 166, 20, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_41_5bc1c3be-5a81-4524-be07-755bf5b95178" },
                    { 167, 20, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_41_bea8bf13-1553-4f4c-bd01-3b6e5ac67a2d" },
                    { 168, 20, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_41_befebc03-2e6e-442c-a753-98271cfd58db" },
                    { 169, 21, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_42_a649ca50-d327-4c0d-ac71-29a3215f934e" },
                    { 170, 21, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_42_0cadfb81-0523-4a87-b1fa-03a391e1a063" },
                    { 171, 21, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_42_b5981cc5-c655-4b9f-9176-e3c7bf0e5035" },
                    { 172, 21, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_42_be4f895b-9462-44f2-8676-b27002d2349b" },
                    { 173, 22, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_43_48aea286-0458-4619-9c35-e30b73bd9dfb" },
                    { 174, 22, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_43_c3f43ec3-e5c2-483e-9eae-f7b584013f10" },
                    { 175, 22, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_43_0a77462d-8cc3-4911-8a7c-c701c10fd58c" },
                    { 176, 22, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_43_4f10188a-11d1-4db0-9395-bb5a860a0d59" },
                    { 177, 22, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_43_61805780-b3bd-4da9-beb3-745985dc36da" },
                    { 178, 23, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_44_78fd9cd3-ed9f-4a0b-bb01-3477699749ae" },
                    { 179, 23, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_44_2cea7319-4e4e-43a5-ae9e-9eb90c766b67" },
                    { 180, 24, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_45_7bf7220b-7c89-4338-aea1-09bcaa84c923" },
                    { 181, 24, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_45_0b71cee0-6c6b-4431-9341-014c7dc912d2" },
                    { 182, 24, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_45_f504252a-7c77-435a-a161-cb5437237550" },
                    { 183, 25, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_46_6d221da2-6997-4dcb-b523-490884d7e410" },
                    { 184, 25, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_46_ac98b89b-fdd1-4b59-ae8c-c5faf12ada25" },
                    { 185, 25, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_46_ed4ae69c-ef86-4bd9-a637-d5ccbf6409a9" },
                    { 186, 25, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_46_b47fff36-115a-47e5-aeeb-7f90e3c9b7cc" },
                    { 187, 25, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_46_63f9eca2-4a09-4976-b64c-9608841fc5ae" },
                    { 188, 26, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_47_385989b4-7ce0-4326-ba91-889262e17734" },
                    { 189, 26, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_47_fa7a335d-f0e7-4b9c-a04a-e156a27e9d32" },
                    { 190, 26, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_47_8f21db6a-edef-45d7-9821-c0fde31a3ea3" },
                    { 191, 26, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_47_30dc70f7-323e-439c-a455-e45ffb56278b" },
                    { 192, 26, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_47_0398ea50-2b39-4e43-bf8d-7607603c2f70" },
                    { 193, 27, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_48_5a249ab8-467e-41e9-9b32-7be3f6f332df" },
                    { 194, 27, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_48_4e0a027b-282a-4e48-88de-9d04909e78fa" },
                    { 195, 27, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_48_bfb959df-f4e1-46fd-b826-29c6e71dc361" },
                    { 196, 27, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_48_9251d621-f37f-477e-9552-87e2d90485e3" },
                    { 197, 27, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_48_2ae0a954-e7f9-4cf0-a2bc-0b1a41335d53" },
                    { 198, 28, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_49_f4b5c539-37c2-4214-96e4-6ac56820851b" },
                    { 199, 28, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_49_ad848918-4fc0-4a9a-ae80-7c002d141b94" },
                    { 200, 28, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_49_f1ecbf85-5d86-4227-9040-57fb97b5ed18" },
                    { 201, 28, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_49_21efbe81-41c0-42f8-88a0-89897a3ccf1b" },
                    { 202, 28, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_49_c6f9f956-c2a3-495a-bea8-9e1b1192c9b9" },
                    { 203, 29, 4, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_50_88b968fa-b3f5-4480-ba24-a380d4e2342f" },
                    { 204, 29, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_50_64fcabea-0d5a-4722-8897-d85ab0ea1300" },
                    { 205, 29, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_50_92dfeff5-f7d5-46bd-95d8-de4e42605b44" },
                    { 206, 29, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_50_77fe2b81-3960-4bd1-8884-7c068c7075b3" },
                    { 207, 29, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_50_ba0d8469-e158-46f6-9925-ace5726887f9" },
                    { 208, 30, 0, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_51_1db04eb2-8705-48e0-8ccb-4ea3d4b81464" },
                    { 209, 30, 3, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_51_5c55ecf2-68a1-4384-9374-0a8550227ab8" },
                    { 210, 30, 2, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_51_99264ab2-773b-48ed-b98e-1b3f2f8cd2bf" },
                    { 211, 30, 1, null, "https://storage.googleapis.com/tail_buddys_bucket1/0_51_01e4bc2b-f18e-434f-911b-3b3f72b9a14e" }
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
                    { 20, 19 },
                    { 21, 7 },
                    { 21, 11 },
                    { 21, 18 },
                    { 21, 19 },
                    { 22, 3 },
                    { 22, 4 },
                    { 22, 10 },
                    { 22, 12 },
                    { 22, 15 },
                    { 23, 1 },
                    { 23, 6 },
                    { 23, 20 },
                    { 24, 1 },
                    { 24, 3 },
                    { 24, 10 },
                    { 24, 20 },
                    { 25, 1 },
                    { 25, 5 },
                    { 25, 14 },
                    { 26, 6 },
                    { 26, 8 },
                    { 26, 10 },
                    { 26, 15 },
                    { 27, 8 },
                    { 27, 10 },
                    { 27, 11 },
                    { 27, 20 },
                    { 28, 3 },
                    { 28, 12 },
                    { 28, 15 },
                    { 28, 16 },
                    { 28, 19 },
                    { 29, 6 },
                    { 29, 9 },
                    { 29, 16 },
                    { 29, 19 },
                    { 30, 1 },
                    { 30, 5 },
                    { 30, 19 }
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
