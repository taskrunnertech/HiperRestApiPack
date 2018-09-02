using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using HiperRestApiPack.FilterExtensions;
using HiperRestApiPackSample.Models;

namespace HiperRestApiPack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IPage<Model>> Get([FromQuery]SampleResuest request, [FromQuery]string fields="")
        {
           var model = new Fixture().Create<IEnumerable<Model>>().AsQueryable();
            
            IPage<Model> result = model.Select(fields).ToPage(request);
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Model> Get(int id, [FromQuery]SampleResuest request, [FromQuery]string fields = "")
        {
           var model = new Fixture().Create<IEnumerable<Model>>().AsQueryable();
            return model.Select(fields).FirstOrDefault();
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
