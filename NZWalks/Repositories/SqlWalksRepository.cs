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
        public async Task<List<Walk>> Get(string? filterOn =null,string? filterQuery=null,
            string? sortBy=null,bool? isAscending=null,int? pageNumber =null,int? pageSize=null)
        {
            var walks =  dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering Data
            if (string.IsNullOrWhiteSpace(filterOn) == false || string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    return await walks.Where(x => x.Name.Contains(filterQuery)).ToListAsync();
                }
                else if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    return await walks.Where(x => x.Description.Contains(filterQuery)).ToListAsync();
                }
                else if (filterOn.Equals("Region", StringComparison.OrdinalIgnoreCase))
                {
                    return await walks.Where(x => x.Region.Name.Contains(filterQuery)).ToListAsync();
                }
                else if (filterOn.Equals("Difficulty", StringComparison.OrdinalIgnoreCase))
                {
                    return await walks.Where(x => x.Difficulty.Name.Contains(filterQuery)).ToListAsync();
                }
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true ? walks.OrderBy(x => x.Length) : walks.OrderByDescending(x => x.Length);
                }
                else if (sortBy.Equals("Difficulty", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true ? walks.OrderBy(x => x.Difficulty.Name) : walks.OrderByDescending(x => x.Difficulty.Name);
                }
                else if (sortBy.Equals("Region", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true ? walks.OrderBy(x => x.Region.Name) : walks.OrderByDescending(x => x.Region.Name);

                }
            }

            //pagination
            if(pageNumber != null && pageSize != null)
            {
                int skipResults = ((int)pageNumber - 1) * (int)pageSize;
                walks = walks.Skip(skipResults).Take((int)pageSize);
            }
            //List<Walk> walks = await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

            return await walks.ToListAsync();
        }

        public async Task<Walk> Create(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }
    }
}
