using System.Text.Json;
using System.Web;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.Interfaces;

namespace WisdomOfCrowndBets.Infrastructure.Repositories
{
    public class OddsApi : IOddsApi
    {
        private static readonly HttpClient _client = new HttpClient();

        public async Task<List<EventDTO>> GetOddsAsync(ApiOddsConfig apiGetOdds)
        {
            List<EventDTO> result = new List<EventDTO>();
            try
            {
                var baseUrl = apiGetOdds.ApiGetOddsUrl.Replace("{sport}", apiGetOdds.ApiGetOddsSport);

                var uriBuilder = new UriBuilder(baseUrl);
                var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);

                queryParams.Add(apiGetOdds.ApiGetOddsHeaderApiKey, apiGetOdds.ApiGetOddsApiKey);
                queryParams.Add(apiGetOdds.ApiGetOddsHeaderRegion, apiGetOdds.ApiGetOddsRegion);
                queryParams.Add(apiGetOdds.ApiGetOddsHeaderMarkets, apiGetOdds.ApiGetOddsMarkets);

                uriBuilder.Query = queryParams.ToString();
                var finalUrl = uriBuilder.Uri;

                using var request = new HttpRequestMessage(HttpMethod.Get, finalUrl);

                HttpResponseMessage response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();                                      
                    result = JsonSerializer.Deserialize<List<EventDTO>>(responseContent);                 
                    return result;
                }
                else
                {
                    Console.WriteLine($"Error calling API: {response.StatusCode} - {response.ReasonPhrase}");
                    return result; 
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HTTP Request Exception: {e.Message}");
                return result;
            }
        }
    }
}
