using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealtDataBFF.Models
{
    public class Code : ICode
    {
        public string code { get; set; }    

        public string systemName { get; set; }
    }
}
