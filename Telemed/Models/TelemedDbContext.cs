using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Telemed.Models;

public partial class TelemedDbContext : DbContext
{
    public TelemedDbContext()
    {
    }

    public TelemedDbContext(DbContextOptions<TelemedDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Encounter> Encounters { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Videosession> Videosessions { get; set; }

    public virtual DbSet<Vital> Vitals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TelemedDB;Username=postgres;Password=Saurabh@14");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

            entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Mode)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Telemedicine'::character varying")
                .HasColumnName("mode");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Patientid).HasColumnName("patientid");
            entity.Property(e => e.Providerid).HasColumnName("providerid");
            entity.Property(e => e.Scheduleddatetime).HasColumnName("scheduleddatetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Updatedat).HasColumnName("updatedat");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Patientid)
                .HasConstraintName("fk_appointment_patient");

            entity.HasOne(d => d.Provider).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Providerid)
                .HasConstraintName("fk_appointment_provider");
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

            entity.HasOne(d => d.Appointment).WithMany(p => p.Encounters)
                .HasForeignKey(d => d.Appointmentid)
                .HasConstraintName("fk_encounter_appointment");

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

            entity.HasOne(d => d.Appointment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Appointmentid)
                .HasConstraintName("fk_payment_appointment");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
