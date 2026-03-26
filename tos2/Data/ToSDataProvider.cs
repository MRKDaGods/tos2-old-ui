using Services;

namespace MRK.Data
{
    public class ToSDataProvider : IDataProvider
    {
        private ToSUser _user;

        public IUser User => _user ??= new ToSUser();
    }

    public class ToSUser : IUser
    {
        public string Username => Service.Home.UserService.UserInfo.AccountName;
        public int TownPoints => Service.Home.UserService.UserInfo.TownPoints;
        public int CauldronTimeLeft => Service.Home.Cauldron.cauldronTimeRemainingServer;
    }
}
