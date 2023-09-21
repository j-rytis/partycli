using partycli.Domain.Models;
using System.Collections.Generic;

namespace partycli.Domain
{
    public interface IConfigurationReader
    {
        List<ServerModel> GetServerList();

        List<LogModel> GetLog();
    }
}