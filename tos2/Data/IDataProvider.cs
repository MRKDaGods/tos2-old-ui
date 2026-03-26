using System.Collections.Generic;

namespace MRK.Data
{
    public interface IDataProvider
    {
        IUser User { get; }
        List<Friend> Friends { get; }
    }
}
