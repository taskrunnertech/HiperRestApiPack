using AutoFixture;
using HiperRestApiPackSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiperRestApiPack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IFilteredQuery _filteredQuery;
        private readonly SampleDbContext _context;

        public ProductController(IFilteredQuery filteredQuery, SampleDbContext context)
        {
            _context = context;
            _filteredQuery = filteredQuery;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]SampleResuest request)
        {
            var query = _context.Products.Include(p => p.Variants);
            var result = await _filteredQuery.ToPageList(query, request);
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery]SampleResuest request)
        {
            var query = _context.Products.Where(p=>p.Id == id);
            var result = await _filteredQuery.FirstOrDefault(query, request);
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


}
