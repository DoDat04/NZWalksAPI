using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        //Domain Model nó tương ứng với bảng đó trong database
        //Vd nó có 4 fields nhưng chúng ta chỉ muốn trả về 3 fields
        //Thì sẽ tạo 1 DTO chứa 3 field và map nó qua đảm bảo được về security
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            // Get Data From Database - Domain Models
            var regionsDomain = await regionRepository.GetAllRegionsAsync();

            //logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");

            // Map Domain Models to DTOs
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            // Return DTOs
            return Ok(regionsDto);
       
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            //Get Region Domain Model From Database
            var regionDomain = await regionRepository.GetRegionByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound(new { message = "Region not found" });
            }

            // Map Region Domain Model to Region DTO
            var regionsDto = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionsDto);
        }


        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion(CreateRegionDto createRegionDto)
        {
            //Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(createRegionDto);

            //Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateRegionAsync(regionDomainModel);

            // Map Domain model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionDomainModel);       
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            var updatedRegion = await regionRepository.UpdateRegionAsync(id, updateRegionDto);

            if (updatedRegion == null)
            {
                return NotFound(new { message = "Region not found" });
            }

            // Convert từ Domain Model sang DTO
            var regionDto = mapper.Map<RegionDto>(updatedRegion);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer, Reader")] // Vai trò Write và Reader sẽ đc delete
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteRegionAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound(new { message = "Region not found" });
            }

            return Ok(new {message = "Delete Successfully"});
        }
    }
}
