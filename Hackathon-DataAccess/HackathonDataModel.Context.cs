﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hackathon_DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HackathonEntities : DbContext
    {
        public HackathonEntities()
            : base("name=HackathonEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<MedicalProfessional> MedicalProfessionals { get; set; }
        public virtual DbSet<Medication> Medications { get; set; }
        public virtual DbSet<MpToPatient> MpToPatients { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Pharmacy> Pharmacies { get; set; }
        public virtual DbSet<Script> Scripts { get; set; }
        public virtual DbSet<PharmacyEvent> PharmacyEvents { get; set; }
        public virtual DbSet<PatientDenied> PatientDenieds { get; set; }
        public virtual DbSet<PatientUsage> PatientUsages { get; set; }
    }
}
