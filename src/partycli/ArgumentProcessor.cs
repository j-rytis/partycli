using partycli.Domain;
using System.Linq;
using System;
using partycli.Enums;

namespace partycli
{
    public interface IArgumentProcessor
    {
        State ProcessArguments(string[] args);
    }

    public class ArgumentProcessor : IArgumentProcessor
    {
        private readonly INordvpnService _nordvpnService;
        private readonly IListPrinter _listPrinter;
        private readonly ILogging _logger;
        private readonly IValueStorage _valueStorage;

        public ArgumentProcessor(INordvpnService nordvpnService, IListPrinter listPrinter, ILogging logger, IValueStorage valueStorage)
        {
            _nordvpnService = nordvpnService;
            _listPrinter = listPrinter;
            _logger = logger;
            _valueStorage = valueStorage;
        }

        public State ProcessArguments(string[] args)
        {
            var currentState = ProcessMainArgument(args);

            for (int i = 1; i < args.Length; i++)
            {
                if (currentState == State.server_list)
                {
                    ProcessSubArgument(args[i]);
                }
            }

            return currentState;
        }

        private State ProcessMainArgument(string[] args)
        {
            if (args[0] != "server_list")
            {
                return State.none;
            }

            if (args.Count() == 1)
            {
                var serverList = _nordvpnService.GetAllServersListAsync().Result;
                StoreDisplayAndLogServerList(serverList);
            }

            return State.server_list;
        }

        private void ProcessSubArgument(string args)
        {
            if (args == "--local")
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.serverlist))
                {
                    _listPrinter.Display(Properties.Settings.Default.serverlist);
                }
                else
                {
                    Console.WriteLine("Error: There are no server data in local storage");
                }
            }
            else
            {
                var serverList = GetAllServersByCountryOrProtocol(args);
                StoreDisplayAndLogServerList(serverList);
            }
        }

        private string GetAllServersByCountryOrProtocol(string arg)
        {
            var argument = arg.Substring(2).ToLower();
            var serverList = string.Empty;

            if (Enum.IsDefined(typeof(Protocol), argument))
            {
                var protocolId = (int)Enum.Parse(typeof(Protocol), argument);
                serverList = _nordvpnService.GetAllServersByProtocolListAsync(protocolId).Result;
            }

            else if (Enum.IsDefined(typeof(Country), argument))
            {
                var countryId = (int)Enum.Parse(typeof(Country), argument);
                serverList = _nordvpnService.GetAllServersByCountryListAsync(countryId).Result;
            }

            return serverList;
        }

        private void StoreDisplayAndLogServerList(string serverList)
        {
            _valueStorage.Store("serverlist", serverList, false);
            _logger.Log("Saved new server list: " + serverList);
            _listPrinter.Display(serverList);
        }
    }
}
