using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngineResultsLookup.Services
{
    public interface ISearchEngineService
    {
        Task<IEnumerable<int>> FindTheUrlOccurenceForKeyWordSearch(string keyword, string url, string provider);
    }
}
