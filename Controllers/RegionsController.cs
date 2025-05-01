using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApiProject.Models.DTO;
using MyFirstApiProject.Repositories;
using MyFirstApiProjects.Data;
using MyFirstApiProjects.Models.Domain;

namespace MyFirstApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {

        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(NZWalksDbContext _dbContext,IRegionRepository regionRepository,IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = _dbContext;
            this.regionRepository = regionRepository;
        }

        //GET ALL REGIONS
        //GET: https:///localhost:portnumber//api/regions
        [HttpGet]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            // asenkron olmayan method var regions = dbContext.Regions.ToList();
            // repository pattern kullanıldığı için bu satır iptal edildi.var regions = await dbContext.Regions.ToListAsync();
            var regions = await regionRepository.GetAlRegionAsync();

            if (regions == null)
            {
                return NotFound();
            }

            //Map/Convert Region Domain Model to Region DTO
            //Region domain model ile region DTO eşitleme
            var regionDto = mapper.Map<List<RegionDto>>(regions);

            return Ok(regionDto);
        }


        //GET SINGLE REGION
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetByIdRegions([FromRoute] Guid id)
        {
            // asenkron olmayan işlem var regions = dbContext.Regions.FirstOrDefault(m => m.Id == id);
            var regionDomain = await regionRepository.GetRegionByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map/Convert Region Domain Model to Region DTO
            //Region domain model ile region DTO eşitleme
            
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //CREATE SINGLE REGION
        //POST: https:localhost:portnumber/api/regions
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] RegionRequestDto regionRequestDto)
        {
            //automapper
            var regionDomain = mapper.Map<Region>(regionRequestDto);

            regionDomain =await regionRepository.CreateRegionAsync(regionDomain);
            //automapper
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            //CreatedAtAction metodu http 201 created kodu üretir
            //burda amaçlanan kullanıcı yeni bir region kaydettikten sonra yeni kaydı ekranda görmesi.
            return CreatedAtAction(nameof(GetByIdRegions), new { id = regionDto.Id }, regionDto);
        }

        //UPDATE SINGLE REGION
        //PUT: https:localhost:portnumber/api/regions/{id}
        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            var regionDomainModel = mapper.Map<Region>(updateRegionDto);


            regionDomainModel = await regionRepository.UpdateRegionAsync(id, regionDomainModel);
            
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);

        }

        //DELETE SINGLE REGION
        //DELETE: https:localhost:portnumber/api/regions/{id}
        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteRegionAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //map domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

    }
}
