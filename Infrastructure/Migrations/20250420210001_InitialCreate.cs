using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TailBuddys.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    { 1, "159 Arlozorov St, Tiberias", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.932322195059527, 34.916609376410968, "Gan Meir", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 2, "72 Allenby St, Rehovot", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.507919494728601, 34.833515086455606, "Independence Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 3, "175 Yitzhak Rabin St, Hadera", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.272371856350432, 35.204065234472438, "Yarkon Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 4, "187 Weizmann St, Herzliya", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 32.024759260047205, 35.253395568849179, "Sacher Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 5, "157 Arlozorov St, Bat Yam", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.44664646109781, 35.289702085201149, "Ramat Gan National Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 6, "85 Dizengoff St, Holon", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.98934153777936, 35.144770000185744, "HaPisga Garden", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 7, "147 Ben Gurion St, Petah Tikva", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.874967783385415, 35.13429068828674, "Canada Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 8, "17 Yitzhak Rabin St, Ramat Gan", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 32.8352893931783, 34.975209784302109, "Ashdod Yam Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 9, "172 Ben Gurion St, Modiin", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.289338568188249, 34.953429313160918, "Gan Ha'ir", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 10, "157 King David St, Beer Sheva", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.638309978820317, 35.042803136769834, "Armon Hanatziv Promenade", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 11, "102 Dizengoff St, Ashkelon", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.71993551127747, 35.11255321228267, "Liberty Bell Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 12, "144 Menachem Begin St, Tel Aviv", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.665500001487391, 35.244067453024584, "Lachish Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 13, "34 HaShalom St, Haifa", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 32.120820094775631, 35.115563847510714, "Ariel Sharon Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 14, "194 Hagana St, Hadera", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.803551327917347, 35.379364714028313, "Park Giv'atayim", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 15, "182 Herzl St, Beer Sheva", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.872133215672076, 34.893645083143596, "Herzliya Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 16, "195 HaShalom St, Kiryat Gat", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.615266362705807, 35.06257633500924, "Park Ashkelon", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 17, "199 Aharonovitch St, Nazareth", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.101123361846554, 34.849212007757842, "Beit She'an Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 18, "45 Herzl St, Tiberias", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.890672987049868, 34.882431927506886, "Ein Gedi Reserve", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 19, "162 Menachem Begin St, Jerusalem", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.28720398697163, 35.168115053048986, "Neot Kedumim", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 20, "128 Weizmann St, Rehovot", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 32.869487722748907, 34.927139801670656, "Ma'ayan Harod", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 21, "45 Herzl St, Tiberias", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.510898154436077, 35.311001392921952, "Timna Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 22, "199 Aharonovitch St, Nazareth", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 31.972501396363768, 35.332655801144597, "Alona Park", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 23, "143 King David St, Netanya", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.733553750808422, 35.039765203149045, "Park Kiryat Motzkin", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 24, "162 Menachem Begin St, Jerusalem", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Beautiful park for dogs. Great for playing and resting.", 30.881452108674239, 35.273713371165115, "Bat Galim Promenade", new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "Email", "FirstName", "Gender", "GoogleId", "IsAdmin", "LastName", "PasswordHash", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 5, 25, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7989), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user1@tail.com", "User1", 1, null, false, "Last1", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 2, new DateTime(1976, 12, 3, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8002), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user2@tail.com", "User2", 0, null, false, "Last2", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 3, new DateTime(2001, 1, 2, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8006), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user3@tail.com", "User3", 1, null, false, "Last3", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 4, new DateTime(1997, 5, 25, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8008), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user4@tail.com", "User4", 2, null, false, "Last4", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 5, new DateTime(1999, 12, 1, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8011), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user5@tail.com", "User5", 2, null, false, "Last5", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 6, new DateTime(2005, 5, 4, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8014), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user6@tail.com", "User6", 0, null, false, "Last6", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 7, new DateTime(1991, 5, 8, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8016), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user7@tail.com", "User7", 0, null, false, "Last7", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 8, new DateTime(1991, 11, 9, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8061), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user8@tail.com", "User8", 2, null, false, "Last8", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 9, new DateTime(1981, 11, 30, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8064), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user9@tail.com", "User9", 0, null, false, "Last9", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 10, new DateTime(1986, 10, 24, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8067), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user10@tail.com", "User10", 1, null, false, "Last10", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 11, new DateTime(2006, 11, 29, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8070), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user11@tail.com", "User11", 0, null, false, "Last11", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 12, new DateTime(1985, 11, 14, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8072), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user12@tail.com", "User12", 2, null, false, "Last12", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 13, new DateTime(1999, 4, 30, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8075), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user13@tail.com", "User13", 0, null, false, "Last13", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 14, new DateTime(1988, 9, 3, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8077), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user14@tail.com", "User14", 0, null, false, "Last14", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 15, new DateTime(1996, 5, 23, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8080), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user15@tail.com", "User15", 0, null, false, "Last15", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 16, new DateTime(1988, 11, 10, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8083), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user16@tail.com", "User16", 2, null, false, "Last16", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 17, new DateTime(1989, 5, 18, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8085), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user17@tail.com", "User17", 2, null, false, "Last17", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 18, new DateTime(1982, 9, 19, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8088), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user18@tail.com", "User18", 1, null, false, "Last18", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 19, new DateTime(1993, 5, 5, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8091), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user19@tail.com", "User19", 1, null, false, "Last19", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) },
                    { 20, new DateTime(1987, 3, 18, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(8093), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "user20@tail.com", "User20", 2, null, false, "Last20", null, null, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549) }
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "Description", "Gender", "IsBot", "Lat", "Lon", "Name", "Size", "Type", "UpdatedAt", "UserId", "Vaccinated" },
                values: new object[,]
                {
                    { 1, "96 Weizmann St, Tel Aviv", new DateTime(2018, 6, 8, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Bailey.", false, false, 31.566440371077096, 35.225679022790075, "Luna", 2, 1, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 1, true },
                    { 2, "71 Weizmann St, Herzliya", new DateTime(2021, 5, 5, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Bear.", false, false, 31.659962975647012, 35.371721538103081, "Rocky", 0, 0, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 2, true },
                    { 3, "21 Jabotinsky St, Netanya", new DateTime(2023, 12, 6, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Simba.", false, false, 30.422384858684513, 35.209225918098838, "Stella", 1, 3, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 3, true },
                    { 4, "28 HaShalom St, Tiberias", new DateTime(2019, 11, 19, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Max.", true, false, 30.632695831361914, 35.062451626503737, "Ace", 1, 1, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 4, true },
                    { 5, "79 Dizengoff St, Kiryat Gat", new DateTime(2019, 1, 7, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Riley.", false, false, 31.961314699576153, 34.925050460820962, "Leo", 2, 8, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 5, true },
                    { 6, "71 Weizmann St, Herzliya", new DateTime(2016, 1, 30, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Sky.", false, false, 31.722912895466884, 35.409144961580985, "Simba", 1, 6, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 6, true },
                    { 7, "98 Ben Gurion St, Tel Aviv", new DateTime(2022, 4, 26, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Lola.", true, false, 31.109416453022387, 35.18396518423566, "Maggie", 2, 6, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 7, true },
                    { 8, "68 Arlozorov St, Rehovot", new DateTime(2016, 2, 2, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Ace.", true, false, 31.519948026808919, 34.88347919105874, "Milo", 1, 4, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 8, true },
                    { 9, "142 Hagana St, Modiin", new DateTime(2024, 9, 19, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Buddy.", true, false, 30.387641135306101, 34.956993994464618, "Rex", 2, 7, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 9, true },
                    { 10, "199 Herzl St, Jerusalem", new DateTime(2016, 1, 9, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Remy.", true, false, 31.858240736569762, 35.335335086066394, "Zoe", 2, 0, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 10, true },
                    { 11, "24 King David St, Ashdod", new DateTime(2017, 5, 25, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Sam.", true, false, 30.8932429816882, 35.430190032090458, "Zeus", 2, 4, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 11, true },
                    { 12, "26 Menachem Begin St, Beer Sheva", new DateTime(2021, 9, 30, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Olive.", false, true, 30.009125122613781, 34.918384120996592, "Abby", 1, 9, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 12, true },
                    { 13, "189 King David St, Jerusalem", new DateTime(2015, 9, 8, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Bella.", false, false, 31.1314256199373, 35.331789054662913, "Lola", 1, 5, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 13, true },
                    { 14, "151 Menachem Begin St, Modiin", new DateTime(2024, 2, 3, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Hazel.", true, false, 32.69701346257051, 35.09081227731204, "Otis", 0, 2, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 14, true },
                    { 15, "91 Allenby St, Bat Yam", new DateTime(2018, 9, 23, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Duke.", true, false, 31.293367298761193, 35.211767744618413, "Otis", 1, 3, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 15, false },
                    { 16, "41 HaPalmach St, Petah Tikva", new DateTime(2018, 8, 16, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Chloe.", true, true, 32.406461550472223, 35.198519788357629, "Bella", 0, 7, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 16, true },
                    { 17, "46 HaShalom St, Tel Aviv", new DateTime(2016, 10, 14, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Olive.", false, false, 33.083972273857192, 35.080671430239732, "Ace", 0, 3, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 17, true },
                    { 18, "11 Allenby St, Holon", new DateTime(2019, 5, 2, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Oscar.", true, false, 30.888385801696185, 34.805754519785623, "Maple", 0, 3, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 18, true },
                    { 19, "159 Jabotinsky St, Bat Yam", new DateTime(2016, 10, 13, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Bella.", true, false, 30.193907286324542, 34.803433222074133, "Finn", 0, 0, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 19, true },
                    { 20, "37 Aharonovitch St, Tiberias", new DateTime(2024, 7, 16, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), "Friendly dog that loves Rex.", false, false, 32.171510688353465, 35.134505482303204, "Charlie", 0, 5, new DateTime(2025, 4, 20, 21, 0, 1, 312, DateTimeKind.Utc).AddTicks(7549), 20, true }
                });

            migrationBuilder.InsertData(
                table: "DogParks",
                columns: new[] { "DogLikesId", "FavParksId" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 1, 17 },
                    { 2, 1 },
                    { 2, 5 },
                    { 2, 8 },
                    { 3, 3 },
                    { 3, 4 },
                    { 3, 8 },
                    { 3, 19 },
                    { 3, 23 },
                    { 4, 4 },
                    { 4, 12 },
                    { 4, 24 },
                    { 5, 5 },
                    { 5, 12 },
                    { 6, 7 },
                    { 6, 11 },
                    { 6, 12 },
                    { 6, 15 },
                    { 6, 16 },
                    { 7, 11 },
                    { 7, 20 },
                    { 8, 4 },
                    { 8, 11 },
                    { 8, 15 },
                    { 8, 20 },
                    { 9, 3 },
                    { 9, 7 },
                    { 9, 8 },
                    { 9, 14 },
                    { 10, 1 },
                    { 10, 9 },
                    { 10, 20 },
                    { 11, 7 },
                    { 11, 18 },
                    { 11, 21 },
                    { 12, 5 },
                    { 12, 6 },
                    { 12, 8 },
                    { 12, 9 },
                    { 13, 6 },
                    { 13, 8 },
                    { 13, 10 },
                    { 13, 16 },
                    { 13, 23 },
                    { 14, 3 },
                    { 14, 11 },
                    { 14, 13 },
                    { 14, 16 },
                    { 15, 1 },
                    { 15, 3 },
                    { 15, 19 },
                    { 16, 10 },
                    { 16, 15 },
                    { 16, 17 },
                    { 16, 18 },
                    { 17, 1 },
                    { 17, 11 },
                    { 17, 15 },
                    { 17, 18 },
                    { 18, 1 },
                    { 18, 2 },
                    { 18, 3 },
                    { 18, 5 },
                    { 18, 16 },
                    { 19, 4 },
                    { 19, 5 },
                    { 19, 14 },
                    { 19, 23 },
                    { 19, 24 },
                    { 20, 4 },
                    { 20, 6 },
                    { 20, 9 },
                    { 20, 17 }
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
