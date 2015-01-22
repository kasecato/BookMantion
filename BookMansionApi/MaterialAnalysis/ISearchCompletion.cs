using BookMansionApi.Model.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMansionApi.MaterialAnalysis
{
    interface ISearchCompletion
    {
        Task<IList<BookMansionCompletion>> Search(string keyword);
    }
}
