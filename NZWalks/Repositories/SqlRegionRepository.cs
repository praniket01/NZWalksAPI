using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;
using NZWalks.Data;

namespace NZWalks.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext dbContext;
        public SqlRegionRepository(NZWalksDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Region>> GetRegion()
        {
            var regions = await dbContext.Regions.ToListAsync();
            if (regions == null)
                return null;
            return regions;
        }

        public async Task<Region> GetRegionByID(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateRegion(Region region)
        {
           
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }
        public async Task<Region> Update(Guid id,Region region)
        {
            var existing = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return null;
            existing.Code = region.Code;
            existing.Name = region.Name;
            existing.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<Region> Delete(Guid id)
        {
            var existing = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return null;
            dbContext.Regions.Remove(existing);
            await dbContext.SaveChangesAsync();
            return existing;
        }
    }
}
