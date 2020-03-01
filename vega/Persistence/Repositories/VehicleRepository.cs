using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Core.Repositories;
using vega.Extensions;

namespace vega.Persistence.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        public VegaDbContext Context;

        public VehicleRepository(VegaDbContext context)
        {
            Context = context;
        }

        public void Add(Vehicle vehicle)
        {
            // throw new System.Exception("Error");
            Context.Vehicles.Add(vehicle);
        }

        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await Context.Vehicles.FindAsync(id);

            return await Context.Vehicles
                        .Include(v => v.Features)
                        .ThenInclude(vf => vf.Feature)
                        .Include(v => v.Model)
                        .ThenInclude(m => m.Make)
                        .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObj)
        {
            var result = new QueryResult<Vehicle>();
            var query = Context.Vehicles
            .Include(v => v.Model)
                .ThenInclude(m => m.Make)
            .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
            // .ToListAsync()
            .AsQueryable();

            if (queryObj.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == queryObj.MakeId);

            if (queryObj.ModelId.HasValue)
                query = query.Where(v => v.Model.Id == queryObj.ModelId);

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["model"] = v => v.Model.Name,
                ["make"] = v => v.Model.Make.Name,
                ["contactName"] = v => v.ContactName
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = await query.CountAsync();
            query = query.ApplyPaging(queryObj);

            result.Items = await query.ToListAsync();

            return result;
        }



        public void Remove(Vehicle vehicle)
        {
            Context.Vehicles.Remove(vehicle);
        }

    }
}