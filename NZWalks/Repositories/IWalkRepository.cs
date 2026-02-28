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
    }
}
