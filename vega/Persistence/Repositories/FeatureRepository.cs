using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Core.Repositories;

namespace vega.Persistence.Repositories
{
    public class FeatureRepository : IFeatureRepository
    {
        public VegaDbContext Context;

        public FeatureRepository(VegaDbContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<Feature>> GetFeatures()
        {
            return await Context.Features.ToListAsync();
        }

    }
}