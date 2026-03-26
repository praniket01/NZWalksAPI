using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;
using NZWalks.Data;
using NZWalks.Mappings;
using NZWalks.Repositories;
using NZWalks.CustomActionFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NZWalks.Controllers.Models;
namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //localhost:8000/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=100
        [HttpGet]
        [Authorize(Roles = "reader,writer")]
        public async Task<List<WalkDto>> Get([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
             [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
             [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {

            List<Walk> walks = await walkRepository.Get(filterOn, filterQuery, sortBy,isAscending,pageNumber,pageSize);
            List<WalkDto> walksdto = mapper.Map<List<WalkDto>>(walks);
            return walksdto;
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var deleteWalk = await walkRepository.Delete(id);
            if (deleteWalk == null) return NotFound();
            return Ok();
        }
        
        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<WalkDto> Update([FromRoute] Guid id, [FromBody] WalkDto walkDto)
        {
            if (ModelState.IsValid)
            {
                Walk updateWalk = mapper.Map<Walk>(walkDto);
                Walk walk = await walkRepository.Update(id, updateWalk);
                if (walk == null) return null;
                WalkDto retValwalkDto = mapper.Map<WalkDto>(walk);
                return retValwalkDto;
            }
            else
                return null;
            
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "reader,writer")]
        public async Task<WalkDto> GetById([FromRoute] Guid id)
        {
            Walk walk = await walkRepository.GetByID(id);
            if (walk == null) return null;
            WalkDto returnWalkDto = mapper.Map < WalkDto >(walk);
            return returnWalkDto;
        }

        [HttpPost]
        [Route("search")]
        public async Task<List<Walk>> Search([FromBody] SearchDTO incomingQuery)
        {
            string query = incomingQuery.searchQuery;
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            var walks = await walkRepository.Search(query);

            return walks;
        }

        [HttpPost]
        //[Route("{id:guid}")]
        [ValidateModleAttribute]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> Create([FromBody] WalkDto walkdto)
        {
                Walk walk = mapper.Map<Walk>(walkdto);
                Walk addedWalk = await walkRepository.Create(walk);
                walkdto = mapper.Map<WalkDto>(addedWalk);
                return Ok(addedWalk);
        }

        [HttpPost]
        [Route("seed-descriptions")]
        public async Task<IActionResult> SeedDescriptions()
        {
            await walkRepository.SeedWalkDescriptions();
            return Ok("Descriptions seeded successfully!");
        }

        [HttpPost]
        [Route("save/{Walkid:guid}")]
        [Authorize]
        public async Task<IActionResult> SaveWalk(Guid Walkid)
        {
            var userId = User.FindFirst("id")?.Value;
            var savedWalk = await walkRepository.SaveWalk(Walkid,userId);
            return savedWalk;
        }

        [HttpDelete]
        [Route("unsave/{walkId:guid}")]
        [Authorize]
        public async Task<IActionResult> UnsaveWalk(Guid walkId)
        {
            var userId = User.FindFirst("id")?.Value;
            var unsavedWalk = await walkRepository.UnsaveWalk(userId,walkId);
            return unsavedWalk;
        }

        [HttpGet]
        [Route("saved")]
        [Authorize]
        public async Task<IActionResult> GetSavedWalks()
        {
            var userId = User.FindFirst("id")?.Value;
            var savedWalks = await walkRepository.GetSavedWalks(userId);
            return Ok(savedWalks);
        }
    }
}
