namespace MRK.Data
{
    public interface IUser
    {
        string Username { get; }
        int TownPoints { get; }
        int CauldronTimeLeft { get; }
    }
}
