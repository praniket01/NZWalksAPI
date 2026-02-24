using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;
using NZWalks.Data;

namespace NZWalks.Repositories
{
    public class SqlWalksRepository : IWalkRepository
    {
        private readonly NZWalksDBContext dbContext;

        public SqlWalksRepository(NZWalksDBContext dBContext)
        {
            this.dbContext = dBContext;
        }

        public async Task<Walk> Delete(Guid id)
        {
            var existingWalk = dbContext.Walks.FirstOrDefault(x => x.Id == id);
            if (existingWalk == null) return null;
            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk> Update(Guid id, Walk updateWalk)
        {
            Walk currentWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (currentWalk == null) return null;
            //Walk updatedWalk = new Walk();
            currentWalk.Name = updateWalk.Name;
            currentWalk.Description = updateWalk.Description;
            currentWalk.Length = updateWalk.Length;
            currentWalk.RegionId = updateWalk.RegionId;
            currentWalk.DifficultyId = updateWalk.DifficultyId;
            currentWalk.walkImageUrl = updateWalk.walkImageUrl;
            await dbContext.SaveChangesAsync();
            return currentWalk;

        }

        public async Task<Walk> GetByID(Guid id)
        {
            Walk walk = await dbContext.Walks.FindAsync(id);
            return walk;
        }
        public async Task<List<Walk>> Get()
        {
            List<Walk> walks = await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            return walks;
        }

        public async Task<Walk> Create(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }
    }
}
