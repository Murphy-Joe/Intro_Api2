using AutoMapper;
using AutoMapper.QueryableExtensions;
using LearningResourcesApi.Data;
using LearningResourcesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningResourcesApi.Controllers
{
    public class LearningResourcesController : ControllerBase
    {
        private readonly LearningResourcesDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;

        public LearningResourcesController(LearningResourcesDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        [HttpGet("/resources")]
        public async Task<ActionResult<GetResourcesResponse>> GetAll()
        {
            var resp = await _context.LearningResources.ProjectTo<ResourceItem>(_mapperConfig).ToListAsync();
            return Ok(resp);
        }

        [HttpGet("/resources/{id:int}", Name ="get-resource")]
        public async Task<ActionResult<ResourceItem>> Get(int id)
        {
            var response = await _context.LearningResources
                .Where(r => r.Id == id)
                .ProjectTo<ResourceItem>(_mapperConfig)
                //.Select(x => new ResourceItem
                //{
                //    Id = x.Id,
                //    Author = x.Author,
                //    Format = x.Format,
                //    Link = x.Link,
                //    SuggestedBy = x.SuggestedBy,
                //    Title = x.Title,
                //    WatchedOn = x.WatchedOn
                //})
                .SingleOrDefaultAsync();

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost("/resources")]
        public async Task<ActionResult<ResourceItem>> AddOne([FromBody] PostResourceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 
            else
            {
                var resource = _mapper.Map<LearningResource>(request);
                // adding it to the DB collection
                _context.LearningResources.Add(resource);
                // save the changes to the actual DB
                await _context.SaveChangesAsync(); // will change the itenties it saved to match the database

                var resp = _mapper.Map<ResourceItem>(resource);
                // dunno why we didn't just return Created(resp)
                return CreatedAtRoute("get-resource", new {id = resp.Id}, resp);
            }
        }
    }
}
