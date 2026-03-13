using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SymptomLogger.Models;

namespace SymptomLogger.Data
{
    public class SymptomLoggerContext : DbContext
    {
        public SymptomLoggerContext (DbContextOptions<SymptomLoggerContext> options)
            : base(options)
        {
        }

        public DbSet<Symptom> Symptom { get; set; }
    }
}
