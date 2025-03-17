using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IMapper mapper;

        public SQLRegionRepository(NZWalksDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<Region> CreateRegionAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionAsync(Guid Id)
        {
            var existingRegion = await dbContext.Regions.FindAsync(Id);

            if (existingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>> GetAllRegionsAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetRegionByIdAsync(Guid id)
        {
            return await dbContext.Regions.FindAsync(id);
        }

        public async Task<Region?> UpdateRegionAsync(Guid id, UpdateRegionDto updateRegionDto)
        {
            var existingRegion = await dbContext.Regions.FindAsync(id);

            if (existingRegion == null)
            {
                return null;
            }

            // Cập nhật dữ liệu từ DTO vào Domain Model
            mapper.Map(updateRegionDto, existingRegion);

            await dbContext.SaveChangesAsync();
            return existingRegion; // Trả về Domain Model để Controller convert sang DTO
        }

    }
}
