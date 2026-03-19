using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Models;
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
            base.OnModelCreating(builder);

            // Custom configurations for Identity tables
            builder.Entity<Patient>(entity =>
            {
                entity.ToTable(name: "Patients");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
        }

        public DbSet<Symptom> Symptom { get; set; }
        public DbSet<Patient> Patient { get; set; }
    }
}
