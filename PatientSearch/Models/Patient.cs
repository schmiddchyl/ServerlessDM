using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientSearch.Models
{
    public class Patient
    {
        public string id { get; set; }

        public string firstName { get; set; }

        public String lastName { get; set; }

        public String gender { get; set; }

        public String birthDate { get; set; }

        public String age { get; set; } 

        public String ResultStatus { get; set; }    

        public String isVIP { get; set; }

        public String masterPatientIndexNumber { get; set; }

        public String ssn { get; set; }


    }
}
