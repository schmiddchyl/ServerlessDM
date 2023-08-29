
namespace ItemSearch.Models
{
    public class Encounter : IEncounter
    {
        public long id { get; set; }

        public string assigningAuthority { get; set; }

        public string admitSource { get; set; }

        public ICode admitType { get; set; }

        public ICode facility { get; set; }

        public long masterPatientIndexId { get; set; }

        public long medicalRecordId { get; set; }

        public ICode department { get; set; }

        public long defaultUnitId { get; set; }

        public long? medicalPayorId { get; set; }

        public ICode medicalService { get; set; }

        public long nursingStationId { get; set; }

        public long attendingPhysicianId { get; set; }

        public long admittingPhysicianId { get; set; }

        public long? referringPhysicianId { get; set; }

        public string encounterId { get; set; }

        public string name { get; set; }

        public string alternateMRN { get; set; }

        public DateTime? admitDate { get; set; }

        public DateTime? dischargeDate { get; set; }

        public string bed { get; set; }

        public string room { get; set; }

        public bool isVip { get; set; }

        public string encounterComment { get; set; }

        public string encounterType { get; set; }

        public string patientClass { get; set; }

        public string patientDisposition { get; set; }

        public string patientType { get; set; }

        public string primaryDiagnosis { get; set; }

        public decimal? totalCharges { get; set; }
    }
}


