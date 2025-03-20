using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;

namespace TailBuddys.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly HashSet<int> _activeDogs; // Injected singleton
        private readonly IDogRepository _dogRepository;
        private readonly INotificationService _notificationService;
        private readonly IAuth _jwtAuthService;

        public NotificationHub(
            IDogRepository dogRepository,
            INotificationService notificationService,
            HashSet<int> activeDogs,
            IAuth jwtAuthService)
        {
            _dogRepository = dogRepository;
            _notificationService = notificationService;
            _activeDogs = activeDogs;
            _jwtAuthService = jwtAuthService;
            Console.WriteLine("SHASHASHASHASHASHASHASHASHASHASHASHASHASHASHASHASHASHASHASHA");
        }

        public async Task<bool> JoinDogGroup(int dogId)
        {
            Console.WriteLine("LALALALALALALALALALALALALALA" + dogId);
            int userId = GetUserIdFromToken();
            Console.WriteLine(userId);
            if (userId == 0) return false;

            var dogs = await _dogRepository.GetAllUserDogsDb(userId);
            if (!dogs.Any(d => d.Id == dogId))
            {
                Console.WriteLine("not good ");
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return false;
            }

            Console.WriteLine("ppppppppppppppppppppppppppppppp");
            await Groups.AddToGroupAsync(Context.ConnectionId, dogId.ToString());
            lock (_activeDogs)
            {
                _activeDogs.Add(dogId);
            }

            // Fetch all match notifications for the dog
            var matchNotifications = await _notificationService.GetDogAllMatchesNotifications(dogId);
            Console.WriteLine("cckckckck");
            // Send each match notification to the frontend
            foreach (var matchNotification in matchNotifications)
            {
                Console.WriteLine(matchNotification.MatchId.ToString());
                await Clients.Caller.SendAsync("ReceiveNewMatch", matchNotification.MatchId);
            }

            return true;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            int userId = GetUserIdFromToken();
            var dogs = await _dogRepository.GetAllUserDogsDb(userId);
            foreach (var dog in dogs)
            {
                lock (_activeDogs)
                {
                    _activeDogs.Remove(dog.Id);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public bool IsDogActive(int dogId)
        {
            lock (_activeDogs)
            {
                return _activeDogs.Contains(dogId);
            }
        }

        public int GetUserIdFromToken()
        {
            Console.WriteLine("lalalalalal~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            var httpContext = Context.GetHttpContext();
            var usertoken = httpContext?.Request.Query["access_token"].ToString();
            Console.WriteLine(usertoken + "+emjfdcmkdvvjdvv+");
            if (string.IsNullOrEmpty(usertoken) || !usertoken.StartsWith("Bearer ")) return 0;
            Console.WriteLine("na na na _______");
            var token = usertoken.Substring(7);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            Console.WriteLine("token!!!!!!!" + jwtToken.ToString());
            return int.Parse(jwtToken?.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
        }


        //--------------------------------------------------------------------------//
        //private static readonly Dictionary<string, string> ActiveChats = new();
        //private static readonly HashSet<string> ActiveMatchesTabs = new();
        //private readonly IDogRepository _dogRepository;
        //private readonly INotificationService _notificationService;

        //public NotificationHub(IDogRepository dogRepository, INotificationService notificationService)
        //{
        //    _dogRepository = dogRepository;
        //    _notificationService = notificationService;
        //}
        //public async Task<bool> JoinDogGroup(int dogId)
        //{
        //    int userId = GetUserIdFromToken();
        //    if (userId == 0) return false;

        //    var dogs = await _dogRepository.GetAllUserDogsDb(userId);
        //    if (!dogs.Any(d => d.Id == dogId))
        //    {
        //        await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
        //        return false;
        //    }

        //    await Groups.AddToGroupAsync(Context.ConnectionId, dogId.ToString());
        //    return true;
        //}

        //private int GetUserIdFromToken() // קיימת לנו כבר פונקציה כזו צריך לראות אולי פשוט לייבא אותה לפה
        //{
        //    var httpContext = Context.GetHttpContext();
        //    var authorizationHeader = httpContext?.Request.Headers["Authorization"].ToString();
        //    if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ")) return 0;

        //    var token = authorizationHeader.Substring(7);
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        //    return int.Parse(jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
        //}

        //public void SetActiveChat(string dogId, string chatId) => ActiveChats[dogId] = chatId;
        //public void RemoveActiveChat(string dogId) => ActiveChats.Remove(dogId);
        //public void SetActiveMatchesTab(string dogId) => ActiveMatchesTabs.Add(dogId);
        //public void RemoveActiveMatchesTab(string dogId) => ActiveMatchesTabs.Remove(dogId);
        //public bool IsChatOpen(string chatId) => ActiveChats.Values.Contains(chatId);
        //public bool IsMatchesTabOpen() => ActiveMatchesTabs.Contains(Context.ConnectionId);

        //public async Task SendMessage(int chatId, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", chatId, message);
        //}

        //public async Task MarkChatAsRead(int dogId, int chatId)
        //{
        //    await _notificationService.MarkChatAsRead(dogId, chatId);
        //    await Clients.Group(dogId.ToString()).SendAsync("NotificationRead", chatId);
        //}

        //public async Task MarkMatchesAsRead(int dogId)
        //{
        //    await _notificationService.MarkMatchesAsRead(dogId);
        //    await Clients.Group(dogId.ToString()).SendAsync("MatchesNotificationRead");
        //}
    }
}
