namespace TailBuddys.Hubs.HubInterfaces
{
    public interface IDogConnectionTracker
    {
        public void JoinDogMatchGroup(int dogId);
        public void LeaveDogMatchGroup(int dogId);
        public bool IsDogInMatchGroup(int dogId);

        public void JoinDogChatsGroup(int dogId);
        public void LeaveDogChatsGroup(int dogId);
        public bool IsDogInChatsGroup(int dogId);

        public void JoinChat(int dogId, int chatId);
        public void LeaveChat(int dogId, int chatId);
        public bool IsDogInSpecificChat(int dogId, int chatId);
    }
}
