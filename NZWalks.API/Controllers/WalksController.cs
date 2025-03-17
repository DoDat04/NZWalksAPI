using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWorkRequestDto addWorkRequestDto)
        {

            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWorkRequestDto);

            // Use Domain Model to Create Walk
            walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);

            // Map Domain Model to DTO
            var walkDto = mapper.Map<AddWorkRequestDto>(walkDomainModel);
            return Ok(walkDto);
        }

        // GET ALL Walk
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Domain Model From Database
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize); // Nếu isAscending là null thì sẽ được đổi thành true

            // Create an exception
            throw new Exception("This is a new exception");

            // Map Domain Model to DTO
            var walkDto = mapper.Map<List<WalkDto>>(walkDomainModel);
            return Ok(walkDto);
        }

        // GET BY ID Walk
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get Domain Model
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound(new { message = "Walk Not Found"});
            }

            // Map Domain Model to DTO
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }

        // UPDATE WALK By ID
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            // Nó return về 1 domain model
            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound(new { message = "Walk Not Found" });
            }

            // Map Domain Model to DTO
            var walkDto = mapper.Map<UpdateWalkRequestDto>(walkDomainModel);
            return Ok(walkDto);       
        }

        // DELETE WALK
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if ( walkDomainModel == null)
            {
                return NotFound(new { message = "Walk not found" });
            }

            return Ok(new { message = "Delete Successfully" });
        }
    }
}
