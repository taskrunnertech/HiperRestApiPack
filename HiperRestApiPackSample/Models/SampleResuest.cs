using HiperRestApiPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiperRestApiPackSample.Models
{
    public class SampleResuest : PagedRequest
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }

    }
}
