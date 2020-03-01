using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core.Repositories
{
    public interface IFeatureRepository
    {
        Task<IEnumerable<Feature>> GetFeatures();
    }
}