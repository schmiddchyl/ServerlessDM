using PatientSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace PatientSearch.Utils
{
    static class  PatientConverter
    {
        public static List<Patient> ConvertHealthDataToPatient(List<HealthDocumentData> healthDocuments)
        {
              List<Patient> patients = new List<Patient>();
            foreach (HealthDocumentData result in healthDocuments)
            {
                Patient patient = new Patient();
                patient.id = result.SK;
                patient.age = result.ItemData["Age"];
                patient.gender = result.ItemData["Gender"];
                patient.birthDate = result.PK;
                patient.firstName = result.GSI4PK;
                patient.lastName = result.GSI4SK;
                patient.ResultStatus = "Complete";
                patient.isVIP = result.ItemData["Vip"];
                patient.masterPatientIndexNumber = result.GSI5PK;
                patient.ssn = result.GSI5SK;
                patients.Add(patient);
            }
            return patients;
        }

    }
}
