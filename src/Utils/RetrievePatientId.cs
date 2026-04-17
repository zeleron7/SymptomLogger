using System.Security.Claims;

namespace SymptomLogger.src.Utils
{
    public static class RetrievePatientId
    {
        // Method to retrieve the patient ID from the claims of the authenticated user
        public static string GetPatientId(ClaimsPrincipal patient)
        {
            var claim = patient.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                throw new InvalidOperationException("Patient ID claim not found.");
            }
            return claim.Value;
        }
    }
}
