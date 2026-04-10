using Moq;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Middleware;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Repositories;
using SymptomLogger.src.Services;
using Xunit;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services.Interfaces;
using AutoMapper;
using SymptomLogger.src.DTO.SymptomLog;

namespace SymptomLogger.Tests.Services
{
    public class SymptomLogServiceTests
    {
        // Mocks for dependencies
        private readonly Mock<ISymptomLogRepository> _repoMock;
        private readonly Mock<ILogger<SymptomLogService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        
        private readonly ISymptomLogService _service;

        public SymptomLogServiceTests()
        {
            _repoMock = new Mock<ISymptomLogRepository>();
            _loggerMock = new Mock<ILogger<SymptomLogService>>();
            _mapperMock = new Mock<IMapper>();

            _service = new SymptomLogService(
                _repoMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task AddSymptomLog_Should_Call_Add_On_Repository()
        {
            // Arrange
            var symptomLog = new CreateSymptomLogDTO
            {
                SymptomId = 123,
                Notes = "Headache",
                Severity = 5,
                DurationMinutes = 60,
                StressLevel = 3,
                SleepHours = 6,
                isFlare = true,
            };

            var patientId = "patient-123";

            var mappedEntity = new SymptomLog
            {
                SymptomId = symptomLog.SymptomId,
                Notes = symptomLog.Notes,
                Severity = symptomLog.Severity,
                DurationMinutes = (int)symptomLog.DurationMinutes, // remove explicit cast for nullable dtos ? 
                StressLevel = (int)symptomLog.StressLevel,
                SleepHours = (int)symptomLog.SleepHours,
                isFlare = symptomLog.isFlare,
                PatientId = patientId
            };

            // Act
            await _service.AddSymptomLogAsync(symptomLog, patientId);

            // Assert
            _repoMock.Verify(
                repo => repo.AddSymptomLog(mappedEntity, patientId),
                Times.Once
            );
        }
    }
}
