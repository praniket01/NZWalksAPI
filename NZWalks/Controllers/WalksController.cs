using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;
using NZWalks.Data;
using NZWalks.Mappings;
using NZWalks.Repositories;
using NZWalks.CustomActionFilter;
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

        [HttpGet]
        public async Task<List<WalkDto>> Get()
        {
            List<Walk> walks = await walkRepository.Get();
            List<WalkDto> walksdto = mapper.Map<List<WalkDto>>(walks);
            return walksdto;
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var deleteWalk = await walkRepository.Delete(id);
            if (deleteWalk == null) return NotFound();
            return Ok();
        }
        
        [HttpPut]
        [Route("{id:guid}")]
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
        public async Task<WalkDto> GetById([FromBody] Guid id)
        {
            Walk walk = await walkRepository.GetByID(id);
            if (walk == null) return null;
            WalkDto returnWalkDto = mapper.Map < WalkDto >(walk);
            return returnWalkDto;
        }

        [HttpPost]
        //[Route("{id:guid}")]
        [ValidateModleAttribute]
        public async Task<IActionResult> Create([FromBody] WalkDto walkdto)
        {
                Walk walk = mapper.Map<Walk>(walkdto);
                Walk addedWalk = await walkRepository.Create(walk);
                walkdto = mapper.Map<WalkDto>(addedWalk);
                return Ok(addedWalk);
        }


    }
}
