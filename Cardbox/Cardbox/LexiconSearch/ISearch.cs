using System.Collections.Generic;

namespace Cardbox.LexiconSearch
{
    public interface ISearch
    {
        IList<string> Query(string searchTerm);
    }
}
