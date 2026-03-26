using Microsoft.AspNetCore.Mvc;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;

namespace NZWalks.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> Create(Walk walk);
        Task<List<Walk>> Get(string? filterOn,string? filterQuery,string? sortBy,bool? isAscending,int? pageNumber,int? pageSize);
        Task<Walk> GetByID(Guid id);
        Task<Walk> Update(Guid id,Walk walkDto);
        Task<Walk> Delete(Guid id);
        Task<List<Walk>> Search(string query);
        Task<IActionResult> SeedWalkDescriptions();
        Task<IActionResult> SaveWalk(Guid Walkid,string username);
        Task<IActionResult> UnsaveWalk(string userId,Guid walkId);
        Task<IActionResult> GetSavedWalks(string username);
    }
}
