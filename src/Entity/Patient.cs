using Microsoft.AspNetCore.Identity;
using SymptomLogger.src.Models;
using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.Entity
{
    public class Patient : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string Email { get; set; } = string.Empty;
        public int Age { get; set; }

        public List<Symptom> Symptoms { get; set; } = new List<Symptom>();
    }
}
