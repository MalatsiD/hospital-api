﻿namespace Hospital_API.Entities
{
    public class AdmitAilment
    {
        public int PatientAdmitId { get; set; }
        public PatientAdmit PatientAdmit { get; set; }
        public int AilmentId { get; set; }
        public Ailment Ailment { get; set; }

        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
    }
}
