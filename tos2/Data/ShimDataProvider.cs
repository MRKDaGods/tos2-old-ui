namespace MRK.Data
{
    public class ShimDataProvider : IDataProvider
    {
        private ShimUser? _user;

        public IUser User => _user ??= new ShimUser();
    }

    public class ShimUser : IUser
    {
        public string Username => "MRKDaGods";
        public int TownPoints => 9923467;
        public int CauldronTimeLeft => 123;
    }
}
