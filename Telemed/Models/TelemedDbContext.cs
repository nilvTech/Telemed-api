using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;   // ← Important: Add this using statement

namespace Telemed.Models
{
    public partial class TelemedDbContext : DbContext
    {
        public TelemedDbContext(DbContextOptions<TelemedDbContext> options)
            : base(options)
        {
        }

        // ====================== All Entity DbSets ======================
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Appointmentdocument> Appointmentdocuments { get; set; }
        public virtual DbSet<Appointmentnote> Appointmentnotes { get; set; }
        public virtual DbSet<Appointmentstatushistory> Appointmentstatushistories { get; set; }
        public virtual DbSet<Consultation> Consultations { get; set; }
        public virtual DbSet<Consultationdiagnosis> Consultationdiagnoses { get; set; }
        public virtual DbSet<Consultationnote> Consultationnotes { get; set; }
        public virtual DbSet<Consultationprescription> Consultationprescriptions { get; set; }
        public virtual DbSet<Encounter> Encounters { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Patientalert> Patientalerts { get; set; }
        public virtual DbSet<Patientfollowup> Patientfollowups { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Videosession> Videosessions { get; set; }
        public virtual DbSet<Vital> Vitals { get; set; }
        public virtual DbSet<Filemaster> Filemasters { get; set; }
        public virtual DbSet<ProviderInfo> Providerinfos { get; set; }

        public virtual DbSet<Providerprofile> Providerprofiles { get; set; }

        public virtual DbSet<ProviderGroup> ProviderGroups { get; set; }

        public virtual DbSet<ProviderGroup_Member> ProviderGroupMembers { get; set; }

        public virtual DbSet<Device> Devices { get; set; }

        // NEW DbSets for Condition Module
        public virtual DbSet<ConditionMaster> ConditionMasters { get; set; } = null!;
        public virtual DbSet<PatientCondition> PatientConditions { get; set; } = null!;

        // Task

        public virtual DbSet<PatientTask> PatientTasks { get; set; }

        //Claim

        public DbSet<Claim> Claims { get; set; }
        public DbSet<Claimdetail> Claimdetails { get; set; }

        // RPM

        public virtual DbSet<Rpmmonitoring> Rpmmonitorings { get; set; }

        // Care Plan

        public virtual DbSet<Careplan> Careplans { get; set; }

        public virtual DbSet<Smartgoal> Smartgoals { get; set; }

        public virtual DbSet<Clinicalmaster> Clinicalmasters { get; set; }

        public virtual DbSet<Clinicalorder> Clinicalorders { get; set; }

        public virtual DbSet<Followup> Followups { get; set; }




        // ====================== Keyless DTO for Patient Summary ======================
        public virtual DbSet<PatientSummaryDto> PatientSummaries { get; set; } = null!;

        public TelemedDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ====================== Entity Configurations ======================

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Adminid).HasName("admin_pkey");
                entity.ToTable("admin");
                entity.HasIndex(e => e.Email, "admin_email_key").IsUnique();
                entity.Property(e => e.Adminid).HasColumnName("adminid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Email)
                    .HasMaxLength(120)
                    .HasColumnName("email");
                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .HasColumnName("firstname");
                entity.Property(e => e.Lastname)
                    .HasMaxLength(100)
                    .HasColumnName("lastname");
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Appointmentid).HasName("appointment_pkey");
                entity.ToTable("appointment");
                entity.HasIndex(e => e.Appointmentdate, "ix_appointment_date");
                entity.HasIndex(e => e.Patientid, "ix_appointment_patient");
                entity.HasIndex(e => new { e.Providerid, e.Appointmentdate }, "ix_appointment_provider_date");
                entity.HasIndex(e => e.Status, "ix_appointment_status");
                entity.Property(e => e.Appointmentid)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("appointmentid");
                entity.Property(e => e.Appointmentdate).HasColumnName("appointmentdate");
                entity.Property(e => e.Checkedintime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("checkedintime");
                entity.Property(e => e.Clinicid).HasColumnName("clinicid");
                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Createddate)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate");
                entity.Property(e => e.Endtime).HasColumnName("endtime");
                entity.Property(e => e.Isactive)
                    .HasDefaultValue(true)
                    .HasColumnName("isactive");
                entity.Property(e => e.Isfollowup)
                    .HasDefaultValue(false)
                    .HasColumnName("isfollowup");
                entity.Property(e => e.Ispaid)
                    .HasDefaultValue(false)
                    .HasColumnName("ispaid");
                entity.Property(e => e.Meetingid)
                    .HasMaxLength(100)
                    .HasColumnName("meetingid");
                entity.Property(e => e.Meetinglink)
                    .HasMaxLength(500)
                    .HasColumnName("meetinglink");
                entity.Property(e => e.Parentappointmentid).HasColumnName("parentappointmentid");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Paymentid).HasColumnName("paymentid");
                entity.Property(e => e.Priority)
                    .HasMaxLength(20)
                    .HasColumnName("priority");
                entity.Property(e => e.Providerid).HasColumnName("providerid");
                entity.Property(e => e.Queueposition).HasColumnName("queueposition");
                entity.Property(e => e.Starttime).HasColumnName("starttime");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
                entity.Property(e => e.Tokennumber).HasColumnName("tokennumber");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updateddate");
                entity.Property(e => e.Visitmode)
                    .HasMaxLength(50)
                    .HasColumnName("visitmode");
                entity.Property(e => e.Visitreason)
                    .HasMaxLength(255)
                    .HasColumnName("visitreason");
                entity.Property(e => e.Visittype)
                    .HasMaxLength(50)
                    .HasColumnName("visittype");
                entity.Property(e => e.Waitingminutes).HasColumnName("waitingminutes");

                entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.Patientid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_appointment_patient");

                entity.HasOne(d => d.Provider).WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.Providerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_appointment_provider");
            });

            modelBuilder.Entity<Appointmentdocument>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("appointmentdocuments_pkey");
                entity.ToTable("appointmentdocuments");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
                entity.Property(e => e.Filetype)
                    .HasMaxLength(50)
                    .HasColumnName("filetype");
                entity.Property(e => e.Fileurl)
                    .HasMaxLength(500)
                    .HasColumnName("fileurl");
                entity.Property(e => e.Uploadedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("uploadedat");
            });

            modelBuilder.Entity<Appointmentnote>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("appointmentnotes_pkey");
                entity.ToTable("appointmentnotes");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Notes).HasColumnName("notes");
            });

            modelBuilder.Entity<Appointmentstatushistory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("appointmentstatushistory_pkey");
                entity.ToTable("appointmentstatushistory");
                entity.Property(e => e.Id)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("id");
                entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
                entity.Property(e => e.Changedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("changedat");
                entity.Property(e => e.Changedby).HasColumnName("changedby");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Consultation>(entity =>
            {
                entity.HasKey(e => e.Consultationid).HasName("consultation_pkey");
                entity.ToTable("consultation");
                entity.HasIndex(e => e.Appointmentid, "ix_consultation_appointment");
                entity.HasIndex(e => e.Patientid, "ix_consultation_patient");
                entity.HasIndex(e => new { e.Providerid, e.Createddate }, "ix_consultation_provider_date");
                entity.HasIndex(e => e.Status, "ix_consultation_status");
                entity.Property(e => e.Consultationid).HasColumnName("consultationid");
                entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
                entity.Property(e => e.Bloodpressure)
                    .HasMaxLength(20)
                    .HasColumnName("bloodpressure");
                entity.Property(e => e.Chiefcomplaint)
                    .HasMaxLength(500)
                    .HasColumnName("chiefcomplaint");
                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Createddate)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate");
                entity.Property(e => e.Diagnosis)
                    .HasMaxLength(500)
                    .HasColumnName("diagnosis");
                entity.Property(e => e.Durationminutes).HasColumnName("durationminutes");
                entity.Property(e => e.Endtime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("endtime");
                entity.Property(e => e.Followupdate).HasColumnName("followupdate");
                entity.Property(e => e.Followupnotes)
                    .HasMaxLength(500)
                    .HasColumnName("followupnotes");
                entity.Property(e => e.Isactive)
                    .HasDefaultValue(true)
                    .HasColumnName("isactive");
                entity.Property(e => e.Isprescriptiongenerated)
                    .HasDefaultValue(false)
                    .HasColumnName("isprescriptiongenerated");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.Oxygensaturation).HasColumnName("oxygensaturation");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Providerid).HasColumnName("providerid");
                entity.Property(e => e.Pulse).HasColumnName("pulse");
                entity.Property(e => e.Recordingurl)
                    .HasMaxLength(500)
                    .HasColumnName("recordingurl");
                entity.Property(e => e.Respiratoryrate).HasColumnName("respiratoryrate");
                entity.Property(e => e.Starttime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("starttime");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
                entity.Property(e => e.Temperature)
                    .HasPrecision(5, 2)
                    .HasColumnName("temperature");
                entity.Property(e => e.Treatmentplan).HasColumnName("treatmentplan");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updateddate");

                entity.HasOne(d => d.Appointment).WithMany(p => p.Consultations)
                    .HasForeignKey(d => d.Appointmentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_consultation_appointment");

                entity.HasOne(d => d.Patient).WithMany(p => p.Consultations)
                    .HasForeignKey(d => d.Patientid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_consultation_patient");

                entity.HasOne(d => d.Provider).WithMany(p => p.Consultations)
                    .HasForeignKey(d => d.Providerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_consultation_provider");


                // Providerinfo


            });

            modelBuilder.Entity<Consultationdiagnosis>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("consultationdiagnosis_pkey");
                entity.ToTable("consultationdiagnosis");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Consultationid).HasColumnName("consultationid");
                entity.Property(e => e.Diagnosiscode)
                    .HasMaxLength(50)
                    .HasColumnName("diagnosiscode");
                entity.Property(e => e.Diagnosisname)
                    .HasMaxLength(200)
                    .HasColumnName("diagnosisname");

                entity.HasOne(d => d.Consultation).WithMany(p => p.Consultationdiagnoses)
                    .HasForeignKey(d => d.Consultationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_consultationdiagnosis_consultation");
            });

            modelBuilder.Entity<Consultationnote>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("consultationnotes_pkey");
                entity.ToTable("consultationnotes");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Consultationid).HasColumnName("consultationid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.HasOne(d => d.Consultation).WithMany(p => p.Consultationnotes)
                    .HasForeignKey(d => d.Consultationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_consultationnotes_consultation");
            });

            modelBuilder.Entity<Consultationprescription>(entity =>
            {
                entity.HasKey(e => e.Prescriptionid).HasName("consultationprescription_pkey");
                entity.ToTable("consultationprescription");
                entity.Property(e => e.Prescriptionid).HasColumnName("prescriptionid");
                entity.Property(e => e.Consultationid).HasColumnName("consultationid");
                entity.Property(e => e.Dosage)
                    .HasMaxLength(100)
                    .HasColumnName("dosage");
                entity.Property(e => e.Duration)
                    .HasMaxLength(100)
                    .HasColumnName("duration");
                entity.Property(e => e.Frequency)
                    .HasMaxLength(100)
                    .HasColumnName("frequency");
                entity.Property(e => e.Instructions)
                    .HasMaxLength(500)
                    .HasColumnName("instructions");
                entity.Property(e => e.Medicationname)
                    .HasMaxLength(200)
                    .HasColumnName("medicationname");

                entity.HasOne(d => d.Consultation).WithMany(p => p.Consultationprescriptions)
                    .HasForeignKey(d => d.Consultationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_consultationprescription_consultation");
            });

            modelBuilder.Entity<Encounter>(entity =>
            {
                entity.HasKey(e => e.Encounterid).HasName("encounter_pkey");
                entity.ToTable("encounter");
                entity.Property(e => e.Encounterid).HasColumnName("encounterid");
                entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
                entity.Property(e => e.Assessment).HasColumnName("assessment");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Diagnosis).HasColumnName("diagnosis");
                entity.Property(e => e.Encounterdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("encounterdate");
                entity.Property(e => e.Icd10code)
                    .HasMaxLength(10)
                    .HasColumnName("icd10code");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.Objective).HasColumnName("objective");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Plan).HasColumnName("plan");
                entity.Property(e => e.Providerid).HasColumnName("providerid");
                entity.Property(e => e.Subjective).HasColumnName("subjective");
                entity.Property(e => e.Updatedat)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updatedat");

                entity.HasOne(d => d.Patient).WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_encounter_patient");

                entity.HasOne(d => d.Provider).WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.Providerid)
                    .HasConstraintName("fk_encounter_provider");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Messageid).HasName("message_pkey");
                entity.ToTable("message");
                entity.Property(e => e.Messageid).HasColumnName("messageid");
                entity.Property(e => e.Isread)
                    .HasDefaultValue(false)
                    .HasColumnName("isread");
                entity.Property(e => e.Messagetext).HasColumnName("messagetext");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Providerid).HasColumnName("providerid");
                entity.Property(e => e.Sendertype)
                    .HasMaxLength(20)
                    .HasColumnName("sendertype");
                entity.Property(e => e.Sentat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("sentat");

                entity.HasOne(d => d.Patient).WithMany(p => p.Messages)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_message_patient");

                entity.HasOne(d => d.Provider).WithMany(p => p.Messages)
                    .HasForeignKey(d => d.Providerid)
                    .HasConstraintName("fk_message_provider");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Notificationid).HasName("notification_pkey");
                entity.ToTable("notification");
                entity.Property(e => e.Notificationid).HasColumnName("notificationid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Isread)
                    .HasDefaultValue(false)
                    .HasColumnName("isread");
                entity.Property(e => e.Message).HasColumnName("message");
                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .HasColumnName("title");
                entity.Property(e => e.Userid).HasColumnName("userid");
                entity.Property(e => e.Usertype)
                    .HasMaxLength(20)
                    .HasColumnName("usertype");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Patientid).HasName("patient_pkey");
                entity.ToTable("patient");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.Careteam).HasColumnName("careteam");
                entity.Property(e => e.Companyname).HasColumnName("companyname");
                entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
                entity.Property(e => e.Effectivedate).HasColumnName("effectivedate");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Employername).HasColumnName("employername");
                entity.Property(e => e.Firstname).HasColumnName("firstname");
                entity.Property(e => e.Gender).HasColumnName("gender");
                entity.Property(e => e.Groupname).HasColumnName("groupname");
                entity.Property(e => e.Holdername).HasColumnName("holdername");
                entity.Property(e => e.Insurancetype).HasColumnName("insurancetype");
                entity.Property(e => e.Insurancevalid).HasColumnName("insurancevalid");
                entity.Property(e => e.Insuranceverified).HasColumnName("insuranceverified");
                entity.Property(e => e.Language).HasColumnName("language");
                entity.Property(e => e.Lastname).HasColumnName("lastname");
                entity.Property(e => e.Maritalstatus).HasColumnName("maritalstatus");
                entity.Property(e => e.Middlename).HasColumnName("middlename");
                entity.Property(e => e.Mrn)
                    .HasMaxLength(50)
                    .HasColumnName("mrn");
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");
                entity.Property(e => e.Plan).HasColumnName("plan");
                entity.Property(e => e.Policynumber).HasColumnName("policynumber");
                entity.Property(e => e.Preferredcommunicationchannel).HasColumnName("preferredcommunicationchannel");
                entity.Property(e => e.Primaryprovider).HasColumnName("primaryprovider");
                entity.Property(e => e.Relationtoinsured).HasColumnName("relationtoinsured");
                entity.Property(e => e.Soapnotes).HasColumnName("soapnotes");
                entity.Property(e => e.Ssn)
                    .HasMaxLength(11)
                    .HasColumnName("ssn");
                entity.Property(e => e.Timezone).HasColumnName("timezone");
            });

            modelBuilder.Entity<Patientalert>(entity =>
            {
                entity.HasKey(e => e.Alertid).HasName("patientalert_pkey");
                entity.ToTable("patientalert");
                entity.HasIndex(e => new { e.Patientid, e.Isread }, "ix_patientalert_patientid_isread");
                entity.Property(e => e.Alertid).HasColumnName("alertid");
                entity.Property(e => e.Alertmessage)
                    .HasMaxLength(500)
                    .HasColumnName("alertmessage");
                entity.Property(e => e.Alerttype)
                    .HasMaxLength(100)
                    .HasColumnName("alerttype");
                entity.Property(e => e.Createddate)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate");
                entity.Property(e => e.Isacknowledged)
                    .HasDefaultValue(false)
                    .HasColumnName("isacknowledged");
                entity.Property(e => e.Isactive)
                    .HasDefaultValue(true)
                    .HasColumnName("isactive");
                entity.Property(e => e.Isread)
                    .HasDefaultValue(false)
                    .HasColumnName("isread");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Severity)
                    .HasMaxLength(50)
                    .HasColumnName("severity");
                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updateddate");

                entity.HasOne(d => d.Patient).WithMany(p => p.Patientalerts)
                    .HasForeignKey(d => d.Patientid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_patientalert_patient");
            });

            modelBuilder.Entity<Patientfollowup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("patientfollowup_pkey");
                entity.ToTable("patientfollowup");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Followupdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("followupdate");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("'Pending'::character varying")
                    .HasColumnName("status");

                entity.HasOne(d => d.Patient).WithMany(p => p.Patientfollowups)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_patientfollowup_patient");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Paymentid).HasName("payment_pkey");
                entity.ToTable("payment");
                entity.Property(e => e.Paymentid).HasColumnName("paymentid");
                entity.Property(e => e.Amount)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount");
                entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Paymentdate)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("paymentdate");
                entity.Property(e => e.Paymentmethod)
                    .HasMaxLength(50)
                    .HasColumnName("paymentmethod");
                entity.Property(e => e.Providerid).HasColumnName("providerid");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Pending'::character varying")
                    .HasColumnName("status");

                entity.HasOne(d => d.Patient).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_payment_patient");

                entity.HasOne(d => d.Provider).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Providerid)
                    .HasConstraintName("fk_payment_provider");
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.Prescriptionid).HasName("prescription_pkey");
                entity.ToTable("prescription");
                entity.Property(e => e.Prescriptionid).HasColumnName("prescriptionid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Dosage)
                    .HasMaxLength(100)
                    .HasColumnName("dosage");
                entity.Property(e => e.Duration)
                    .HasMaxLength(100)
                    .HasColumnName("duration");
                entity.Property(e => e.Encounterid).HasColumnName("encounterid");
                entity.Property(e => e.Frequency)
                    .HasMaxLength(100)
                    .HasColumnName("frequency");
                entity.Property(e => e.Medicinename)
                    .HasMaxLength(200)
                    .HasColumnName("medicinename");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Providerid).HasColumnName("providerid");

                entity.HasOne(d => d.Encounter).WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.Encounterid)
                    .HasConstraintName("fk_prescription_encounter");

                entity.HasOne(d => d.Patient).WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_prescription_patient");

                entity.HasOne(d => d.Provider).WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.Providerid)
                    .HasConstraintName("fk_prescription_provider");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(e => e.Providerid).HasName("provider_pkey");
                entity.ToTable("provider");
                entity.HasIndex(e => e.Email, "provider_email_key").IsUnique();
                entity.Property(e => e.Providerid).HasColumnName("providerid");
                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .HasColumnName("action");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Email)
                    .HasMaxLength(120)
                    .HasColumnName("email");
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");
                entity.Property(e => e.Primaryaddress).HasColumnName("primaryaddress");
                entity.Property(e => e.Providername)
                    .HasMaxLength(150)
                    .HasColumnName("providername");
                entity.Property(e => e.Speciality)
                    .HasMaxLength(150)
                    .HasColumnName("speciality");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Active'::character varying")
                    .HasColumnName("status");
                entity.Property(e => e.Updatedat)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updatedat");
                entity.Property(e => e.Website)
                    .HasMaxLength(200)
                    .HasColumnName("website");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Userid).HasName("users_pkey");
                entity.ToTable("users");
                entity.HasIndex(e => e.Email, "users_email_key").IsUnique();
                entity.Property(e => e.Userid).HasColumnName("userid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Email)
                    .HasMaxLength(120)
                    .HasColumnName("email");
                entity.Property(e => e.Isactive)
                    .HasDefaultValue(true)
                    .HasColumnName("isactive");
                entity.Property(e => e.Lastloginat)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("lastloginat");
                entity.Property(e => e.Passwordhash)
                    .HasMaxLength(255)
                    .HasColumnName("passwordhash");
                entity.Property(e => e.Referenceid).HasColumnName("referenceid");
                entity.Property(e => e.Refreshtoken)
                    .HasMaxLength(500)
                    .HasColumnName("refreshtoken");
                entity.Property(e => e.Refreshtokenexpiry)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("refreshtokenexpiry");
                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<Videosession>(entity =>
            {
                entity.HasKey(e => e.Videosessionid).HasName("videosession_pkey");
                entity.ToTable("videosession");
                entity.Property(e => e.Videosessionid).HasColumnName("videosessionid");
                entity.Property(e => e.Callstatus)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Scheduled'::character varying")
                    .HasColumnName("callstatus");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdat");
                entity.Property(e => e.Durationseconds).HasColumnName("durationseconds");
                entity.Property(e => e.Encounterid).HasColumnName("encounterid");
                entity.Property(e => e.Endtime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("endtime");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Providerid).HasColumnName("providerid");
                entity.Property(e => e.Recordingurl).HasColumnName("recordingurl");
                entity.Property(e => e.Starttime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("starttime");
                entity.Property(e => e.Updatedat)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updatedat");
                entity.Property(e => e.Videodata).HasColumnName("videodata");
                entity.Property(e => e.Videoname)
                    .HasMaxLength(255)
                    .HasColumnName("videoname");
                entity.Property(e => e.Videosize).HasColumnName("videosize");

                entity.HasOne(d => d.Encounter).WithMany(p => p.Videosessions)
                    .HasForeignKey(d => d.Encounterid)
                    .HasConstraintName("fk_videosession_encounter");

                entity.HasOne(d => d.Patient).WithMany(p => p.Videosessions)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_videosession_patient");

                entity.HasOne(d => d.Provider).WithMany(p => p.Videosessions)
                    .HasForeignKey(d => d.Providerid)
                    .HasConstraintName("fk_videosession_provider");
            });

            modelBuilder.Entity<Vital>(entity =>
            {
                entity.HasKey(e => e.Vitalsid).HasName("vitals_pkey");
                entity.ToTable("vitals");
                entity.Property(e => e.Vitalsid).HasColumnName("vitalsid");
                entity.Property(e => e.Bloodpressure)
                    .HasMaxLength(20)
                    .HasColumnName("bloodpressure");
                entity.Property(e => e.Bmi)
                    .HasPrecision(5, 2)
                    .HasColumnName("bmi");
                entity.Property(e => e.Encounterid).HasColumnName("encounterid");
                entity.Property(e => e.Heartrate).HasColumnName("heartrate");
                entity.Property(e => e.Height)
                    .HasPrecision(5, 2)
                    .HasColumnName("height");
                entity.Property(e => e.Oxygensaturation).HasColumnName("oxygensaturation");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Recordedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("recordedat");
                entity.Property(e => e.Respiratoryrate).HasColumnName("respiratoryrate");
                entity.Property(e => e.Temperature)
                    .HasPrecision(4, 1)
                    .HasColumnName("temperature");
                entity.Property(e => e.Weight)
                    .HasPrecision(5, 2)
                    .HasColumnName("weight");

                entity.HasOne(d => d.Encounter).WithMany(p => p.Vitals)
                    .HasForeignKey(d => d.Encounterid)
                    .HasConstraintName("fk_vitals_encounter");

                entity.HasOne(d => d.Patient).WithMany(p => p.Vitals)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_vitals_patient");
            });

            // ====================== Keyless Entity Configuration for DTO ======================
            modelBuilder.Entity<PatientSummaryDto>(entity =>
            {
                entity.HasNoKey();        // DTO has no primary key
                entity.ToView(null);      // Not mapped to any table
            });

            //========== Filemaster===========================================================

            modelBuilder.Entity<Filemaster>(entity =>
            {
                entity.HasKey(e => e.Fileid).HasName("filemaster_pkey");

                entity.ToTable("filemaster");

                entity.Property(e => e.Fileid).HasColumnName("fileid");
                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Createddate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate");
                entity.Property(e => e.Filename)
                    .HasMaxLength(255)
                    .HasColumnName("filename");
                entity.Property(e => e.Filetype)
                    .HasMaxLength(50)
                    .HasColumnName("filetype");
                entity.Property(e => e.Iscompleted)
                    .HasDefaultValue(false)
                    .HasColumnName("iscompleted");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Pdfcontent).HasColumnName("pdfcontent");
                entity.Property(e => e.Totalchunks).HasColumnName("totalchunks");
                entity.Property(e => e.Totalsize).HasColumnName("totalsize");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updateddate");
                entity.Property(e => e.Uploadedchunks)
                    .HasDefaultValue(0)
                    .HasColumnName("uploadedchunks");
                // Clinical Order
                modelBuilder.Entity<Filemaster>()
    .HasOne(f => f.Clinicalorder)
    .WithMany()
    .HasForeignKey(f => f.Clinicalorderid);
            });

            //===============Providerinfo&providerprofile===================//

            modelBuilder.Entity<ProviderInfo>(entity =>
            {
                entity.HasKey(e => e.Providerinfoid).HasName("providerinfo_pkey");

                entity.ToTable("providerinfo");

                entity.HasIndex(e => e.Email, "idx_provider_email");

                entity.HasIndex(e => e.Email, "providerinfo_email_key").IsUnique();

                entity.Property(e => e.Providerinfoid).HasColumnName("providerinfoid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdat");
                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Displayname)
                    .HasMaxLength(150)
                    .HasColumnName("displayname");
                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");
                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .HasColumnName("firstname");
                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .HasColumnName("gender");
                entity.Property(e => e.GroupName).HasMaxLength(200);
                entity.Property(e => e.Lastname)
                    .HasMaxLength(100)
                    .HasColumnName("lastname");
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");
                entity.Property(e => e.Profilepicture).HasColumnName("profilepicture");
                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("'Provider'::character varying")
                    .HasColumnName("role");
                entity.Property(e => e.Updatedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedat");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
            });

            //======= Providerprofile=====//

            modelBuilder.Entity<Providerprofile>(entity =>
            {
                entity.HasKey(e => e.Profileid).HasName("providerprofile_pkey");

                entity.ToTable("providerprofile");

                entity.HasIndex(e => new { e.City, e.State }, "idx_providerprofile_location");

                entity.HasIndex(e => e.NpiNumber, "idx_providerprofile_npi");

                entity.HasIndex(e => new { e.Speciality1, e.Speciality2 }, "idx_providerprofile_speciality");

                entity.HasIndex(e => e.NpiNumber, "providerprofile_npi_number_key").IsUnique();

                entity.HasIndex(e => e.Providerinfoid, "providerprofile_providerinfoid_key").IsUnique();

                entity.Property(e => e.Profileid).HasColumnName("profileid");
                entity.Property(e => e.Addressline1)
                    .HasMaxLength(200)
                    .HasColumnName("addressline1");
                entity.Property(e => e.Addressline2)
                    .HasMaxLength(200)
                    .HasColumnName("addressline2");
                entity.Property(e => e.Bio).HasColumnName("bio");
                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");
                entity.Property(e => e.Country)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("'United States'::character varying")
                    .HasColumnName("country");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdat");
                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Isactive)
                    .HasDefaultValue(true)
                    .HasColumnName("isactive");
                entity.Property(e => e.Licensenumber)
                    .HasMaxLength(100)
                    .HasColumnName("licensenumber");
                entity.Property(e => e.NpiNumber)
                    .HasMaxLength(50)
                    .HasColumnName("npi_number");
                entity.Property(e => e.Providerinfoid).HasColumnName("providerinfoid");
                entity.Property(e => e.Providertype)
                    .HasMaxLength(100)
                    .HasColumnName("providertype");
                entity.Property(e => e.Secondaryrole)
                    .HasMaxLength(100)
                    .HasColumnName("secondaryrole");
                entity.Property(e => e.Speciality1)
                    .HasMaxLength(150)
                    .HasColumnName("speciality1");
                entity.Property(e => e.Speciality2)
                    .HasMaxLength(150)
                    .HasColumnName("speciality2");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .HasColumnName("state");
                entity.Property(e => e.Timezone)
                    .HasMaxLength(100)
                    .HasColumnName("timezone");
                entity.Property(e => e.Updatedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedat");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
                entity.Property(e => e.Website)
                    .HasMaxLength(200)
                    .HasColumnName("website");
                entity.Property(e => e.Yearofexperience).HasColumnName("yearofexperience");
                entity.Property(e => e.Zip)
                    .HasMaxLength(20)
                    .HasColumnName("zip");

                entity.HasOne(d => d.Providerinfo).WithOne(p => p.Providerprofile)
                    .HasForeignKey<Providerprofile>(d => d.Providerinfoid)
                    .HasConstraintName("fk_providerprofile_providerinfo");
            });

            //===== Proivder Group======

            modelBuilder.Entity<ProviderGroup>(entity =>
            {
                entity.ToTable("providergroup");

                entity.HasKey(e => e.GroupId).HasName("providergroup_pkey");

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Speciality);

                // Column Mappings
                entity.Property(e => e.GroupId).HasColumnName("groupid");
                entity.Property(e => e.GroupName).HasColumnName("groupname");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Speciality).HasColumnName("speciality");
                entity.Property(e => e.Website).HasColumnName("website");
                entity.Property(e => e.Bio).HasColumnName("bio");
                entity.Property(e => e.AddressLine1).HasColumnName("addressline1");
                entity.Property(e => e.AddressLine2).HasColumnName("addressline2");
                entity.Property(e => e.City).HasColumnName("city");
                entity.Property(e => e.State).HasColumnName("state");
                entity.Property(e => e.Zip).HasColumnName("zip");
                entity.Property(e => e.Country).HasColumnName("country");
                entity.Property(e => e.IsActive).HasColumnName("isactive");
                entity.Property(e => e.CreatedAt).HasColumnName("createdat");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedat");
                entity.Property(e => e.CreatedBy).HasColumnName("createdby");
                entity.Property(e => e.UpdatedBy).HasColumnName("updatedby");

                entity.Property(e => e.GroupName).HasMaxLength(150);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Speciality).HasMaxLength(150);
                entity.Property(e => e.Website).HasMaxLength(200);
                entity.Property(e => e.AddressLine1).HasMaxLength(200);
                entity.Property(e => e.AddressLine2).HasMaxLength(200);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.State).HasMaxLength(50);
                entity.Property(e => e.Zip).HasMaxLength(20);
                entity.Property(e => e.Country)
                      .HasMaxLength(100)
                      .HasDefaultValueSql("'United States'::character varying");

                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
            });

            //==========ProviderGroup_Member==============

            modelBuilder.Entity<ProviderGroup_Member>(entity =>
            {
                entity.ToTable("providergroup_members");

                entity.HasKey(e => e.GroupMemberId).HasName("providergroup_members_pkey");

                entity.HasIndex(e => new { e.GroupId, e.ProviderInfoId }).IsUnique();

                // Column Mappings - This fixes the current error
                entity.Property(e => e.GroupMemberId).HasColumnName("groupmemberid");
                entity.Property(e => e.GroupId).HasColumnName("groupid");
                entity.Property(e => e.ProviderInfoId).HasColumnName("providerinfoid");
                entity.Property(e => e.JoinDate).HasColumnName("joindate");
                entity.Property(e => e.RoleInGroup).HasColumnName("roleingroup");
                entity.Property(e => e.IsActive).HasColumnName("isactive");
                entity.Property(e => e.CreatedAt).HasColumnName("createdat");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedat");


                entity.Property(e => e.RoleInGroup)
                      .HasMaxLength(50)
                      .HasDefaultValueSql("'Member'::character varying");

                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.JoinDate).HasDefaultValueSql("now()");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

                // Relationships
                entity.HasOne(d => d.Group)
                      .WithMany(p => p.GroupMembers)
                      .HasForeignKey(d => d.GroupId)
                      .HasConstraintName("fk_groupmember_group")
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.ProviderInfo)
                      .WithMany(p => p.GroupMemberships)
                      .HasForeignKey(d => d.ProviderInfoId)
                      .HasConstraintName("fk_groupmember_providerinfo")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //==========Device==================================================//

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("device_pkey");

                entity.ToTable("device");

                entity.HasIndex(e => e.DeviceId, "device_device_id_key").IsUnique();

                entity.HasIndex(e => e.SerialNumber, "device_serial_number_key").IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.DeviceId)
                    .HasMaxLength(200)
                    .HasColumnName("device_id");
                entity.Property(e => e.DevicePicture).HasColumnName("device_picture");
                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(200)
                    .HasColumnName("manufacturer");
                entity.Property(e => e.ModelNumber)
                    .HasMaxLength(200)
                    .HasColumnName("model_number");
                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("name");
                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(200)
                    .HasColumnName("serial_number");
                entity.Property(e => e.Status)
                    .HasDefaultValue(true)
                    .HasColumnName("status");
                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");
            });

            // ConditionMaster

            modelBuilder.Entity<ConditionMaster>(entity =>
            {
                entity.ToTable("conditionmaster");
                entity.HasKey(e => e.ConditionId);

                entity.Property(e => e.ConditionId).HasColumnName("conditionid");
                entity.Property(e => e.ConditionName).HasColumnName("conditionname");
                entity.Property(e => e.IcdCode).HasColumnName("icdcode");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Type).HasColumnName("type");

                // ✅ Fix — these were missing
                entity.Property(e => e.CreatedAt).HasColumnName("createdat");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedat");

                entity.HasIndex(e => e.IcdCode).IsUnique();
            });

            // PatientCondition

            modelBuilder.Entity<PatientCondition>(entity =>
            {
                entity.ToTable("patientcondition");
                entity.HasKey(e => e.PatientConditionId);

                entity.Property(e => e.PatientConditionId).HasColumnName("patientconditionid");
                entity.Property(e => e.PatientId).HasColumnName("patientid");
                entity.Property(e => e.ConditionId).HasColumnName("conditionid");
                entity.Property(e => e.ConsultationId).HasColumnName("consultationid");
                entity.Property(e => e.ProviderInfoId).HasColumnName("providerinfoid");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.OnsetDate).HasColumnName("onsetdate");
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.ManagedBy).HasColumnName("managedby");

                // ✅ Fix — these were missing HasColumnName
                entity.Property(e => e.CreatedAt).HasColumnName("createdat");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedat");
                entity.Property(e => e.CreatedBy).HasColumnName("createdby");
                entity.Property(e => e.UpdatedBy).HasColumnName("updatedby");

                entity.HasOne(d => d.Patient)
                      .WithMany(p => p.PatientConditions)
                      .HasForeignKey(d => d.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.ConditionMaster)
                      .WithMany(c => c.PatientConditions)
                      .HasForeignKey(d => d.ConditionId)
                      .OnDelete(DeleteBehavior.Restrict);



                entity.HasOne(d => d.ProviderInfo)
                      .WithMany(p => p.PatientConditions)
                      .HasForeignKey(d => d.ProviderInfoId)
                      .OnDelete(DeleteBehavior.SetNull);


                entity.HasOne(d => d.Consultation)
                    .WithMany(p=> p.PatientConditions)
                      .HasForeignKey(d => d.ConsultationId)
                     .OnDelete(DeleteBehavior.SetNull);

                modelBuilder.Entity<PatientCondition>()
               .Property(pc => pc.ConsultationId)
                .HasColumnName("consultationid");

            });

            // Task

            modelBuilder.Entity<PatientTask>(entity =>
            {
                entity.HasKey(e => e.Taskid).HasName("task_pkey");

                entity.ToTable("task");

                // ----------------------------------------
                // Indexes
                // ----------------------------------------
                entity.HasIndex(e => e.Duedate, "idx_task_duedate");
                entity.HasIndex(e => e.Patientid, "idx_task_patientid");
                entity.HasIndex(e => e.Status, "idx_task_status");

                // ----------------------------------------
                // Columns
                // ----------------------------------------
                entity.Property(e => e.Taskid).HasColumnName("taskid");
                entity.Property(e => e.Taskname).HasColumnName("taskname").HasMaxLength(255);
                entity.Property(e => e.Duedate).HasColumnName("duedate");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Providerinfoid).HasColumnName("providerinfoid");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("'Pending'::character varying")
                    .HasColumnName("status");
                entity.Property(e => e.Priority)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Medium'::character varying")
                    .HasColumnName("priority");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Completeddate).HasColumnName("completeddate");

                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdat");

                entity.Property(e => e.Updatedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedat");

                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");

                // ----------------------------------------
                // RELATIONSHIP with Patient
                // ----------------------------------------
                entity.HasOne(d => d.Patient)                            //                 entity.HasOne(d => d.patient)

                    .WithMany(p => p.PatientTasks)
                    .HasForeignKey(e => e.Patientid)
                    .HasConstraintName("fk_task_patient");

                // ----------------------------------------
                // RELATIONSHIP with ProviderInfo
                // ----------------------------------------
                entity.HasOne(d => d.Providerinfo)    //
                    .WithMany(p => p.PatientTasks)
                    .HasForeignKey(e => e.Providerinfoid)
                    .HasConstraintName("fk_task_provider");
            });

            // Cliam

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(e => e.Claimid).HasName("claim_pkey");

                entity.ToTable("claim");

                entity.HasIndex(e => e.Claimnumber, "claim_claimnumber_key").IsUnique();

                entity.HasIndex(e => e.Patientid, "idx_claim_patientid");

                entity.HasIndex(e => e.Status, "idx_claim_status");

                entity.Property(e => e.Claimid).HasColumnName("claimid");
                entity.Property(e => e.Claimnumber)
                    .HasMaxLength(50)
                    .HasColumnName("claimnumber");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdat");
                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Deniedreason).HasColumnName("deniedreason");
                entity.Property(e => e.Icdcode)
                    .HasMaxLength(20)
                    .HasColumnName("icdcode");
                entity.Property(e => e.Paidamount)
                    .HasPrecision(12, 2)
                    .HasDefaultValueSql("0")
                    .HasColumnName("paidamount");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Payer)
                    .HasMaxLength(100)
                    .HasColumnName("payer");
                entity.Property(e => e.Providerinfoid).HasColumnName("providerinfoid");
                entity.Property(e => e.Status)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("'Pending'::character varying")
                    .HasColumnName("status");
                entity.Property(e => e.Submissiondate).HasColumnName("submissiondate");
                entity.Property(e => e.Totalamount)
                    .HasPrecision(12, 2)
                    .HasColumnName("totalamount");
                entity.Property(e => e.Totaltime).HasColumnName("totaltime");
                entity.Property(e => e.Updatedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedat");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
            

            entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Providerinfoid).HasColumnName("providerinfoid");

                //  Patient relation
                entity.HasOne(c => c.Patient)
                    .WithMany(p => p.Claims)   // or Claim list if you add
                    .HasForeignKey(c => c.Patientid)
                    .HasConstraintName("fk_claim_patient");

                //  Provider relation
                entity.HasOne(c => c.Providerinfo)
                    .WithMany(p => p.Claims)   // change if needed
                    .HasForeignKey(c => c.Providerinfoid)
                    .HasConstraintName("fk_claim_provider");
            });

            // ================= CLAIM DETAIL =================
            modelBuilder.Entity<Claimdetail>(entity =>
            {
                entity.HasKey(e => e.Claimdetailid).HasName("claimdetail_pkey");

                entity.ToTable("claimdetail");

                entity.Property(e => e.Claimdetailid).HasColumnName("claimdetailid");
                entity.Property(e => e.Amount)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount");
                entity.Property(e => e.Claimid).HasColumnName("claimid");
                entity.Property(e => e.Cptcode)
                    .HasMaxLength(20)
                    .HasColumnName("cptcode");
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");
                entity.Property(e => e.Units)
                    .HasDefaultValue(1)
                    .HasColumnName("units");

                entity.HasOne(d => d.Claim).WithMany(p => p.Claimdetails)
                    .HasForeignKey(d => d.Claimid)
                    .HasConstraintName("fk_claimdetail_claim");
            });

            // RPM

                modelBuilder.Entity<Rpmmonitoring>(entity =>
                {
                    entity.HasKey(e => e.Rpmmonitoringid).HasName("rpmmonitoring_pkey");

                    entity.ToTable("rpmmonitoring");

                    entity.HasIndex(e => e.Isreviewed, "idx_rpmmonitoring_isreviewed");

                    entity.HasIndex(e => new { e.Patientid, e.Isreviewed }, "idx_rpmmonitoring_patient_isreviewed");

                    entity.HasIndex(e => new { e.Patientid, e.Readingdate }, "idx_rpmmonitoring_patient_readingdate").IsDescending(false, true);

                    entity.HasIndex(e => e.Patientid, "idx_rpmmonitoring_patientid");

                    entity.HasIndex(e => e.Readingdate, "idx_rpmmonitoring_readingdate").IsDescending();

                    entity.Property(e => e.Rpmmonitoringid).HasColumnName("rpmmonitoringid");
                    entity.Property(e => e.Createdat)
                        .HasDefaultValueSql("now()")
                        .HasColumnName("createdat");
                    entity.Property(e => e.Createdby).HasColumnName("createdby");
                    entity.Property(e => e.Deviceid)
                        .HasMaxLength(100)
                        .HasColumnName("deviceid");
                    entity.Property(e => e.Devicetype)
                        .HasMaxLength(50)
                        .HasColumnName("devicetype");
                    entity.Property(e => e.Diastolic).HasColumnName("diastolic");
                    entity.Property(e => e.Glucose).HasColumnName("glucose");
                    entity.Property(e => e.Glucoseunit)
                        .HasMaxLength(10)
                        .HasDefaultValueSql("'mg/dL'::character varying")
                        .HasColumnName("glucoseunit");
                    entity.Property(e => e.Heartrate).HasColumnName("heartrate");
                    entity.Property(e => e.Height)
                        .HasPrecision(5, 2)
                        .HasColumnName("height");
                    entity.Property(e => e.Heightunit)
                        .HasMaxLength(10)
                        .HasDefaultValueSql("'inch'::character varying")
                        .HasColumnName("heightunit");
                    entity.Property(e => e.Isauto)
                        .HasDefaultValue(false)
                        .HasColumnName("isauto");
                    entity.Property(e => e.Isreviewed)
                        .HasDefaultValue(false)
                        .HasColumnName("isreviewed");
                    entity.Property(e => e.Note).HasColumnName("note");
                    entity.Property(e => e.Patientid).HasColumnName("patientid");
                    entity.Property(e => e.Readingdate).HasColumnName("readingdate");
                    entity.Property(e => e.Respiratoryrate).HasColumnName("respiratoryrate");
                    entity.Property(e => e.Reviewedat).HasColumnName("reviewedat");
                    entity.Property(e => e.Reviewedby).HasColumnName("reviewedby");
                    entity.Property(e => e.Sourcedata)
                        .HasMaxLength(20)
                        .HasColumnName("sourcedata");
                    entity.Property(e => e.Spo2).HasColumnName("spo2");
                    entity.Property(e => e.Systolic).HasColumnName("systolic");
                    entity.Property(e => e.Temperature)
                        .HasPrecision(5, 2)
                    .HasColumnName("temperature");
                entity.Property(e => e.Temperatureunit)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'F'::character varying")
                    .HasColumnName("temperatureunit");
                entity.Property(e => e.Updatedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedat");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
                entity.Property(e => e.Weight)
                    .HasPrecision(6, 2)
                    .HasColumnName("weight");
                entity.Property(e => e.Weightunit)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'lbs'::character varying")
                    .HasColumnName("weightunit");
                    entity.HasOne(d => d.ReviewedByProvider)
       // ReviewByProvider
       .WithMany()
       .HasForeignKey(d => d.Reviewedby)
       .HasConstraintName("fk_rpmmonitoring_reviewedby");

                    //  Patient FK
                    entity.HasOne(r => r.Patient)
                    .WithMany(p => p.Rpmmonitorings)
                    .HasForeignKey(r => r.Patientid)
                    .HasConstraintName("fk_rpmmonitoring_patient");

                // Provider FK 
                entity.HasOne(r => r.ReviewedByProvider)
                    .WithMany(p => p.Rpmmonitorings)
                    .HasForeignKey(r => r.Reviewedby)
                    .HasConstraintName("fk_rpmmonitoring_reviewedby");
            });

            // Care Plan

            modelBuilder.Entity<Careplan>(entity =>
            {
                entity.HasKey(e => e.Careplanid).HasName("careplan_pkey");

                entity.ToTable("careplan");

                entity.HasIndex(e => e.Nextreviewdate, "idx_careplan_nextreviewdate");

                entity.HasIndex(e => e.Patientid, "idx_careplan_patientid");

                entity.HasIndex(e => e.Risklevel, "idx_careplan_risklevel");

                entity.HasIndex(e => e.Status, "idx_careplan_status");

                entity.Property(e => e.Careplanid).HasColumnName("careplanid");
                entity.Property(e => e.Allergies).HasColumnName("allergies");
                entity.Property(e => e.Bmitarget)
                    .HasPrecision(5, 2)
                    .HasColumnName("bmitarget");
                entity.Property(e => e.Bpdiastolictarget).HasColumnName("bpdiastolictarget");
                entity.Property(e => e.Bpsystolictarget).HasColumnName("bpsystolictarget");
                entity.Property(e => e.Ccmminutes)
                    .HasDefaultValue(0)
                    .HasColumnName("ccmminutes");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdat");
                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Enddate).HasColumnName("enddate");
                entity.Property(e => e.Glucosetargetmax).HasColumnName("glucosetargetmax");
                entity.Property(e => e.Glucosetargetmin).HasColumnName("glucosetargetmin");
                entity.Property(e => e.Goals).HasColumnName("goals");
                entity.Property(e => e.Hba1ctarget)
                    .HasPrecision(4, 2)
                    .HasColumnName("hba1ctarget");
                entity.Property(e => e.Heartratetargetmax).HasColumnName("heartratetargetmax");
                entity.Property(e => e.Heartratetargetmin).HasColumnName("heartratetargetmin");
                entity.Property(e => e.Interventions).HasColumnName("interventions");
                entity.Property(e => e.Lastreviewdate).HasColumnName("lastreviewdate");
                entity.Property(e => e.Ldltarget)
                    .HasPrecision(4, 1)
                    .HasColumnName("ldltarget");
                entity.Property(e => e.Medications).HasColumnName("medications");
                entity.Property(e => e.Nextreviewdate).HasColumnName("nextreviewdate");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Problems).HasColumnName("problems");
                entity.Property(e => e.Providerinfoid).HasColumnName("providerinfoid");
                entity.Property(e => e.Reviewfrequency)
                    .HasMaxLength(50)
                    .HasColumnName("reviewfrequency");
                entity.Property(e => e.Risklevel)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Medium'::character varying")
                    .HasColumnName("risklevel");
                entity.Property(e => e.Spo2target).HasColumnName("spo2target");
                entity.Property(e => e.Startdate).HasColumnName("startdate");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Active'::character varying")
                    .HasColumnName("status");
                entity.Property(e => e.Updatedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedat");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
                entity.Property(e => e.Weighttarget)
                    .HasPrecision(6, 2)
                    .HasColumnName("weighttarget");

                entity.HasOne(d => d.Patient).WithMany(p => p.Careplans)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_careplan_patient");

                entity.HasOne(d => d.Providerinfo).WithMany(p => p.Careplans)
                    .HasForeignKey(d => d.Providerinfoid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_careplan_provider");
            });

            // Smart Goal

            modelBuilder.Entity<Smartgoal>(entity =>
            {
                entity.HasKey(e => e.Smartgoalid).HasName("smartgoal_pkey");

                entity.ToTable("smartgoal");

                entity.HasIndex(e => e.Measurementtype, "idx_smartgoal_measurementtype");

                entity.HasIndex(e => e.Patientid, "idx_smartgoal_patientid");

                entity.HasIndex(e => e.Providerinfoid, "idx_smartgoal_providerid");

                entity.HasIndex(e => e.Status, "idx_smartgoal_status");

                entity.HasIndex(e => e.Targetdate, "idx_smartgoal_targetdate");

                entity.Property(e => e.Smartgoalid).HasColumnName("smartgoalid");
                entity.Property(e => e.Careplanid).HasColumnName("careplanid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdat");
                entity.Property(e => e.Createdby).HasColumnName("createdby");
                entity.Property(e => e.Currentvalue)
                    .HasPrecision(10, 2)
                    .HasColumnName("currentvalue");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Diettype)
                    .HasMaxLength(100)
                    .HasColumnName("diettype");
                entity.Property(e => e.Exercisetype)
                    .HasMaxLength(100)
                    .HasColumnName("exercisetype");
                entity.Property(e => e.Goaltitle)
                    .HasMaxLength(255)
                    .HasColumnName("goaltitle");
                entity.Property(e => e.Measurementtype)
                    .HasMaxLength(50)
                    .HasColumnName("measurementtype");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Progress)
                    .HasPrecision(5, 2)
                    .HasDefaultValueSql("0")
                    .HasColumnName("progress");
                entity.Property(e => e.Providerinfoid).HasColumnName("providerinfoid");
                entity.Property(e => e.Startdate).HasColumnName("startdate");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Active'::character varying")
                    .HasColumnName("status");
                entity.Property(e => e.Targetdate).HasColumnName("targetdate");
                entity.Property(e => e.Targetvalue)
                    .HasPrecision(10, 2)
                    .HasColumnName("targetvalue");
                entity.Property(e => e.Unit)
                    .HasMaxLength(20)
                    .HasColumnName("unit");
                entity.Property(e => e.Updatedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedat");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
                entity.Property(e => e.Weeklyminutes).HasColumnName("weeklyminutes");

                entity.HasOne(d => d.Careplan).WithMany(p => p.Smartgoals)
                    .HasForeignKey(d => d.Careplanid)
                    .HasConstraintName("fk_smartgoal_careplan");

                entity.HasOne(d => d.Patient).WithMany(p => p.Smartgoals)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_smartgoal_patient");

                entity.HasOne(d => d.Providerinfo).WithMany(p => p.Smartgoals)
                    .HasForeignKey(d => d.Providerinfoid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_smartgoal_provider");
            });

            // Clinical Master

            modelBuilder.Entity<Clinicalmaster>(entity =>
            {
                entity.HasKey(e => e.Clinicalmasterid).HasName("clinicalmaster_pkey");

                entity.ToTable("clinicalmaster");

                entity.HasIndex(e => e.Ordercode, "idx_clinicalmaster_ordercode");

                entity.HasIndex(e => e.Ordername, "idx_clinicalmaster_ordername");

                entity.HasIndex(e => e.Ordertype, "idx_clinicalmaster_ordertype");

                entity.Property(e => e.Clinicalmasterid).HasColumnName("clinicalmasterid");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdat");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Ordercode)
                    .HasMaxLength(50)
                    .HasColumnName("ordercode");
                entity.Property(e => e.Ordername)
                    .HasMaxLength(150)
                    .HasColumnName("ordername");
                entity.Property(e => e.Ordertype)
                    .HasMaxLength(20)
                    .HasColumnName("ordertype");
            });

            // Clinical Order

            modelBuilder.Entity<Clinicalorder>(entity =>
            {
                entity.HasKey(e => e.Clinicalorderid).HasName("clinicalorder_pkey");

                entity.ToTable("clinicalorder");

                entity.HasIndex(e => e.Clinicalmasterid, "idx_clinicalorder_clinicalmasterid");

                entity.HasIndex(e => e.Encounterid, "idx_clinicalorder_encounterid");

                entity.HasIndex(e => e.Orderdate, "idx_clinicalorder_orderdate");

                entity.HasIndex(e => e.Patientid, "idx_clinicalorder_patientid");

                entity.HasIndex(e => e.Priority, "idx_clinicalorder_priority");

                entity.HasIndex(e => e.Providerinfoid, "idx_clinicalorder_providerinfoid");

                entity.HasIndex(e => e.Status, "idx_clinicalorder_status");

                entity.Property(e => e.Clinicalorderid).HasColumnName("clinicalorderid");
                entity.Property(e => e.Clinicalmasterid).HasColumnName("clinicalmasterid");
                entity.Property(e => e.Completeddate).HasColumnName("completeddate");
                entity.Property(e => e.Createdat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdat");
                entity.Property(e => e.Encounterid).HasColumnName("encounterid");
                entity.Property(e => e.Orderdate)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("orderdate");
                entity.Property(e => e.Patientid).HasColumnName("patientid");
                entity.Property(e => e.Priority)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Routine'::character varying")
                    .HasColumnName("priority");
                entity.Property(e => e.Providerinfoid).HasColumnName("providerinfoid");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("'Pending'::character varying")
                    .HasColumnName("status");
                entity.Property(e => e.Updatedat)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedat");

                entity.HasOne(d => d.Clinicalmaster).WithMany(p => p.Clinicalorders)
                    .HasForeignKey(d => d.Clinicalmasterid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_clinicalorder_master");

                entity.HasOne(d => d.Encounter).WithMany(p => p.Clinicalorders)
                    .HasForeignKey(d => d.Encounterid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_clinicalorder_encounter");

                entity.HasOne(d => d.Patient).WithMany(p => p.Clinicalorders)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("fk_clinicalorder_patient");

                entity.HasOne(d => d.Providerinfo).WithMany(p => p.Clinicalorders)
                    .HasForeignKey(d => d.Providerinfoid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_clinicalorder_providerinfo");

            });

            // FollowUp

            modelBuilder.Entity<Followup>(entity =>
                {
                    entity.HasKey(e => e.Followupid).HasName("followup_pkey");

                    entity.ToTable("followup");

                    entity.Property(e => e.Followupid).HasColumnName("followupid");
                    entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
                    entity.Property(e => e.Createdat)
                        .HasDefaultValueSql("now()")
                        .HasColumnName("createdat");
                    entity.Property(e => e.Followupdate).HasColumnName("followupdate");
                    entity.Property(e => e.Followuptype)
                        .HasMaxLength(50)
                        .HasColumnName("followuptype");
                    entity.Property(e => e.Notes).HasColumnName("notes");
                    entity.Property(e => e.Patientid).HasColumnName("patientid");
                    entity.Property(e => e.Status)
                        .HasMaxLength(20)
                        .HasDefaultValueSql("'Scheduled'::character varying")
                        .HasColumnName("status");

                    entity.HasOne(d => d.Appointment).WithMany(p => p.Followups)
                        .HasForeignKey(d => d.Appointmentid)
                        .HasConstraintName("fk_followup_appointment");

                    entity.HasOne(d => d.Patient).WithMany(p => p.Followups)
                        .HasForeignKey(d => d.Patientid)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_followup_patient");
                });



          OnModelCreatingPartial(modelBuilder);
        }
            

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
