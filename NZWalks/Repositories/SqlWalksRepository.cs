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

        public async Task<IActionResult> SeedWalkDescriptions()
        {


            var walks = await dbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.Difficulty)
                .ToListAsync();

            var random = new Random();

            var intros = new[]
            {
        "is a stunning trail",
        "offers an unforgettable hiking experience",
        "is a must-visit for outdoor enthusiasts",
        "is one of the most scenic routes",
        "provides a refreshing escape into nature"
    };

            var scenery = new[]
            {
        "lush forests, rolling hills, and breathtaking viewpoints",
        "serene landscapes with diverse natural beauty",
        "panoramic views and tranquil surroundings",
        "a mix of rugged terrain and peaceful greenery",
        "spectacular scenery unique to New Zealand"
    };

            var experience = new[]
            {
        "making it ideal for both beginners and experienced hikers",
        "perfect for those looking for a relaxed yet rewarding walk",
        "offering a balance of challenge and enjoyment",
        "providing a memorable outdoor adventure",
        "making it a favorite among nature lovers"
    };

            foreach (var walk in walks)
            {
                var intro = intros[random.Next(intros.Length)];
                var scene = scenery[random.Next(scenery.Length)];
                var exp = experience[random.Next(experience.Length)];

                walk.Description =
                    $"The {walk.Name} in {walk.Region.Name} {intro}. " +
                    $"This {walk.Difficulty.Name.ToLower()} level walk stretches approximately {walk.Length} km and takes you through {scene}.\n\n" +

                    $"Whether you're seeking a peaceful retreat or an active day outdoors, this trail delivers an enjoyable experience, {exp}. " +
                    $"Along the way, you'll encounter picturesque spots perfect for relaxing, photography, and appreciating the natural surroundings.\n\n" +

                    $"Overall, the {walk.Name} is a great choice for anyone wanting to explore the beauty of {walk.Region.Name} while enjoying a well-balanced and rewarding hike.";
            }

            await dbContext.SaveChangesAsync();

            return new OkObjectResult("Data seeded successfully") ;
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
        public async Task<List<Walk>> Get(
     string? filterOn = null,
     string? filterQuery = null,
     string? sortBy = null,
     bool? isAscending = null,
     int? pageNumber = null,
     int? pageSize = null)
        {
            var walks = dbContext.Walks
                .Include(x => x.Difficulty)
                .Include(x => x.Region)
                .AsQueryable();

            // 🔹 Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterQuery));
                }
                else if (filterOn.Equals("Region", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Region.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("Difficulty", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Difficulty.Name.Contains(filterQuery));
                }
            }

            // 🔹 Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true
                        ? walks.OrderBy(x => x.Name)
                        : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true
                        ? walks.OrderBy(x => x.Length)
                        : walks.OrderByDescending(x => x.Length);
                }
                else if (sortBy.Equals("Difficulty", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true
                        ? walks.OrderBy(x => x.Difficulty.Name)
                        : walks.OrderByDescending(x => x.Difficulty.Name);
                }
                else if (sortBy.Equals("Region", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending == true
                        ? walks.OrderBy(x => x.Region.Name)
                        : walks.OrderByDescending(x => x.Region.Name);
                }
            }

            // 🔹 Pagination
            if (pageNumber != null && pageSize != null)
            {
                int skipResults = ((int)pageNumber - 1) * (int)pageSize;
                walks = walks.Skip(skipResults).Take((int)pageSize);
            }

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
