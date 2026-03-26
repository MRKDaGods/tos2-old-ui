namespace MRK.Data
{
    public interface IDataProvider
    {
        IUser User { get; }
    }

    public interface IUser
    {
        string Username { get; }
        int TownPoints { get; }
        int CauldronTimeLeft { get; }
    }
}
