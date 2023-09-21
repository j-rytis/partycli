using System.Collections.Generic;
using Newtonsoft.Json;
using partycli.Domain;
using partycli.Domain.Models;

namespace partycli
{
    public class ConfigurationReader : IConfigurationReader
    {
        public List<ServerModel> GetServerList()
        {
            var serverList = Properties.Settings.Default.serverlist;

            var serverlistModel = JsonConvert.DeserializeObject<List<ServerModel>>(serverList);

            return serverlistModel;
        }
        
        public List<LogModel> GetLog()
        {
            var log = Properties.Settings.Default.log;

            var logModel = JsonConvert.DeserializeObject<List<LogModel>>(log);

            return logModel;
        }
    }
}
