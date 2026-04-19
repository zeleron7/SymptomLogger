using System.Security.Claims;

namespace SymptomLogger.src.Utils
{
    public static class RetrieveUserId
    {
        // Method to retrieve the user ID from the claims of the authenticated user
        public static string GetUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                throw new InvalidOperationException("User ID claim not found.");
            }
            return claim.Value;
        }
    }
}
