using System.Collections.Generic;

namespace WordServices
{
    public interface ISearch
    {
        IList<string> Query(string searchTerm);
    }
}
