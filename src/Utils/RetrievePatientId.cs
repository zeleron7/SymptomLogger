using System.Security.Claims;

namespace SymptomLogger.src.Utils
{
    public static class RetrievePatientId
    {
        public static string? GetPatientId(ClaimsPrincipal patient)
        {
            // validate for a valid GUID otherwise it retrieves the email as patient ID which causes issues in the service layer when trying to associate symptoms with patients
            return patient.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier && Guid.TryParse(c.Value, out _))
                .Select(c => c.Value).FirstOrDefault();
        }
    }
}
