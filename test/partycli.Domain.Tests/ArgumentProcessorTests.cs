using AutoFixture;
using FluentAssert;
using Moq;
using partycli.Domain.Models;

namespace partycli.Domain.Tests
{
    public class ArgumentProcessorTests
    {
        private ArgumentProcessor _argumentProcessor;

        private Mock<INordvpnClient> _nordvpnClient;
        private Mock<IListPrinter> _listPrinter;
        private Mock<ILogging> _logging;
        private Mock<IValueStorage> _valueStorage;
        private Mock<IConfigurationReader> _configurationReader;
        private Mock<IConsoleWriter> _consoleWriter;

        private Fixture _fixture;

        private List<ServerModel> _expectedServerList;

        [SetUp]
        public void Setup()
        {
            _nordvpnClient = new Mock<INordvpnClient>();
            _listPrinter = new Mock<IListPrinter>();
            _logging = new Mock<ILogging>();
            _valueStorage = new Mock<IValueStorage>();
            _configurationReader = new Mock<IConfigurationReader>();
            _consoleWriter = new Mock<IConsoleWriter>();

            _fixture = new Fixture();

            _argumentProcessor = new ArgumentProcessor(_nordvpnClient.Object,
                                                       _listPrinter.Object,
                                                       _logging.Object,
                                                       _valueStorage.Object,
                                                       _configurationReader.Object,
                                                       _consoleWriter.Object);

            _expectedServerList = _fixture.Create<List<ServerModel>>();
        }

        [Test]
        public void ProcessArguments_ArgIsNotServerList_ReturnsStateNone()
        {
            // Arrange
            string[] argument = { _fixture.Create<string>() };

            // Act
            var response = _argumentProcessor.ProcessArgumentsAsync(argument).Result;

            // Assert
            response.ShouldBeEqualTo(Enums.State.none);
        }

        [Test]
        public void ProcessArguments_ArgIsServerList_ReturnsStateServerList()
        {
            // Arrange
            string[] argument = { "server_list" };

            // Act
            var response = _argumentProcessor.ProcessArgumentsAsync(argument).Result;

            // Assert
            response.ShouldBeEqualTo(Enums.State.server_list);
        }

        [Test]
        public void ProcessArguments_ArgsAreServerListAndLocal_AndStorageIsNotEmpty_DisplaysList()
        {
            // Arrange
            string[] argument = { "server_list", "--local" };

            _configurationReader.Setup(x => x.GetServerList()).Returns(_expectedServerList);

            // Act
            var response = _argumentProcessor.ProcessArgumentsAsync(argument);

            // Assert
            _listPrinter.Verify(x => x.Display(_expectedServerList), Times.Once);
        }

        [Test]
        public void ProcessArguments_ArgsAreServerListAndLocal_AndStorageIsEmpty_OutputsErrorMessage()
        {
            // Arrange
            string[] argument = { "server_list", "--local" };
            _expectedServerList = new List<ServerModel>();
            var message = _fixture.Create<string>();

            _configurationReader.Setup(x => x.GetServerList()).Returns(_expectedServerList);
            _consoleWriter.Setup(s => s.Output(message));

            // Act
            var response = _argumentProcessor.ProcessArgumentsAsync(argument);

            // Assert
            _consoleWriter.Verify(c => c.Output(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ProcessArguments_ArgsAreServerListAndFrance_ReturnsServerList()
        {
            // Arrange
            string[] argument = { "server_list", "--france" };

            _nordvpnClient.Setup(x => x.GetAllServersByCountryListAsync(74)).ReturnsAsync(_expectedServerList);

            // Act
            var response = _argumentProcessor.ProcessArgumentsAsync(argument);

            // Assert
            response.Equals(_expectedServerList);
        }

        [Test]
        public void ProcessArguments_ArgsAreServerListAndTcp_ReturnsServerList()
        {
            // Arrange
            string[] argument = { "server_list", "--tcp" };

            _nordvpnClient.Setup(x => x.GetAllServersByProtocolListAsync(3)).ReturnsAsync(_expectedServerList);

            // Act
            var response = _argumentProcessor.ProcessArgumentsAsync(argument);

            // Assert
            response.Equals(_expectedServerList);
        }
    }
}
