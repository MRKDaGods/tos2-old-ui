using Services;

namespace MRK.Data.ToS
{
    public class User : IUser
    {
        public string Username => Service.Home.UserService.UserInfo.AccountName;
        public int TownPoints => Service.Home.UserService.UserInfo.TownPoints;
        public int CauldronTimeLeft => Service.Home.Cauldron.cauldronTimeRemainingServer;
    }
}
