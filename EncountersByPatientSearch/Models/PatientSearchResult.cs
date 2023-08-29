using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncountersByPatientSearch.Models
{
    public class PatientSearchResult
    {
        public PatientSearchResult(List<Patient> patients)
        {
            items = patients;
        }
        public List<Patient> items { init; get; }
    }
}

