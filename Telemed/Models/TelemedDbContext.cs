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
                entity.Property(e => e.Totalchunks).HasColumnName("totalchunks");
                entity.Property(e => e.Totalsize).HasColumnName("totalsize");
                entity.Property(e => e.Updatedby).HasColumnName("updatedby");
                entity.Property(e => e.Updateddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updateddate");
                entity.Property(e => e.Uploadedchunks)
                    .HasDefaultValue(0)
                    .HasColumnName("uploadedchunks");
            });
        

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}