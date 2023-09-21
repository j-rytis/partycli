using AutoFixture;
using Moq;
using NUnit.Framework.Internal;
using partycli.Domain;
using partycli.Domain.Models;

namespace partycli.Tests
{
    public class LoggingTests
    {
        private Logging _logger;

        private Mock<IValueStorage> _valueStorage;
        private Mock<IConfigurationReader> _configurationReader;

        private static readonly Fixture _fixture = new();

        private string _messageToLog;

        [SetUp]
        public void Setup()
        {
            _valueStorage = new Mock<IValueStorage>();
            _configurationReader = new Mock<IConfigurationReader>();

            _logger = new Logging(_valueStorage.Object, _configurationReader.Object);

            _messageToLog = _fixture.Create<string>();
            _valueStorage.Setup(s => s.Store("log", _messageToLog, false));
        }

        [Test]
        public void Log_GivenLogsAreSavedInConfiguration_StoresMessage()
        {
            // Arrange
            var expectedLog = _fixture.Create<List<LogModel>>();
            _configurationReader.Setup(x => x.GetLog()).Returns(expectedLog);

            // Act
            _logger.Log(_messageToLog);

            // Assert
            _valueStorage.Verify(storage => storage.Store(It.IsAny<string>(), It.IsAny<string>(), false), Times.Once);
        }

        [Test]
        public void Log_GivenNoLogsAreSavedInConfiguration_StoresMessage()
        {
            // Arrange
            var expectedLog = new List<LogModel>();
            _configurationReader.Setup(x => x.GetLog()).Returns(expectedLog);

            // Act
            _logger.Log(_messageToLog);

            // Assert
            _valueStorage.Verify(storage => storage.Store(It.IsAny<string>(), It.IsAny<string>(), false), Times.Once);
        }
    }
}