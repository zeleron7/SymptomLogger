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
    public class SymptomLoggerContext : IdentityDbContext<User>
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
            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            #endregion

            // Custom configurations for application tables
            #region Relationship configurations

            // Symptom > User relationship
            builder.Entity<Symptom>()
                .HasOne(s => s.User)
                .WithMany(s => s.Symptoms)
                .HasForeignKey(s => s.UserId);

            // SymptomLog > User relationship
            builder.Entity<SymptomLog>()
                .HasOne(sl => sl.User)
                .WithMany(sl => sl.SymptomLogs)
                .HasForeignKey(sl => sl.UserId);

            // SymptomLog > Condition relationship
            builder.Entity<SymptomLog>()
                .HasOne(sl => sl.Condition)
                .WithMany()
                .HasForeignKey(sl => sl.ConditionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes to avoid deleting conditions when a log is deleted

            // SymptomLogEntry > SymptomLog relationship
            builder.Entity<SymptomLogEntry>()
                .HasOne(sle => sle.SymptomLog)
                .WithMany(sl => sl.SymptomLogEntries)
                .HasForeignKey(sle => sle.SymptomLogId);

            // SymptomLogEntry > Symptom relationship
            builder.Entity<SymptomLogEntry>()
                .HasOne(sle => sle.Symptom)
                .WithMany() 
                .HasForeignKey(sle => sle.SymptomId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes to avoid deleting symptoms when a log entry is deleted

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

            // Seed initial data for Conditions
            #region Condition seeding
            /*builder.Entity<Condition>().HasData(
                new Condition { ConditionId = 1, Name = "Fibromyalgia", System = "Musculoskeletal", IcdCode = "M79.7" },
                new Condition { ConditionId = 2, Name = "Irritable Bowel Syndrome (IBS)", System = "Digestive", IcdCode = "K58" },
                new Condition { ConditionId = 3, Name = "Migraine", System = "Neurological", IcdCode = "G43" }
            );*/
            #endregion 
        }
        public DbSet<User> User { get; set; }
        public DbSet<SymptomLog> SymptomLog { get; set; }
        public DbSet<Symptom> Symptom { get; set; }
        public DbSet<Condition> Condition { get; set; }
    }
}
