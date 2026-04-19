using Microsoft.EntityFrameworkCore;
using SymptomLogger.src.Data;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services.Interfaces;


namespace SymptomLogger.src.Services
{
    public class SeedDatabaseService : ISeedDatabaseService
    {
        private readonly ILogger<SeedDatabaseService> _logger;
        private readonly SymptomLoggerContext _context;
        private const string seedSource = "src/Data/SeedData/symptoms.json"; // ./symptoms.json
        public SeedDatabaseService( ILogger<SeedDatabaseService> logger, SymptomLoggerContext context)
        {
            _context = context;
            _logger = logger;
        }

        // Seed database
        #region SeedDatabase
        public async Task SeedDatabaseAsync()
        {
            using (var streamReader = new StreamReader(seedSource))
            {
                var jsonData = await streamReader.ReadToEndAsync();
                if (string.IsNullOrEmpty(jsonData))
                {
                    _logger.LogWarning("JSON data is empty.");
                    throw new Exception("JSON data is empty.");
                }
                var symptoms = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Symptom>>(jsonData);
                if (symptoms == null)
                {
                    _logger.LogWarning("Failed to deserialize symptoms from JSON.");
                    throw new ArgumentNullException("Failed to deserialize symptoms from JSON.");
                }
                // get first user to assign symptoms to (FOR TESTING PURPOSE)
                // ASSIGN TO ADMIN USER LATER
                var user = _context.User.FirstOrDefault();

                // assign user Id to each symptom
                foreach (var symptom in symptoms)
                {
                    symptom.UserId = user.Id;
                }

                await _context.AddRangeAsync(symptoms);
                _logger.LogInformation("Sucessfully seeded symptoms into database.");
                await _context.SaveChangesAsync();
            }
        }
        #endregion 

        //Clear database
        #region ClearDatabase
        public async Task ClearDatabaseAsync()
        {
            var symptoms = await _context.Symptom.ToListAsync();

            if (symptoms.Count > 0)
            {
                _context.Symptom.RemoveRange(symptoms);
                _logger.LogInformation("Cleared symptoms from database.");
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("No symptoms found to clear from database.");
            } 
        }
        #endregion 
    }
}
