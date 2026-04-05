using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace SymptomLogger.src.Data
{
    public class SymptomLoggerContext : IdentityDbContext<Patient>
    {
        public SymptomLoggerContext(DbContextOptions<SymptomLoggerContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call the base method to ensure Identity configurations are applied
            base.OnModelCreating(builder);

            // Custom configurations for Identity tables
            #region Identity table configurations
            builder.Entity<Patient>(entity =>
            {
                entity.ToTable(name: "Patients");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            #endregion

            // Custom configurations for application tables
            #region Relationship configurations

            // Symptom > Patient relationship
            builder.Entity<Symptom>()
                .HasOne(s => s.Patient)
                .WithMany(s => s.Symptoms)
                .HasForeignKey(s => s.PatientId);

            // SymptomLog > Patient relationship
            builder.Entity<SymptomLog>()
                .HasOne(sl => sl.Patient)
                .WithMany(sl => sl.SymptomLogs)
                .HasForeignKey(sl => sl.PatientId);

            // SymptomLog > Symptom relationship
            builder.Entity<SymptomLog>()
                .HasOne(sl => sl.Symptom)
                .WithMany() 
                .HasForeignKey(s => s.SymptomId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes to avoid deleting symptoms when a log is deleted

            // ConditionSymptom > Condition relationship
            builder.Entity<ConditionSymptom>()
                .HasOne(cs => cs.Condition)
                .WithMany()
                .HasForeignKey(cs => cs.ConditionId);

            // ConditionSymptom > Symptom relationship
            builder.Entity<ConditionSymptom>()
                .HasOne(cs => cs.Symptom)
                .WithMany()
                .HasForeignKey(cs => cs.SymptomId);
            #endregion 
        }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<SymptomLog> SymptomLog { get; set; }
        public DbSet<Symptom> Symptom { get; set; }
    }
}
