

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using EncountersByPatientSearch.Models;

namespace EncountersByPatientSearch.Utils
{
    static class  EncounterConverter
    {
        public static List<IEncounter> ConvertHealthDataToEncounters(List<HealthDocumentData> healthDocuments)
        {
              List<IEncounter> encounters = new List<IEncounter>();
            foreach (HealthDocumentData result in healthDocuments)
            {
                Encounter encounter = new Encounter();
             
                encounter.id = long.Parse((result.PK.Split('#'))[1]);
                encounter.encounterId = result.PK;
                encounter.assigningAuthority = result.GSI5SK;
                
                encounter.admitType = protectedparseICode(result.ItemData["admitType"]);
                encounter.facility  = protectedparseICode(result.ItemData["facility"]);
                encounter.masterPatientIndexId = long.Parse((result.GSI4PK.Split('#'))[1]);
                encounter.department = protectedparseICode(result.ItemData["department"]);
                encounter.medicalService = protectedparseICode(result.ItemData["medicalService"]);

                // api doc says "unitId" but patientapi is coded to "defaultUnit"
                encounter.defaultUnitId     = protectedparseLong("defaultUnitId", result.ItemData); 
                encounter.medicalPayorId    = protectedparseLong("medicalPayorId", result.ItemData);
                encounter.nursingStationId  = protectedparseLong("nursingStation", result.ItemData);
                encounter.attendingPhysicianId = protectedparseLong("attendingPhysicianId", result.ItemData);
                encounter.admittingPhysicianId = protectedparseLong("admittingPhysicianId", result.ItemData);
                encounter.referringPhysicianId = protectedparseLong("referringPhysicianId", result.ItemData);
                
                encounter.name = protectedParseString("name" ,result.ItemData);
                encounter.alternateMRN  = protectedParseString("alternateMRN", result.ItemData);
                encounter.admitDate     = parseDynamoDate(result.GSI2PK, "yyyy_MM_dd");
                encounter.dischargeDate = parseDynamoDate(protectedParseString("dischargeDate", result.ItemData), "yyyy_MM_dd");

                encounter.bed  = protectedParseString("bed",  result.ItemData);
                encounter.room = protectedParseString("room", result.ItemData);
                encounter.encounterComment  = protectedParseString("encounterComment", result.ItemData);
                encounter.encounterType     = protectedParseString("encounterType", result.ItemData);
                encounter.patientClass      = protectedParseString("patientClass", result.ItemData);
                
                encounter.patientDisposition = protectedParseString("patientDisposition", result.ItemData);
                encounter.patientType = protectedParseString("patientType",result.ItemData);                
                encounter.primaryDiagnosis = protectedParseString("primaryDiagnosis", result.ItemData);
                encounter.totalCharges = decimal.Parse(protectedParseString("totalCharges", result.ItemData));
             
                encounters.Add(encounter);
            }
            return encounters;
        }

        private static DateTime parseDynamoDate(string dateString, string format)
        {
            DateTime result;
            if (dateString == null)
            {
               return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            }
            return DateTime.MinValue;
        }

        private static ICode protectedparseICode(object arg)
        {
           ICode code = null;
            if (arg != null)
            {
            code = (ICode)arg;
            }
            return code; ;
        }

        private static string protectedParseString(String key, Dictionary<string,object> dict)
        {
            string result = null;
            try
            {
                result = (string)dict[key];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Key = " + key + " is not found.");
            }
            return result;
        }

        private static long protectedparseLong(String key, Dictionary<string, object> dict)
        {
            Object arg = null;
            long returnValue = 0;

            try
            {
                arg = dict[key];       
                string argValue = (string)arg;
                Console.WriteLine("protectedParseLong arg : " + arg);
                returnValue = Convert.ToInt64(argValue);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Key = " + key + " is not found.");
            }
       
            return returnValue; ;
        }
    }
}
