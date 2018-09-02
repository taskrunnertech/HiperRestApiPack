using AutoFixture;
using HiperRestApiPack.FilterExtensions;
using HiperRestApiPack.Mongo;
using HiperRestApiPackSample.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using Platform.API.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiperRestApiPack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesAsyncController : ControllerBase
    {
        private readonly IMongoRepository<Group> _groupRepo;

        public ValuesAsyncController(IMongoRepository<Group> groupRepo)
        {
            _groupRepo = groupRepo;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IPage<Group>>> Get([FromQuery]SampleResuest request, [FromQuery]string fields = "")
        {
            IMongoQueryable<Group> result = _groupRepo.GetAll(c => c.IsActive && !c.IsDeleted);

            if (!string.IsNullOrEmpty(request.Name))
            {
                result = result.Where(c => c.Name == request.Name);
            }
            if (!string.IsNullOrEmpty(request.CompanyId))
            {
                result = result.Where(c => c.CompanyId == request.CompanyId);
            }
           

            IPage<Group> resultData = await result.Select(fields).ToPageAsync(request);
            return Ok(resultData);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
