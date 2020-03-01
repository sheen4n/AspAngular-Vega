using System.Threading.Tasks;
using vega.Core.Repositories;

namespace vega.Core
{
    public interface IUnitOfWork
    {
        IFeatureRepository Features { get; }
        IMakeRepository Makes { get; }
        IVehicleRepository Vehicles { get; }
        Task CompleteAsync();
    }
}