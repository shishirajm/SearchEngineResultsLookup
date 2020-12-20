using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngineResultsLookup.Services
{
    public interface ISearchEngineService
    {
        Task<IEnumerable<int>> FindTheUrlOccurenceForKeyWordSearch(string keyWords, string url, string provider);

        Task<string> Temp(int i, string keyWords, string provider);
    }
}
