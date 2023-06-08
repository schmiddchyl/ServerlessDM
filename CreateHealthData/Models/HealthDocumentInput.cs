
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateHealthData.Models
{
    public  class HealthInput
    {
        public HealthData HealthData { get; set; }
        public string content { get; set; }
        public string path { get; set; }
    }
}
