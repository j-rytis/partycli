using System.Linq;
using System;
using partycli.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using partycli.Enums;

namespace partycli.Domain
{
    public interface IArgumentProcessor
    {
        Task<State> ProcessArgumentsAsync(string[] args);
    }

    public class ArgumentProcessor : IArgumentProcessor
    {
        private readonly INordvpnClient _nordvpnService;
        private readonly IListPrinter _listPrinter;
        private readonly ILogging _logger;
        private readonly IValueStorage _valueStorage;
        private readonly IConfigurationReader _configurationReader;
        private readonly IConsoleWriter _consoleWriter;

        public ArgumentProcessor(INordvpnClient nordvpnService,
                                 IListPrinter listPrinter,
                                 ILogging logger,
                                 IValueStorage valueStorage,
                                 IConfigurationReader configurationReader,
                                 IConsoleWriter consoleWriter)
        {
            _nordvpnService = nordvpnService;
            _listPrinter = listPrinter;
            _logger = logger;
            _valueStorage = valueStorage;
            _configurationReader = configurationReader;
            _consoleWriter = consoleWriter;
        }

        public async Task<State> ProcessArgumentsAsync(string[] args)
        {
            var currentState = await ProcessMainArgumentAsync(args);

            for (int i = 1; i < args.Length; i++)
            {
                if (currentState == State.server_list)
                {
                    ProcessSubArgument(args[i]);
                }
            }

            return currentState;
        }

        private async Task<State> ProcessMainArgumentAsync(string[] args)
        {
            if (args[0] != "server_list")
            {
                return State.none;
            }

            if (args.Count() == 1)
            {
                var serverList = await _nordvpnService.GetAllServersListAsync();
                StoreDisplayAndLogServerList(serverList);
            }

            return State.server_list;
        }

        private void ProcessSubArgument(string args)
        {
            if (args == "--local")
            {
                var serverList = _configurationReader.GetServerList();

                if (serverList.Any())
                {
                    _listPrinter.Display(serverList);
                }
                else
                {
                    _consoleWriter.Output("Error: There are no server data in local storage");
                }
            }
            else
            {
                var serverList = GetAllServersByCountryOrProtocol(args);
                StoreDisplayAndLogServerList(serverList);
            }
        }

        private List<ServerModel> GetAllServersByCountryOrProtocol(string arg)
        {
            var argument = arg.Substring(2).ToLower();
            List<ServerModel> serverList;

            if (Enum.IsDefined(typeof(Protocol), argument))
            {
                var protocolId = (int)Enum.Parse(typeof(Protocol), argument);
                serverList = _nordvpnService.GetAllServersByProtocolListAsync(protocolId).Result;
            }

            else
            {
                var countryId = (int)Enum.Parse(typeof(Country), argument);
                serverList = _nordvpnService.GetAllServersByCountryListAsync(countryId).Result;
            }

            return serverList;
        }

        private void StoreDisplayAndLogServerList(List<ServerModel> serverListModel)
        {
            var serverList = JsonConvert.SerializeObject(serverListModel);

            _valueStorage.Store("serverlist", serverList, false);
            _logger.Log("Saved new server list: " + serverList);
            _listPrinter.Display(serverListModel);
        }
    }
}
