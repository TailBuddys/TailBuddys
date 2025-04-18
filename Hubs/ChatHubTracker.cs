namespace TailBuddys.Hubs
{
    public class ChatHubTracker
    {
        private static readonly Dictionary<int, HashSet<int>> ActiveChats = new();

        public static void JoinChat(int dogId, int chatId)
        {
            lock (ActiveChats)
            {
                if (!ActiveChats.ContainsKey(chatId))
                    ActiveChats[chatId] = new HashSet<int>();

                ActiveChats[chatId].Add(dogId);
            }
        }

        public static void LeaveChat(int dogId, int chatId)
        {
            lock (ActiveChats)
            {
                if (ActiveChats.ContainsKey(chatId))
                {
                    ActiveChats[chatId].Remove(dogId);
                    if (!ActiveChats[chatId].Any())
                        ActiveChats.Remove(chatId);
                }
            }
        }

        public static bool IsDogInChat(int dogId, int chatId)
        {
            lock (ActiveChats)
            {
                return ActiveChats.ContainsKey(chatId) && ActiveChats[chatId].Contains(dogId);
            }
        }
    }
}
