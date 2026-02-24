using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;

namespace NZWalks.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetRegion();
        Task<Region?> GetRegionByID(Guid id);

        Task<Region> CreateRegion(Region region);


        Task<Region> Update(Guid i,Region region);

        Task<Region> Delete(Guid id);
    }
}
