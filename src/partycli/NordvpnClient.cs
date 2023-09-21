using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using partycli.Domain;
using partycli.Domain.Models;

namespace partycli
{
    public class NordvpnClient : INordvpnClient
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string uri = "https://api.nordvpn.com/v1/servers";

        public async Task<List<ServerModel>> GetAllServersListAsync()
        {
            var response = await GetResponseFromUri(uri);
            return response;
        }

        public async Task<List<ServerModel>> GetAllServersByCountryListAsync(int countryId)
        {
            var fullUri = $"{uri}?filters[servers_technologies][id]=35&filters[country_id]={countryId}";
            var response = await GetResponseFromUri(fullUri);
            return response;
        }

        public async Task<List<ServerModel>> GetAllServersByProtocolListAsync(int vpnProtocol)
        {
            var fullUri = $"{uri}?filters[servers_technologies][id]={vpnProtocol}";
            var response = await GetResponseFromUri(fullUri);
            return response;
        }

        private async Task<List<ServerModel>> GetResponseFromUri(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            var serverlist = JsonConvert.DeserializeObject<List<ServerModel>>(responseString);

            return serverlist;
        }
    }
}
