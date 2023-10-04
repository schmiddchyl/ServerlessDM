using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthDataBFF.Models
{
    public class PatientSearchTerm
    {
        public string encounter { get; set; }
        public string mrn { get; set; }

        // DEMOGRAPHICS or DOCUMENTS (this could be an enum)
        public string searchType { get; set; }

        public override string ToString()
        {
            return $"Encounter: {encounter}, SearchType: {searchType}, MRN: {mrn}";
        }
    }
}
