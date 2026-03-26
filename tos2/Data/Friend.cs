namespace MRK.Data
{
    public enum FriendStatus
    {
        Offline = 1,
        Online,
        Active,
        AwayFromKeyboard,
        ActiveGame,
        InLobby,
    }

    public class Friend
    {
        public FriendStatus Status { get; }
        public string Name { get; }

        public Friend(FriendStatus status, string name)
        {
            Status = status;
            Name = name;
        }
    }
}
