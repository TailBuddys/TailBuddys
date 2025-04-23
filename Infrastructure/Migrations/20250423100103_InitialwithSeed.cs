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
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    { 1, "74 Herzl St, Rehovot", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.803725685895337, 35.08125940049338, "Gan Meir", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 2, "72 Hagana St, Beit Shemesh", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 31.725036064898752, 34.954051845419912, "Independence Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 3, "22 Aharonovitch St, Herzliya", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.005056765930959, 35.047166356555692, "Yarkon Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 4, "135 Menachem Begin St, Bat Yam", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.16620074472419, 34.737777651569608, "Sacher Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 5, "164 Weizmann St, Ashdod", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.195625805117935, 34.996128479064353, "Ramat Gan National Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 6, "151 Dizengoff St, Nazareth", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.958522208954417, 34.758238393754027, "HaPisga Garden", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 7, "151 Dizengoff St, Rehovot", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.288782473028167, 34.834898922475631, "Canada Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 8, "151 Dizengoff St, Nazareth", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 32.242960511823114, 35.154823685046118, "Ashdod Yam Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 9, "164 Menachem Begin St, Petah Tikva", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 31.455896755262145, 35.093623190359494, "Gan Ha'ir", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 10, "26 Weizmann St, Herzliya", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.327378026598232, 35.017367928682944, "Armon Hanatziv Promenade", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 11, "91 Ben Gurion St, Ashkelon", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 31.956513248467385, 35.044852696683627, "Liberty Bell Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 12, "13 Hagana St, Holon", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 29.893493371060131, 35.072434110942233, "Lachish Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 13, "59 Menachem Begin St, Ashkelon", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.690470323125162, 35.189430963909892, "Ariel Sharon Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 14, "189 HaPalmach St, Netanya", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.077056006116386, 34.845375654818014, "Park Giv'atayim", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 15, "140 Weizmann St, Tel Aviv", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 32.287636410949133, 35.058807202618212, "Herzliya Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 16, "165 Herzl St, Hadera", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.942294456861362, 35.282088308709355, "Park Ashkelon", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 17, "23 Aharonovitch St, Tel Aviv", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.995432802309619, 35.237696523311882, "Beit She'an Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 18, "13 Hagana St, Holon", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.562650586009166, 34.837084187746015, "Ein Gedi Reserve", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 19, "92 Dizengoff St, Modiin", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 32.907255533165753, 35.14256578911349, "Neot Kedumim", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 20, "86 Hagana St, Herzliya", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 32.135416503603089, 35.271950058982455, "Ma'ayan Harod", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 21, "133 HaPalmach St, Netanya", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 31.153804826036271, 35.429314614064118, "Timna Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 22, "72 Hagana St, Beit Shemesh", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 31.199151251395598, 34.935912882533579, "Alona Park", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 23, "74 Herzl St, Rehovot", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.186424658443141, 34.929330163957317, "Park Kiryat Motzkin", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 24, "181 Herzl St, Bat Yam", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Beautiful park for dogs. Great for playing and resting.", 30.57833704702006, 35.360322825455178, "Bat Galim Promenade", new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "Email", "FirstName", "Gender", "GoogleId", "IsAdmin", "LastName", "PasswordHash", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2004, 7, 7, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1953), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user1@tail.com", "User1", 2, null, false, "Last1", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 2, new DateTime(2002, 5, 6, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1973), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user2@tail.com", "User2", 2, null, false, "Last2", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 3, new DateTime(1979, 6, 26, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1978), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user3@tail.com", "User3", 2, null, false, "Last3", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 4, new DateTime(2001, 11, 25, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1981), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user4@tail.com", "User4", 0, null, false, "Last4", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 5, new DateTime(1986, 12, 11, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1984), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user5@tail.com", "User5", 0, null, false, "Last5", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 6, new DateTime(1981, 5, 7, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1988), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user6@tail.com", "User6", 0, null, false, "Last6", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 7, new DateTime(1992, 12, 15, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1991), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user7@tail.com", "User7", 2, null, false, "Last7", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 8, new DateTime(2007, 5, 1, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1993), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user8@tail.com", "User8", 1, null, false, "Last8", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 9, new DateTime(2001, 9, 22, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1997), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user9@tail.com", "User9", 0, null, false, "Last9", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 10, new DateTime(1982, 12, 21, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2000), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user10@tail.com", "User10", 2, null, false, "Last10", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 11, new DateTime(1984, 6, 10, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2003), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user11@tail.com", "User11", 2, null, false, "Last11", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 12, new DateTime(1983, 5, 19, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2005), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user12@tail.com", "User12", 2, null, false, "Last12", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 13, new DateTime(1976, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2008), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user13@tail.com", "User13", 2, null, false, "Last13", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 14, new DateTime(2003, 3, 18, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2011), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user14@tail.com", "User14", 0, null, false, "Last14", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 15, new DateTime(1991, 7, 2, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2013), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user15@tail.com", "User15", 0, null, false, "Last15", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 16, new DateTime(1996, 6, 26, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2017), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user16@tail.com", "User16", 1, null, false, "Last16", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 17, new DateTime(1985, 1, 14, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2019), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user17@tail.com", "User17", 0, null, false, "Last17", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 18, new DateTime(2000, 11, 26, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2065), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user18@tail.com", "User18", 0, null, false, "Last18", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 19, new DateTime(2001, 12, 4, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2070), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user19@tail.com", "User19", 2, null, false, "Last19", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) },
                    { 20, new DateTime(1979, 12, 14, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(2074), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "user20@tail.com", "User20", 1, null, false, "Last20", null, null, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457) }
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "Description", "Gender", "IsBot", "Lat", "Lon", "Name", "Size", "Type", "UpdatedAt", "UserId", "Vaccinated" },
                values: new object[,]
                {
                    { 1, "61 Jabotinsky St, Modiin", new DateTime(2018, 6, 3, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Lola.", true, false, 30.640431199876161, 35.408581198529077, "Olive", 1, 6, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 1, true },
                    { 2, "133 Hagana St, Hadera", new DateTime(2019, 11, 27, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Sadie.", false, false, 30.733736929248877, 34.708256827775905, "Stella", 1, 9, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 2, true },
                    { 3, "110 Rothschild St, Jerusalem", new DateTime(2018, 8, 29, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Lola.", true, false, 32.146585539432763, 35.046480338148854, "Ace", 2, 4, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 3, true },
                    { 4, "159 King David St, Ashkelon", new DateTime(2016, 1, 10, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Leo.", true, false, 31.058638490468521, 34.796184218521375, "Sadie", 1, 7, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 4, true },
                    { 5, "133 Yitzhak Rabin St, Eilat", new DateTime(2024, 11, 8, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Luna.", false, true, 31.387292768760137, 34.842299004318001, "Jack", 1, 6, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 5, true },
                    { 6, "133 Yitzhak Rabin St, Eilat", new DateTime(2019, 10, 10, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Hazel.", true, false, 30.091991952509723, 35.168837978949121, "Lucky", 0, 0, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 6, true },
                    { 7, "182 Menachem Begin St, Tel Aviv", new DateTime(2016, 1, 19, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Buddy.", true, false, 29.858077914402024, 34.942563039231899, "Max", 2, 5, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 7, true },
                    { 8, "179 Hagana St, Petah Tikva", new DateTime(2018, 12, 7, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Rosie.", true, false, 31.776591754499869, 35.06003130659569, "Otis", 2, 8, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 8, true },
                    { 9, "159 King David St, Ashkelon", new DateTime(2021, 11, 16, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Rex.", true, true, 31.783936653365281, 35.137342901173454, "Pepper", 1, 8, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 9, true },
                    { 10, "110 Rothschild St, Jerusalem", new DateTime(2018, 2, 2, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Jack.", false, false, 31.839718508472352, 35.381299113049742, "Stella", 0, 6, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 10, true },
                    { 11, "26 HaPalmach St, Beit Shemesh", new DateTime(2021, 7, 26, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Nova.", false, false, 30.218509017174636, 34.833884672433157, "Gracie", 1, 7, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 11, true },
                    { 12, "189 HaPalmach St, Beit Shemesh", new DateTime(2019, 11, 9, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Benji.", true, false, 31.078484771090253, 35.203869910512211, "Bella", 1, 6, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 12, false },
                    { 13, "150 Allenby St, Modiin", new DateTime(2016, 10, 5, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Harley.", false, false, 32.467574562400195, 34.982222104068214, "Stella", 2, 2, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 13, true },
                    { 14, "109 Yitzhak Rabin St, Ashdod", new DateTime(2021, 2, 20, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Sky.", true, false, 30.757461654958824, 34.970683634830586, "Luna", 0, 2, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 14, true },
                    { 15, "68 Jabotinsky St, Kiryat Gat", new DateTime(2022, 1, 12, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Sky.", true, true, 31.207649239441132, 35.105008693338988, "Jasper", 0, 8, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 15, true },
                    { 16, "31 Arlozorov St, Petah Tikva", new DateTime(2018, 8, 11, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Otis.", true, false, 30.369541918154951, 35.276504067777871, "Marley", 2, 0, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 16, true },
                    { 17, "159 Menachem Begin St, Modiin", new DateTime(2020, 2, 18, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Benji.", true, false, 31.966179520031542, 35.196761291569487, "Lucky", 1, 9, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 17, true },
                    { 18, "144 Ben Gurion St, Hadera", new DateTime(2022, 10, 29, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Rex.", true, false, 30.799090133364668, 35.201983595944526, "Lucky", 2, 2, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 18, true },
                    { 19, "95 King David St, Tiberias", new DateTime(2024, 11, 5, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Sasha.", true, false, 31.688498091576513, 35.357313938839276, "Sasha", 1, 1, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 19, true },
                    { 20, "151 King David St, Hadera", new DateTime(2016, 5, 16, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), "Friendly dog that loves Lucy.", true, false, 32.830731840275625, 34.946591203507509, "Rocky", 0, 1, new DateTime(2025, 4, 23, 10, 1, 2, 643, DateTimeKind.Utc).AddTicks(1457), 20, true }
                });

            migrationBuilder.InsertData(
                table: "DogParks",
                columns: new[] { "DogLikesId", "FavParksId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 15 },
                    { 1, 19 },
                    { 2, 1 },
                    { 2, 19 },
                    { 2, 24 },
                    { 3, 5 },
                    { 3, 7 },
                    { 3, 10 },
                    { 3, 12 },
                    { 3, 23 },
                    { 4, 8 },
                    { 4, 19 },
                    { 5, 2 },
                    { 5, 4 },
                    { 5, 7 },
                    { 5, 16 },
                    { 6, 9 },
                    { 6, 10 },
                    { 7, 1 },
                    { 7, 4 },
                    { 7, 12 },
                    { 7, 22 },
                    { 7, 23 },
                    { 8, 5 },
                    { 8, 6 },
                    { 8, 7 },
                    { 8, 9 },
                    { 8, 11 },
                    { 9, 12 },
                    { 9, 13 },
                    { 9, 23 },
                    { 10, 9 },
                    { 10, 23 },
                    { 11, 1 },
                    { 11, 2 },
                    { 11, 10 },
                    { 12, 1 },
                    { 12, 11 },
                    { 12, 13 },
                    { 13, 6 },
                    { 13, 8 },
                    { 13, 9 },
                    { 13, 18 },
                    { 14, 1 },
                    { 14, 16 },
                    { 15, 7 },
                    { 15, 18 },
                    { 16, 23 },
                    { 16, 24 },
                    { 17, 12 },
                    { 17, 17 },
                    { 18, 7 },
                    { 18, 15 },
                    { 18, 20 },
                    { 18, 22 },
                    { 19, 14 },
                    { 20, 3 },
                    { 20, 16 },
                    { 20, 23 }
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
