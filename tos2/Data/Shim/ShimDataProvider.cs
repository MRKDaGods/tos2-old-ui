using System.Collections.Generic;

namespace MRK.Data.Shim
{
    public class ShimDataProvider : IDataProvider
    {
        private User? _user;

        public IUser User => _user ??= new User();

        public List<Friend> Friends =>
            new List<Friend>
            {
                new Friend(FriendStatus.Online, "ammar123"),
                new Friend(FriendStatus.InLobby, "kuttle"),
                new Friend(FriendStatus.ActiveGame, "xxxa123"),
                new Friend(FriendStatus.Offline, "lace"),
                new Friend(FriendStatus.Offline, "exsetty"),
                new Friend(FriendStatus.Offline, "sweeper"),
            };
    }
}
