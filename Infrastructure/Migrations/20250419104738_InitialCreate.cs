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
                    { 1, "Address 1", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Built for exploration, with narrow trails, wide fields, and a loop that circles a peaceful lake.", 32.490521069186201, 34.870816395912733, "Park #1", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 2, "Address 2", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Safe for evening visits, with lampposts, security cameras, and a warm atmosphere after sundown.", 32.124683326045975, 35.520953632340976, "Park #2", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 3, "Address 3", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "A hidden gem filled with colorful flowers, peaceful benches, and hidden nooks for relaxed sniffing.", 32.386325243953721, 34.93305667314435, "Park #3", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 4, "Address 4", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "A hidden gem filled with colorful flowers, peaceful benches, and hidden nooks for relaxed sniffing.", 32.480443042279148, 35.393317979430734, "Park #4", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 5, "Address 5", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Hosts weekend adoption drives, charity walks, and social events that welcome all dog breeds.", 32.232220082147563, 35.279001648891878, "Park #5", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 6, "Address 6", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Popular with locals, features dog fountains, agility equipment, and benches for owners to relax and socialize.", 32.617638986156862, 34.921613039166168, "Park #6", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 7, "Address 7", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Special events like dog birthday parties and costume contests add excitement to regular visits.", 32.424905766954893, 35.66837119195857, "Park #7", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 8, "Address 8", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "A hidden gem filled with colorful flowers, peaceful benches, and hidden nooks for relaxed sniffing.", 32.384951147513391, 35.574062575413357, "Park #8", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 9, "Address 9", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Daily dog walkers make this their go-to, citing trust, community, and happy tail wags everywhere.", 32.702707285401992, 35.064238757082123, "Park #9", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 10, "Address 10", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Peaceful urban retreat with grassy fields, paved walkways, and plenty of space for off-leash fun.", 32.583137042808204, 34.931226095371045, "Park #10", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 11, "Address 11", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Has dog-friendly signage, snack vendors, and emergency kits for peace of mind while you play.", 32.757612137771872, 34.869402946809686, "Park #11", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 12, "Address 12", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Pet-friendly all the way, with friendly signage, community boards, and waste bags always available.", 32.941623074352485, 34.803622794008355, "Park #12", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 13, "Address 13", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Pet-friendly all the way, with friendly signage, community boards, and waste bags always available.", 32.614098504672874, 34.736842981174313, "Park #13", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 14, "Address 14", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Feels like a slice of wilderness in the middle of town, perfect for dogs that love to roam.", 32.201226385580313, 34.990493993263996, "Park #14", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 15, "Address 15", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Complete with maps, fenced paths, and signs to keep dogs safe while off-leash.", 32.922005295802613, 34.788483912320935, "Park #15", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 16, "Address 16", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Popular with locals, features dog fountains, agility equipment, and benches for owners to relax and socialize.", 32.145952366135568, 34.747132890800735, "Park #16", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 17, "Address 17", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Built for exploration, with narrow trails, wide fields, and a loop that circles a peaceful lake.", 32.933309304235934, 35.036720076231646, "Park #17", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 18, "Address 18", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Pet-friendly all the way, with friendly signage, community boards, and waste bags always available.", 32.019931338648675, 35.208022675634318, "Park #18", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 19, "Address 19", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Has dog-friendly signage, snack vendors, and emergency kits for peace of mind while you play.", 32.504888501764484, 35.035499145835068, "Park #19", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 20, "Address 20", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Special events like dog birthday parties and costume contests add excitement to regular visits.", 32.489296023570482, 34.896047767251972, "Park #20", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 21, "Address 21", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Newly renovated with eco-friendly designs, agility sets, and separate exercise fields for energetic pups.", 32.516592781161229, 35.037167650167689, "Park #21", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 22, "Address 22", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Daily dog walkers make this their go-to, citing trust, community, and happy tail wags everywhere.", 32.935090523588329, 34.828870187942073, "Park #22", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 23, "Address 23", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "One of the few parks with a swimming zone for water-loving dogs to splash safely.", 32.967663781564319, 35.698526543428706, "Park #23", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 24, "Address 24", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Newly renovated with eco-friendly designs, agility sets, and separate exercise fields for energetic pups.", 32.809682946127282, 35.55322156403539, "Park #24", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 25, "Address 25", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Ideal for both training and playtime, with spaces to run freely or work on obedience drills.", 32.599851317038031, 34.967248227829636, "Park #25", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 26, "Address 26", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Daily dog walkers make this their go-to, citing trust, community, and happy tail wags everywhere.", 32.872712814263203, 35.475767865360197, "Park #26", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 27, "Address 27", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Safe for evening visits, with lampposts, security cameras, and a warm atmosphere after sundown.", 32.386744482756782, 35.619415355802595, "Park #27", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 28, "Address 28", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Hosts weekend adoption drives, charity walks, and social events that welcome all dog breeds.", 32.006420868602426, 34.926447651651856, "Park #28", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 29, "Address 29", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Has a calm, welcoming atmosphere with play tunnels, dog ramps, and grassy areas to roll around in.", 32.709286290229734, 35.386803390714498, "Park #29", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 30, "Address 30", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Popular with locals, features dog fountains, agility equipment, and benches for owners to relax and socialize.", 32.21313574545556, 35.522402771992795, "Park #30", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 31, "Address 31", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Feels like a slice of wilderness in the middle of town, perfect for dogs that love to roam.", 32.758296797383167, 34.892024401356593, "Park #31", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 32, "Address 32", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Hosts weekend adoption drives, charity walks, and social events that welcome all dog breeds.", 32.757521760143455, 35.136092479890422, "Park #32", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 33, "Address 33", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Ideal for both training and playtime, with spaces to run freely or work on obedience drills.", 32.140568298206887, 34.846544075374595, "Park #33", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 34, "Address 34", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Feels like a slice of wilderness in the middle of town, perfect for dogs that love to roam.", 32.8223870624809, 34.888492379617446, "Park #34", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 35, "Address 35", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Fenced play zones separate big and small dogs, ensuring a fun and safe experience for all visitors.", 32.639987218066715, 35.319849292861598, "Park #35", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 36, "Address 36", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Has dog-friendly signage, snack vendors, and emergency kits for peace of mind while you play.", 32.655085281461702, 35.29462063547215, "Park #36", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 37, "Address 37", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Built for exploration, with narrow trails, wide fields, and a loop that circles a peaceful lake.", 32.074542842645279, 35.026745749598454, "Park #37", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 38, "Address 38", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Has a calm, welcoming atmosphere with play tunnels, dog ramps, and grassy areas to roll around in.", 32.664561695544165, 35.269681499773313, "Park #38", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 39, "Address 39", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Feels like a slice of wilderness in the middle of town, perfect for dogs that love to roam.", 32.561152088058201, 34.926973662534905, "Park #39", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 40, "Address 40", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "A hidden gem filled with colorful flowers, peaceful benches, and hidden nooks for relaxed sniffing.", 32.529635562039147, 35.061900408668883, "Park #40", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 41, "Address 41", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Pet-friendly all the way, with friendly signage, community boards, and waste bags always available.", 32.424209179337716, 34.897371556188354, "Park #41", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 42, "Address 42", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Fenced play zones separate big and small dogs, ensuring a fun and safe experience for all visitors.", 32.97063821398833, 35.092993720765818, "Park #42", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 43, "Address 43", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Feels like a slice of wilderness in the middle of town, perfect for dogs that love to roam.", 32.440913981366144, 35.18892573784052, "Park #43", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 44, "Address 44", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "One of the few parks with a swimming zone for water-loving dogs to splash safely.", 32.395220312453219, 35.186820663870932, "Park #44", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 45, "Address 45", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Safe for evening visits, with lampposts, security cameras, and a warm atmosphere after sundown.", 32.254856130289618, 35.366459154407551, "Park #45", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 46, "Address 46", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Feels like a slice of wilderness in the middle of town, perfect for dogs that love to roam.", 32.582046396558283, 35.643540997332252, "Park #46", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 47, "Address 47", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Not overly crowded, but always friendly faces and polite, responsible owners respecting park etiquette.", 32.905241070098612, 34.843931695283018, "Park #47", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 48, "Address 48", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Open year-round, provides grassy play zones, snow-safe trails in winter, and covered benches for owners.", 32.101557911428962, 35.544431541701336, "Park #48", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 49, "Address 49", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Has dog-friendly signage, snack vendors, and emergency kits for peace of mind while you play.", 32.777168718675142, 35.690524264823026, "Park #49", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 50, "Address 50", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Popular with locals, features dog fountains, agility equipment, and benches for owners to relax and socialize.", 32.274357736934647, 35.07681120839699, "Park #50", new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "Email", "FirstName", "Gender", "GoogleId", "IsAdmin", "LastName", "PasswordHash", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1992, 1, 1, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9798), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user1@mail.com", "User1", 0, null, false, "Last1", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 2, new DateTime(1983, 1, 15, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9812), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user2@mail.com", "User2", 0, null, false, "Last2", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 3, new DateTime(1972, 9, 17, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9814), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user3@mail.com", "User3", 0, null, false, "Last3", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 4, new DateTime(1988, 2, 10, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9818), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user4@mail.com", "User4", 1, null, false, "Last4", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 5, new DateTime(1962, 3, 16, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9821), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user5@mail.com", "User5", 2, null, false, "Last5", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 6, new DateTime(2007, 1, 15, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9823), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user6@mail.com", "User6", 0, null, false, "Last6", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 7, new DateTime(1958, 9, 20, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9826), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user7@mail.com", "User7", 0, null, false, "Last7", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 8, new DateTime(1960, 12, 9, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9828), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user8@mail.com", "User8", 2, null, false, "Last8", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 9, new DateTime(1988, 8, 4, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9831), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user9@mail.com", "User9", 1, null, false, "Last9", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 10, new DateTime(1984, 5, 29, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9834), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user10@mail.com", "User10", 1, null, false, "Last10", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 11, new DateTime(1988, 4, 25, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9837), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user11@mail.com", "User11", 0, null, false, "Last11", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 12, new DateTime(2003, 11, 13, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9839), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user12@mail.com", "User12", 0, null, false, "Last12", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 13, new DateTime(1974, 3, 17, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9842), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user13@mail.com", "User13", 2, null, false, "Last13", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 14, new DateTime(1961, 10, 26, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9845), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user14@mail.com", "User14", 2, null, false, "Last14", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 15, new DateTime(1996, 10, 27, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9886), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user15@mail.com", "User15", 2, null, false, "Last15", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 16, new DateTime(1960, 9, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9889), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user16@mail.com", "User16", 2, null, false, "Last16", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 17, new DateTime(1991, 3, 15, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9892), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user17@mail.com", "User17", 0, null, false, "Last17", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 18, new DateTime(2006, 6, 1, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9895), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user18@mail.com", "User18", 0, null, false, "Last18", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 19, new DateTime(1979, 12, 20, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9897), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user19@mail.com", "User19", 2, null, false, "Last19", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 20, new DateTime(1982, 8, 29, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9899), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user20@mail.com", "User20", 1, null, false, "Last20", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 21, new DateTime(1972, 4, 27, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9902), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user21@mail.com", "User21", 1, null, false, "Last21", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 22, new DateTime(1967, 2, 16, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9905), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user22@mail.com", "User22", 2, null, false, "Last22", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 23, new DateTime(2003, 6, 29, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9907), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user23@mail.com", "User23", 1, null, false, "Last23", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 24, new DateTime(1980, 11, 9, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9910), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user24@mail.com", "User24", 1, null, false, "Last24", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 25, new DateTime(1980, 12, 7, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9913), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user25@mail.com", "User25", 0, null, false, "Last25", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 26, new DateTime(1983, 9, 6, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9915), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user26@mail.com", "User26", 2, null, false, "Last26", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 27, new DateTime(1987, 1, 21, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9918), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user27@mail.com", "User27", 1, null, false, "Last27", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 28, new DateTime(1985, 3, 4, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9921), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user28@mail.com", "User28", 1, null, false, "Last28", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 29, new DateTime(1968, 3, 14, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9923), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user29@mail.com", "User29", 2, null, false, "Last29", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) },
                    { 30, new DateTime(1986, 7, 4, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9926), new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "user30@mail.com", "User30", 0, null, false, "Last30", null, null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193) }
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "Description", "Gender", "IsBot", "Lat", "Lon", "Name", "Size", "Type", "UpdatedAt", "UserId", "Vaccinated" },
                values: new object[,]
                {
                    { 1, "86 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Mischievous but sweet, has a habit of stealing socks, hiding them under the couch, and acting innocent.", true, null, 32.753432078278571, 35.57355863978232, "Buddy1", 2, 4, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 1, true },
                    { 2, "93 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Easy-going temperament, great with kids and other dogs, loves weekend hikes and swimming in shallow creeks.", false, null, 32.736748790165969, 35.59024690692187, "Buddy2", 2, 0, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 2, true },
                    { 3, "96 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Often caught staring out the window, daydreaming about squirrels and sausages.", true, null, 32.299849529786393, 35.266703608861448, "Charlie3", 1, 5, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 3, true },
                    { 4, "100 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Good with cats, birds, rabbits, and sometimes even turtles. Everyone is a friend.", true, null, 32.592841544981802, 35.209848871864004, "Lola4", 2, 6, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 4, false },
                    { 5, "78 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Terrified of balloons, but otherwise confident, strong, and happy-go-lucky.", false, null, 32.669453407637832, 34.813796834915429, "Rocky5", 0, 8, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 5, true },
                    { 6, "83 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Often caught staring out the window, daydreaming about squirrels and sausages.", false, null, 32.493735253003322, 35.648253007008961, "Max6", 1, 0, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 6, true },
                    { 7, "103 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Has a habit of collecting toys and creating secret stash zones behind furniture.", false, null, 32.822340435679365, 34.983326884500961, "Buddy7", 0, 3, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 7, true },
                    { 8, "33 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Strong and athletic, loves obstacle courses and shows off jumping over small fences like a champ.", true, null, 32.420999749902073, 35.370233086215926, "Bella8", 2, 1, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 8, true },
                    { 9, "32 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Charming and talkative, communicates with happy howls and expressive eyes that melt everyone's heart.", false, null, 32.382020564916253, 35.59820140396662, "Lucy9", 0, 8, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 9, true },
                    { 10, "25 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Energetic and playful pup who loves long walks, belly rubs, squeaky toys, and chasing balls at the park.", true, null, 32.048212693009205, 34.973247384779569, "Molly10", 0, 5, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 10, false },
                    { 11, "71 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Energetic and playful pup who loves long walks, belly rubs, squeaky toys, and chasing balls at the park.", true, null, 32.531858295210775, 34.706147415274188, "Toby11", 1, 7, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 11, true },
                    { 12, "71 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Highly intelligent and curious dog who learns tricks fast and loves solving puzzle toys with treats inside.", false, null, 32.188924999383367, 34.759868471644552, "Lola12", 0, 9, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 12, true },
                    { 13, "88 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Responds best to kindness and cheese, gets nervous at loud sounds but recovers fast.", true, null, 32.370551739597971, 35.647424305829375, "Lucy13", 1, 4, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 13, true },
                    { 14, "61 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Rolls on the floor to get attention and gives high-fives when you ask nicely.", true, null, 32.721307550128607, 34.75036252398499, "Lucy14", 1, 1, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 14, true },
                    { 15, "86 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Gentle and affectionate dog who enjoys quiet evenings, soft beds, and making new furry and human friends.", true, null, 32.281852087464749, 34.913829265057842, "Lucy15", 1, 0, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 15, false },
                    { 16, "29 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Loyal and obedient, follows commands quickly, enjoys running beside a bike and sleeping at your feet.", true, null, 32.556066440798929, 34.918098163513811, "Daisy16", 1, 6, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 16, false },
                    { 17, "76 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Loyal and obedient, follows commands quickly, enjoys running beside a bike and sleeping at your feet.", true, null, 32.056431450914246, 35.517870881758256, "Toby17", 1, 2, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 17, true },
                    { 18, "94 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Charming and talkative, communicates with happy howls and expressive eyes that melt everyone's heart.", false, null, 32.670748154105432, 35.079331219723215, "Lucy18", 1, 2, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 18, true },
                    { 19, "52 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Strong and athletic, loves obstacle courses and shows off jumping over small fences like a champ.", false, null, 32.494679840378147, 35.673464546374611, "Lucy19", 0, 5, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 19, false },
                    { 20, "24 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Perfect family dog, gentle with toddlers, patient with chaos, and full of love for everyone.", false, null, 32.946983264413866, 34.735887414363916, "Daisy20", 2, 5, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 20, true },
                    { 21, "96 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Often caught staring out the window, daydreaming about squirrels and sausages.", true, null, 32.45196552912207, 35.024150881567436, "Bella21", 1, 1, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 21, true },
                    { 22, "82 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Can’t resist mud puddles and playing in the rain, often needs a bath after every adventure.", false, null, 32.713182358659232, 34.865297470455189, "Max22", 1, 8, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 22, true },
                    { 23, "63 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Has a habit of collecting toys and creating secret stash zones behind furniture.", false, null, 32.995501236917484, 35.08293313468392, "Lucy23", 1, 3, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 23, true },
                    { 24, "41 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Very expressive, tilts head in curiosity to every word and barks to join the conversation.", false, null, 32.231711885731492, 34.832537884480395, "Buddy24", 2, 1, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 24, false },
                    { 25, "46 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Sassy and opinionated, growls at vacuum cleaners but happily cuddles after expressing disapproval.", false, null, 32.175613416841443, 35.566021234595077, "Rocky25", 0, 6, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 25, true },
                    { 26, "67 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Responds best to kindness and cheese, gets nervous at loud sounds but recovers fast.", false, null, 32.301048286235826, 35.156906350091482, "Daisy26", 1, 6, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 26, true },
                    { 27, "37 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Wild zoomies in the morning, calm naps in the afternoon, and affectionate kisses at bedtime.", true, null, 32.031558020029983, 35.122227511229568, "Buddy27", 2, 5, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 27, true },
                    { 28, "87 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Great swimmer, dives into lakes and pools like a golden bullet chasing tennis balls.", true, null, 32.864275373001995, 35.424069541183734, "Bella28", 0, 0, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 28, true },
                    { 29, "10 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Enjoys doggy playdates, group walks, and riding in the backseat with ears flapping in the breeze.", true, null, 32.466662699936734, 35.486548651823334, "Toby29", 0, 0, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 29, true },
                    { 30, "108 Bark Street", null, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), "Playful companion with a big heart, enjoys fetch, sunbathing in the yard, and cuddles on the couch.", false, null, 32.909554611655068, 35.4009377186213, "Buddy30", 0, 5, new DateTime(2025, 4, 19, 10, 47, 38, 93, DateTimeKind.Utc).AddTicks(9193), 30, false }
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
