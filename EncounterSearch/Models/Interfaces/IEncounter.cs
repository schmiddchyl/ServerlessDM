using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemSearch.Models
{
    public interface IEncounter
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        long id { get; }

        /// <summary>
        /// The Assigning Authority for this encounter (this is the hl7root)
        /// </summary>
        string assigningAuthority { get; }

        /// <summary>
        /// The Admit source for this encounter [HL7]
        /// </summary>
        string admitSource { get; }

        #region Foreign Keys

        /// <summary>
        /// The Admit Type for this encounter.
        /// </summary>
        ICode admitType { get; }

        /// <summary>
        /// The Medical Facility associated with this encounter.
        /// </summary>
        ICode facility { get; }

        /// <summary>
        /// The Master Patient Index entry for this encounter. This is the link to the Patient's Demographics
        /// </summary>
        long masterPatientIndexId { get; }

        /// <summary>
        /// The Medical Record entry for this encounter. This is the link to the Patient's Address / Phone / Emergency Contact information associated with this encounter.
        /// </summary>
        long medicalRecordId { get; }

        /// <summary>
        /// The Medical Department for this encounter. [HL7]
        /// </summary>
        ICode department { get; }

        /// <summary>
        /// The Medical Unit for this encounter. [HL7]
        /// </summary>
        long defaultUnitId { get; }

        /// <summary>
        /// The Medical Payor for this encounter. [HL7]
        /// </summary>
        long? medicalPayorId { get; }

        /// <summary>
        /// The Medical Service for this encounter. [HL7]
        /// </summary>
        ICode medicalService { get; }

        /// <summary>
        /// The Nursing Station associated with this encounter. [HL7]
        /// </summary>
        long nursingStationId { get; }

        #endregion Foreign Keys

        #region Physicians

        /// <summary>
        /// Link to the OnBase user which is the Attending Physician for the Encounter.
        /// </summary>
        long attendingPhysicianId { get; }

        /// <summary>
        /// Link to the OnBase user which is the Admitting Physician for the Encounter.
        /// </summary>
        long admittingPhysicianId { get; }

        /// <summary>
        /// Link to the OnBase user which is the Referring Physician for the Encounter.
        /// </summary>
        long? referringPhysicianId { get; }

        #endregion Physicians

        #region Encounter
        /// <summary>
        /// The encounter identifier (from admit system)
        /// </summary>
        string encounterId { get; }

        /// <summary>
        /// Auto-name string for the Encounter.
        /// </summary>
        string name { get; }

        /// <summary>
        /// External visit Id Number (from admit system)
        /// </summary>
        string alternateMRN { get; }

        /// <summary>
        /// Date the Patient was admitted. 
        /// </summary>
        DateTime? admitDate { get; }

        /// <summary>
        /// Date the Patient was discharged.
        /// </summary>
        DateTime? dischargeDate { get; }

        /// <summary>
        /// The Bed that the Patient is in.
        /// </summary>
        string bed { get; }

        /// <summary>
        /// The Room that the Patient is in.
        /// </summary>
        string room { get; }

        /// <summary>
        /// Encounter specific comments from admit system
        /// </summary>
        string encounterComment { get; }

        /// <summary>
        /// Extra info related to encounter type
        /// </summary>
        string encounterType { get; }

        /// <summary>
        /// Admit system patient classification Id.
        /// </summary>
        string patientClass { get; }

        /// <summary>
        /// Status code defining the disposition of the patient at time of discharge. e.g. 20 - Expired.
        /// </summary>
        string patientDisposition { get; }

        /// <summary>
        /// Facility specific patient type classification.
        /// </summary>
        string patientType { get; }

        /// <summary>
        /// The Primary Diagnosis related to this encounter.
        /// </summary>
        string primaryDiagnosis { get; }

        /// <summary>
        /// The total visit charges.
        /// </summary>
        decimal? totalCharges { get; }

        #endregion Encounter Fields
    }
}
