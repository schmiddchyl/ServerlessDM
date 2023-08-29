
using EncountersByPatientSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace EncountersByPatientSearch.Utils
{
    static class  PatientConverter
    {
        public static List<Patient> ConvertHealthDataToPatient(List<HealthDocumentData> healthDocuments)
        {
              List<Patient> patients = new List<Patient>();
            foreach (HealthDocumentData result in healthDocuments)
            {
                Patient patient = new Patient();
                patient.PatientId = result.SK;
                patient.Age = (string)result.ItemData["Age"];
                patient.Gender = (string)result.ItemData["Gender"];
                patient.BirthDate = (string)result.PK;
                patient.Firstname = result.GSI4PK;
                patient.Lastname = result.GSI4SK;
                patient.ResultStatus = "Complete";
                patient.Vip = (string)result.ItemData["Vip"];
                patients.Add(patient);
            }
            return patients;
        }

    }
}
