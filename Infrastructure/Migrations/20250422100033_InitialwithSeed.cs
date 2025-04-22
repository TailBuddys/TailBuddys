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
                    ReceiverDogId = table.Column<int>(type: "int", nullable: false)
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
                    { 1, "189 Weizmann St, Nazareth", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.069288199804017, 35.006777362417019, "Gan Meir", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 2, "163 Allenby St, Beit Shemesh", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.448240338888613, 35.364853386938968, "Independence Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 3, "190 Herzl St, Petah Tikva", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.812748478983007, 34.727004137170169, "Yarkon Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 4, "51 Allenby St, Tiberias", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.7936932256139, 34.986387987444147, "Sacher Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 5, "191 Allenby St, Modiin", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.108591891694548, 34.888822867770372, "Ramat Gan National Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 6, "185 King David St, Netanya", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 32.615555265448442, 35.136573526678433, "HaPisga Garden", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 7, "199 Herzl St, Rehovot", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.722862846798083, 34.743257869342393, "Canada Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 8, "30 Arlozorov St, Hadera", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 32.143078813719718, 35.15004255689999, "Ashdod Yam Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 9, "199 Herzl St, Rehovot", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.274865492626304, 35.16566064029945, "Gan Ha'ir", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 10, "81 Allenby St, Beit Shemesh", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.946315971567543, 35.441871702704418, "Armon Hanatziv Promenade", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 11, "105 Arlozorov St, Netanya", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.737274504275049, 35.097497530130781, "Liberty Bell Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 12, "176 Ben Gurion St, Jerusalem", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.307368999281589, 34.872268925268919, "Lachish Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 13, "104 King David St, Beer Sheva", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.167718978764498, 35.167208075036285, "Ariel Sharon Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 14, "184 Ben Gurion St, Modiin", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.182260782860055, 35.009898771449514, "Park Giv'atayim", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 15, "65 HaShalom St, Rehovot", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 29.609916306640887, 34.862890909036736, "Herzliya Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 16, "183 Arlozorov St, Modiin", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.705701722316231, 35.249727008082751, "Park Ashkelon", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 17, "119 Aharonovitch St, Beit Shemesh", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.947423513694464, 35.239549960849367, "Beit She'an Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 18, "121 Herzl St, Ashkelon", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 30.389314457882712, 35.07870247788226, "Ein Gedi Reserve", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 19, "130 HaPalmach St, Modiin", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.736471099589998, 34.884448424899659, "Neot Kedumim", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 20, "17 Arlozorov St, Ashkelon", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.178892991237337, 35.359587515227872, "Ma'ayan Harod", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 21, "151 Herzl St, Bat Yam", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.193899603054195, 35.379169792680067, "Timna Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 22, "163 Allenby St, Beit Shemesh", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 29.940032466542284, 34.904774858082376, "Alona Park", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 23, "188 Jabotinsky St, Rehovot", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 31.740292443118594, 35.325978076515696, "Park Kiryat Motzkin", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 24, "184 Ben Gurion St, Modiin", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Beautiful park for dogs. Great for playing and resting.", 32.886822260235405, 34.918373244149777, "Bat Galim Promenade", new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "Email", "FirstName", "Gender", "GoogleId", "IsAdmin", "LastName", "PasswordHash", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2004, 3, 2, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9890), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user1@tail.com", "User1", 0, null, false, "Last1", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 2, new DateTime(1986, 3, 11, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9924), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user2@tail.com", "User2", 0, null, false, "Last2", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 3, new DateTime(2008, 1, 24, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9930), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user3@tail.com", "User3", 0, null, false, "Last3", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 4, new DateTime(2004, 6, 13, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9937), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user4@tail.com", "User4", 1, null, false, "Last4", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 5, new DateTime(1999, 10, 15, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9944), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user5@tail.com", "User5", 0, null, false, "Last5", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 6, new DateTime(1976, 10, 18, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9951), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user6@tail.com", "User6", 0, null, false, "Last6", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 7, new DateTime(1984, 7, 5, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9958), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user7@tail.com", "User7", 2, null, false, "Last7", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 8, new DateTime(1993, 7, 20, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9965), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user8@tail.com", "User8", 2, null, false, "Last8", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 9, new DateTime(1983, 4, 17, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9972), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user9@tail.com", "User9", 2, null, false, "Last9", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 10, new DateTime(1977, 11, 10, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(9980), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user10@tail.com", "User10", 0, null, false, "Last10", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 11, new DateTime(1996, 9, 18, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(79), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user11@tail.com", "User11", 2, null, false, "Last11", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 12, new DateTime(1990, 10, 16, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(86), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user12@tail.com", "User12", 2, null, false, "Last12", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 13, new DateTime(2004, 7, 24, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(94), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user13@tail.com", "User13", 1, null, false, "Last13", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 14, new DateTime(1995, 8, 3, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(102), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user14@tail.com", "User14", 2, null, false, "Last14", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 15, new DateTime(1994, 9, 29, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(108), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user15@tail.com", "User15", 1, null, false, "Last15", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 16, new DateTime(1989, 11, 7, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(114), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user16@tail.com", "User16", 2, null, false, "Last16", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 17, new DateTime(1982, 12, 1, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(121), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user17@tail.com", "User17", 1, null, false, "Last17", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 18, new DateTime(2002, 11, 16, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(127), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user18@tail.com", "User18", 2, null, false, "Last18", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 19, new DateTime(1985, 7, 10, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(133), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user19@tail.com", "User19", 0, null, false, "Last19", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) },
                    { 20, new DateTime(1998, 9, 30, 10, 0, 32, 113, DateTimeKind.Utc).AddTicks(140), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "user20@tail.com", "User20", 2, null, false, "Last20", null, null, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714) }
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "Description", "Gender", "IsBot", "Lat", "Lon", "Name", "Size", "Type", "UpdatedAt", "UserId", "Vaccinated" },
                values: new object[,]
                {
                    { 1, "189 Yitzhak Rabin St, Rehovot", new DateTime(2020, 6, 30, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Sasha.", true, false, 30.02279791006703, 35.02987148102909, "Winnie", 1, 3, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 1, true },
                    { 2, "168 HaShalom St, Netanya", new DateTime(2024, 6, 12, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Buddy.", true, false, 32.093257450271622, 35.22312043159193, "Zeus", 1, 3, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 2, false },
                    { 3, "160 Jabotinsky St, Tel Aviv", new DateTime(2015, 6, 7, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Simba.", true, false, 30.439248811254867, 35.270164751878944, "Finn", 2, 9, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 3, true },
                    { 4, "50 Jabotinsky St, Haifa", new DateTime(2022, 9, 12, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Charlie.", true, false, 29.769791740361303, 35.036427089186418, "Scout", 0, 1, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 4, true },
                    { 5, "180 Herzl St, Netanya", new DateTime(2015, 7, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Sam.", false, false, 32.614064428945873, 35.079831776756521, "Daisy", 2, 8, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 5, true },
                    { 6, "46 Hagana St, Ramat Gan", new DateTime(2018, 6, 15, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Ellie.", true, false, 31.33084872652989, 34.985237149380509, "Winnie", 1, 5, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 6, true },
                    { 7, "132 Rothschild St, Ashkelon", new DateTime(2024, 12, 14, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Zoe.", false, true, 31.528470989993522, 35.181771288619828, "Scout", 2, 0, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 7, true },
                    { 8, "119 Dizengoff St, Bat Yam", new DateTime(2018, 11, 12, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Honey.", false, false, 30.416631747446072, 34.857384171298129, "Milo", 2, 1, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 8, true },
                    { 9, "123 Herzl St, Nazareth", new DateTime(2018, 12, 8, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Rex.", true, false, 31.173593726202309, 35.416953542718105, "Rocky", 0, 5, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 9, false },
                    { 10, "125 Aharonovitch St, Holon", new DateTime(2016, 9, 21, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Sam.", false, false, 30.871736483158717, 35.062910036536323, "Jasper", 2, 9, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 10, true },
                    { 11, "191 Weizmann St, Beit Shemesh", new DateTime(2024, 8, 9, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Ginger.", false, false, 32.450180042761097, 35.021175391052559, "Sadie", 1, 4, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 11, true },
                    { 12, "24 Menachem Begin St, Ashdod", new DateTime(2024, 12, 3, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Stella.", false, false, 31.511282098675814, 35.029709564979107, "Nova", 2, 0, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 12, true },
                    { 13, "197 Rothschild St, Holon", new DateTime(2022, 8, 12, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Scout.", true, false, 31.214464759719434, 35.098992632889882, "Ginger", 1, 1, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 13, true },
                    { 14, "132 Rothschild St, Ashkelon", new DateTime(2018, 6, 14, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Sky.", true, false, 29.734861955524391, 34.979740385488135, "Winnie", 2, 5, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 14, true },
                    { 15, "22 Dizengoff St, Nazareth", new DateTime(2023, 8, 8, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Jack.", true, false, 31.23051891452225, 34.935224973524697, "Zoe", 0, 7, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 15, false },
                    { 16, "176 Arlozorov St, Petah Tikva", new DateTime(2017, 11, 27, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Ellie.", true, true, 31.136925642725611, 35.437282351581572, "Moose", 1, 6, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 16, true },
                    { 17, "73 Aharonovitch St, Haifa", new DateTime(2016, 7, 25, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Hazel.", true, false, 31.423585718571452, 35.337085756541555, "Shadow", 0, 9, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 17, true },
                    { 18, "97 Menachem Begin St, Tel Aviv", new DateTime(2020, 1, 31, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Zeus.", false, false, 31.227313805742241, 34.970613551189402, "Ace", 0, 2, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 18, true },
                    { 19, "168 Weizmann St, Eilat", new DateTime(2018, 2, 15, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Sadie.", false, false, 30.741889956955568, 34.982610135697314, "Zoe", 1, 3, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 19, true },
                    { 20, "116 Menachem Begin St, Tiberias", new DateTime(2021, 11, 4, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), "Friendly dog that loves Riley.", false, false, 30.926854770870573, 35.420202352205116, "Honey", 2, 8, new DateTime(2025, 4, 22, 10, 0, 32, 112, DateTimeKind.Utc).AddTicks(8714), 20, true }
                });

            migrationBuilder.InsertData(
                table: "DogParks",
                columns: new[] { "DogLikesId", "FavParksId" },
                values: new object[,]
                {
                    { 1, 14 },
                    { 1, 22 },
                    { 2, 3 },
                    { 2, 7 },
                    { 2, 14 },
                    { 2, 17 },
                    { 3, 2 },
                    { 3, 4 },
                    { 3, 11 },
                    { 3, 21 },
                    { 4, 1 },
                    { 4, 11 },
                    { 4, 13 },
                    { 4, 18 },
                    { 4, 20 },
                    { 5, 17 },
                    { 5, 22 },
                    { 5, 24 },
                    { 6, 2 },
                    { 6, 7 },
                    { 6, 20 },
                    { 6, 21 },
                    { 6, 23 },
                    { 7, 3 },
                    { 7, 4 },
                    { 7, 19 },
                    { 7, 22 },
                    { 8, 15 },
                    { 8, 16 },
                    { 8, 17 },
                    { 8, 22 },
                    { 8, 24 },
                    { 9, 1 },
                    { 9, 18 },
                    { 9, 22 },
                    { 10, 9 },
                    { 10, 10 },
                    { 10, 16 },
                    { 10, 19 },
                    { 11, 1 },
                    { 11, 24 },
                    { 12, 9 },
                    { 12, 13 },
                    { 13, 9 },
                    { 13, 23 },
                    { 14, 6 },
                    { 14, 11 },
                    { 14, 17 },
                    { 14, 18 },
                    { 14, 24 },
                    { 15, 2 },
                    { 15, 4 },
                    { 15, 11 },
                    { 15, 15 },
                    { 16, 4 },
                    { 16, 7 },
                    { 16, 9 },
                    { 16, 13 },
                    { 16, 24 },
                    { 17, 15 },
                    { 17, 16 },
                    { 17, 22 },
                    { 18, 4 },
                    { 18, 12 },
                    { 18, 13 },
                    { 18, 19 },
                    { 18, 24 },
                    { 19, 1 },
                    { 19, 3 },
                    { 19, 7 },
                    { 19, 12 },
                    { 19, 15 },
                    { 20, 2 },
                    { 20, 18 }
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
