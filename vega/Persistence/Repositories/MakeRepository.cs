using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Core.Repositories;

namespace vega.Persistence.Repositories
{
    public class MakeRepository : IMakeRepository
    {
        public VegaDbContext Context;

        public MakeRepository(VegaDbContext context)
        {
            this.Context = context;
        }

        public async Task<IEnumerable<Make>> GetMakes()
        {
            return await Context.Makes.Include(m => m.Models).ToListAsync();
        }

    }
}