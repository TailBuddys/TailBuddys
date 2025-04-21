using TailBuddys.Hubs.HubInterfaces;

public class DogConnectionTracker : IDogConnectionTracker
{
    private readonly HashSet<int> _activeDogs = new();
    private readonly HashSet<int> _dogChatsGroup = new();
    private readonly Dictionary<int, HashSet<int>> _chatParticipants = new();

    private readonly object _lock = new();

    public void JoinDogMatchGroup(int dogId)
    {
        lock (_lock) { _activeDogs.Add(dogId); }
    }

    public void LeaveDogMatchGroup(int dogId)
    {
        lock (_lock) { _activeDogs.Remove(dogId); }
    }

    public bool IsDogInMatchGroup(int dogId)
    {
        lock (_lock) { return _activeDogs.Contains(dogId); }
    }

    public void JoinDogChatsGroup(int dogId)
    {
        lock (_lock) { _dogChatsGroup.Add(dogId); }
    }

    public void LeaveDogChatsGroup(int dogId)
    {
        lock (_lock) { _dogChatsGroup.Remove(dogId); }
    }

    public bool IsDogInChatsGroup(int dogId)
    {
        lock (_lock) { return _dogChatsGroup.Contains(dogId); }
    }

    public void JoinChat(int dogId, int chatId)
    {
        lock (_lock)
        {
            if (!_chatParticipants.ContainsKey(chatId))
                _chatParticipants[chatId] = new HashSet<int>();

            _chatParticipants[chatId].Add(dogId);
        }
    }

    public void LeaveChat(int dogId, int chatId)
    {
        lock (_lock)
        {
            if (_chatParticipants.ContainsKey(chatId))
            {
                _chatParticipants[chatId].Remove(dogId);
                if (_chatParticipants[chatId].Count == 0)
                    _chatParticipants.Remove(chatId);
            }
        }
    }

    public bool IsDogInSpecificChat(int dogId, int chatId)
    {
        lock (_lock)
        {
            return _chatParticipants.ContainsKey(chatId) &&
                   _chatParticipants[chatId].Contains(dogId);
        }
    }
    public IEnumerable<int> GetAllChatsForDog(int dogId)
    {
        lock (_lock)
        {
            return _chatParticipants
                .Where(kvp => kvp.Value.Contains(dogId))
                .Select(kvp => kvp.Key)
                .ToList(); // copy to avoid collection modification issues
        }
    }
}