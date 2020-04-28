using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;

namespace WeatherApp
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<WeatherForecastRoot> GetWeatherData(string query)
        {
            //WeatherData weatherData = null;
            //try
            //{
            //    var response = await _client.GetAsync(query);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var content = await response.Content.ReadAsStringAsync();
            //        weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("\t\tERROR {0}", ex.Message);
            //}

            //return weatherData;

            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(query);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherForecastRoot>(json);
            }
        }
    }
}
