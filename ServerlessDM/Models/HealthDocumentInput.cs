using CreateHealthDocumentLambda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandCreateHealthDocumentLambda.Models
{
    public  class HealthDocumentInput
    {
        public HealthDocumentData HealthDocumentData { get; set; }
        public string content { get; set; }
        public string path { get; set; }
    }
}
