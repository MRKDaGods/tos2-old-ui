using System.Collections.Generic;
using System.Linq;

namespace MRK.Data.ToS
{
    public class ToSDataProvider : IDataProvider
    {
        private User _user;

        public IUser User => _user ??= new User();
        public List<Friend> Friends => GetFriends();

        private List<Friend> GetFriends()
        {
            return Services
                .Service.Home.Friends.GetFriends()
                .Select(f => new Friend((FriendStatus)(int)f.friendStatus, f.friendName))
                .ToList();
        }
    }
}
