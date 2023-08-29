using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncountersByPatientSearch.Models
{
    public class PatientSearchTerm
    {
        public string name { get; set; }
        public string type { get; set; }
        public PatientSearchValue value { get; set; }
    }
}
