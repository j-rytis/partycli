using System.Net.Http;
using System.Threading.Tasks;

namespace partycli.Domain
{
    public interface INordvpnService
    {
        Task<string> GetAllServersListAsync();

        Task<string> GetAllServersByCountryListAsync(int countryId);

        Task<string> GetAllServersByProtocolListAsync(int vpnProtocol);
    }

    public class NordvpnService : INordvpnService
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string uri = "https://api.nordvpn.com/v1/servers";

        public async Task<string> GetAllServersListAsync()
        {
            var response = await GetResponseFromUri(uri);
            return response;
        }

        public async Task<string> GetAllServersByCountryListAsync(int countryId)
        {
            var fullUri = $"{uri}?filters[servers_technologies][id]=35&filters[country_id]={countryId}";
            var response = await GetResponseFromUri(fullUri);
            return response;
        }

        public async Task<string> GetAllServersByProtocolListAsync(int vpnProtocol)
        {
            var fullUri = $"{uri}?filters[servers_technologies][id]={vpnProtocol}";
            var response = await GetResponseFromUri(fullUri);
            return response;
        }

        private async Task<string> GetResponseFromUri(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
