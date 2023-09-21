using partycli.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace partycli.Domain
{
    public interface INordvpnClient
    {
        Task<List<ServerModel>> GetAllServersListAsync();

        Task<List<ServerModel>> GetAllServersByCountryListAsync(int countryId);

        Task<List<ServerModel>> GetAllServersByProtocolListAsync(int vpnProtocol);
    }
}
