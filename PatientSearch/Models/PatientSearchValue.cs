using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientSearch.Models
{
    public class PatientSearchValue
    {
        // For type 'collection'
        public List<string> items { get; set; }

        // For type 'date'
        public string exact { get; set; }

        // Extend this class to add more fields for different types if needed.
    }
}
