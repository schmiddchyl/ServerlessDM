using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientSearch.Models
{
    public class Patient
    {
        public string PatientId { get; set; }

        public string Firstname { get; set; }

        public String Lastname { get; set; }

        public String Gender { get; set; }

        public String BirthDate { get; set; }

        public String Age { get; set; } 

        public String ResultStatus { get; set; }    

        public String Vip { get; set; }


    }
}
