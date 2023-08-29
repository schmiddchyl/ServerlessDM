

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ItemSearch.Models;
using System.Globalization;

namespace EncounterSearch.Utils
{
    static class  EncounterConverter
    {
        public static List<IEncounter> ConvertHealthDataToEncounters(List<HealthDocumentData> healthDocuments)
        {
              List<IEncounter> encounters = new List<IEncounter>();
            foreach (HealthDocumentData result in healthDocuments)
            {
                Encounter encounter = new ItemSearch.Models.Encounter();
                Console.WriteLine("---setting encounterId to " + result.PK);
                Console.WriteLine("result.ItemData " + result.ItemData);
       
                encounter.encounterId = result.PK;
                encounter.assigningAuthority = result.GSI5SK;

                encounter.admitType = (ICode)result.ItemData["admitType"];
                encounter.facility = (ICode)result.ItemData["facility"];

            //    encounter.masterPatientIndexId = long.Parse(result.GSI4PK);
           //     encounter.department = setICode(result.ItemData["department"]);
             //   encounter.medicalService = setICode(result.ItemData["medicalService"]);

                // api doc says "unitId" but patientapi is coded to "defaultUnit"
              //  encounter.defaultUnitId = long.Parse(result.ItemData["defaultUnitId"]); 
              //  encounter.medicalPayorId = long.Parse(result.ItemData["medicalPayorId"]);
              //  encounter.nursingStationId = long.Parse(result.ItemData["nursingStation"]);

              //  encounter.attendingPhysicianId = long.Parse(result.ItemData["attendingPhysicianId"]);
              //  encounter.admittingPhysicianId = long.Parse(result.ItemData["admittingPhysicianId"]);
              //  encounter.referringPhysicianId = long.Parse(result.ItemData["referringPhysicianId"]);
              /*
                encounter.name = result.ItemData["name"];
                encounter.alternateMRN = result.ItemData["alternateMRN"];
                encounter.admitDate = parseDynamoDate(result.GSI2PK, "yyyy_MM_dd");
                encounter.dischargeDate = parseDynamoDate(result.ItemData["dischargeDate"], "yyyy_MM_dd");

                encounter.bed  = result.ItemData["bed"];
                encounter.room = result.ItemData["room"];
                encounter.encounterComment = result.ItemData["encounterComment"];
                encounter.encounterType = result.ItemData["encounterType"];
                encounter.patientClass = result.ItemData["patientClass"];
              */
               // encounter.patientDisposition = (string)result.ItemData["patientDisposition"];
                /*
                encounter.patientType = result.ItemData["patientType"];                
                encounter.primaryDiagnosis = result.ItemData["primaryDiagnosis"];
                encounter.totalCharges = decimal.Parse(result.ItemData["totalCharges"]);
              */
                encounters.Add(encounter);
            }
            return encounters;
        }

        private static DateTime parseDynamoDate(string dateString, string format)
        {
            DateTime result = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            return result;
        }

        private static ICode setICode(string Json)
        {
           return System.Text.Json.JsonSerializer.Deserialize<ICode>(Json);
        }
    }
}
