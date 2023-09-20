using AutoFixture;
using FluentAssertions;
using Moq;
using partycli.Domain;

namespace partycli.Tests
{
    public class ArgumentProcessorTests
    {
        private ArgumentProcessor _argumentProcessor;

        private Mock<INordvpnService> _nordvpnService;
        private Mock<IListPrinter> _listPrinter;
        private Mock<ILogging> _logging;
        private Mock<IValueStorage> _valueStorage;

        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _nordvpnService = new Mock<INordvpnService>();
            _listPrinter = new Mock<IListPrinter>();
            _logging = new Mock<ILogging>();
            _valueStorage = new Mock<IValueStorage>();

            _fixture = new Fixture();

            _argumentProcessor = new ArgumentProcessor(_nordvpnService.Object, _listPrinter.Object, _logging.Object, _valueStorage.Object);
        }

        [Test]
        public void ProcessArguments_ArgIsNotServerList_ReturnsStateNone()
        {
            // Arrange
            string[] argument = { _fixture.Create<string>() };

            // Act
            var response = _argumentProcessor.ProcessArguments(argument);

            // Assert
            response.Should().Be(Enums.State.none);
        }

        [Test]
        public void ProcessArguments_ArgIsServerList_ReturnsStateServerList()
        {
            // Arrange
            string[] argument = { "server_list" };

            // Act
            var response = _argumentProcessor.ProcessArguments(argument);

            // Assert
            response.Should().Be(Enums.State.server_list);
        }

        [Test]
        public void ProcessArguments_ArgsAreServerListAndLocal_AndStorageIsNotEmpty_DisplaysList()
        {
            // Arrange
            string[] argument = { "server_list", "--local" };

            // Act
            var response = _argumentProcessor.ProcessArguments(argument);

            // Assert
            
        }

        [Test]
        public void ProcessArguments_ArgsAreServerListAndFrance_ReturnsServerList()
        {
            // Arrange
            string[] argument = { "server_list", "--france" };
            var expectedList = _fixture.Create<string>();

            _nordvpnService.Setup(x => x.GetAllServersByCountryListAsync(74)).ReturnsAsync(expectedList);

            // Act
            var response = _argumentProcessor.ProcessArguments(argument);

            // Assert
            response.Equals(expectedList);
        }

        [Test]
        public void ProcessArguments_ArgsAreServerListAndTcp_ReturnsServerList()
        {
            // Arrange
            string[] argument = { "server_list", "--tcp" };
            var expectedList = _fixture.Create<string>();

            _nordvpnService.Setup(x => x.GetAllServersByProtocolListAsync(3)).ReturnsAsync(expectedList);

            // Act
            var response = _argumentProcessor.ProcessArguments(argument);

            // Assert
            response.Equals(expectedList);
        }
    }
}
