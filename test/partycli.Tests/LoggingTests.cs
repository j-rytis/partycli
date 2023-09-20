using AutoFixture;
using Moq;
using NUnit.Framework.Internal;
using partycli.Domain;

namespace partycli.Tests
{
    public class LoggingTests
    {
        private Logging _logger;

        private Mock<IValueStorage> _valueStorage;

        //private static readonly Fixture _fixture = new();

        [SetUp]
        public void Setup()
        {
            _valueStorage = new Mock<IValueStorage>();

            _logger = new Logging(_valueStorage.Object);
        }

        [Test]
        public void Log_GivenAnyString_StoresMessage()
        {
            // Arrange
            var messageToLog = "testas";  //_fixture.Create<string>();

            // Act
            _logger.Log(messageToLog);

            // Assert
            _valueStorage.Verify(storage => storage.Store("log", messageToLog, false), Times.Once);
        }
    }
}