using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.Entity
{
    public class Patient : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        // Patient can have multiple symptoms
        public ICollection<Symptom> Symptoms { get; set; } = new List<Symptom>();

        // Patient can have multiple symptom logs
        public ICollection<SymptomLog> SymptomLogs { get; set; } = new List<SymptomLog>();
    }
}
