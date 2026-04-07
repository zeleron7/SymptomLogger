using SymptomLogger.src.Entity;
using System.Linq;

namespace SymptomLogger.src.Data
{
    public class ConditionSeeder
    {
        private readonly SymptomLoggerContext _context;
        private readonly ILogger<ConditionSeeder> _logger;
        private const string seedSource = "src/Data/SeedData/conditions.json"; // ./conditions.json
        public ConditionSeeder(SymptomLoggerContext context, ILogger<ConditionSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Seed conditions into database
        #region SeedConditions
        public void SeedConditions()
        {
            try
            {
                if (!string.IsNullOrEmpty(seedSource))
                {
                    using (var streamReader = new StreamReader(seedSource))
                    {
                        var jsonData = streamReader.ReadToEnd();
                        var conditions = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Condition>>(jsonData);

                        // Check if conditions already exist in the database to prevent duplicate seeding
                        if (_context.Condition.Any())
                        {
                            _logger.LogInformation("Conditions already exist in the database. Skipping seeding process.");
                            return;
                        }
                        _context.AddRange(conditions);
                        _context.SaveChanges();
                        _logger.LogInformation("Successfully seeded conditions into database.");
                    }
                }
                else
                {
                    _logger.LogWarning("Seed source for conditions is empty. Skipping seeding process.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding conditions into the database.");
                throw new Exception("An error occurred while seeding conditions into the database.", ex);
            }
        }
        #endregion 
    }
}
