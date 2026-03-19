using Microsoft.AspNetCore.Identity;
using SymptomLogger.src.Models;
using System.ComponentModel.DataAnnotations;

namespace SymptomLogger.src.Entity
{
    public class Patient : IdentityUser
    {
        [Key]
        public int PatiendId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        // Patient can have multiple symptoms
        //public List<Symptom> Symptoms { get; set; }
    }
}
