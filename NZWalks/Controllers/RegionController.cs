using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;
using NZWalks.Data;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    //attribute
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegion()
        {
            var regions = await regionRepository.GetRegion();

            if (regions == null)
                return NotFound();

             var regionDtos = mapper.Map<List<RegionDto>>(regions);
            return Ok(regionDtos);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRegionByID([FromRoute] Guid id)
        {
            // Extraction from domain layer
            var region = await regionRepository.GetRegionByID(id);

            // Check if region is null
            if (region == null)
                return NotFound();

            // Conversion of domain to DTO
            var regionDto = mapper.Map<RegionDto>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] RegionDto region)
        {
            try
            {
                //Convert of Region dto to domain
                var reg = mapper.Map<Region>(region);

                Region savedregion = await regionRepository.CreateRegion(reg);

                //Map domain model to dto
                var retValRegDto = mapper.Map<RegionDto>(savedregion);
                return CreatedAtAction(nameof(CreateRegion), new { id = reg.Id }, retValRegDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RegionUpdateRequestDto regionupdate)
        {

           
            var regionconverter = mapper.Map<Region>(regionupdate);
            //save to Db
            var Existingregion = await regionRepository.Update(id, regionconverter);
            if (Existingregion == null) return NotFound();

            var retValRegDto =mapper.Map<RegionDto>(Existingregion);
            //return to client
            return Ok(retValRegDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async  Task<IActionResult> Delete([FromBody] Guid id)
        {
            var reg = await regionRepository.Delete(id);
            if (reg == null)
                return NotFound();
            
            return Ok();
        }
    }
}
