using System.Threading.Tasks;
using vega.Core;
using vega.Core.Repositories;
using vega.Persistence.Repositories;

namespace vega.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext context;
        public IFeatureRepository Features { get; private set; }
        public IMakeRepository Makes { get; private set; }
        public IVehicleRepository Vehicles { get; private set; }

        public UnitOfWork(VegaDbContext context)
        {
            this.context = context;
            Features = new FeatureRepository(this.context);
            Makes = new MakeRepository(this.context);
            Vehicles = new VehicleRepository(this.context);
        }


        public async Task CompleteAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}