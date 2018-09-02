using HiperRestApiPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiperRestApiPackSample.Models
{
    public class SampleResuest : PagedRequest
    {
        public string Field1 { get; set; }
        public string Field2 { get; set; }

        public string Name { get; set; }
        public string CompanyId { get; set; }

    }
}
