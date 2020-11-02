using System;
using System.Threading.Tasks;

using OpenWeatherAPI;

namespace IoTHubDevice.Services
{
    public class WeatherService
    {
        private const string API_KEY = "95d87895159e129ee6a8a89f6e1e72f9";

        public Query Data { get; set; }

        private string city = string.Empty;
        private OpenWeatherAPI.API client;

        public WeatherService()
        {
            client = new OpenWeatherAPI.API(API_KEY);
        }

        public async Task UpdateData(string inputCity = "")
        {
            await Task.Run(() =>
            {
                try
                {
                    city = string.IsNullOrWhiteSpace(inputCity) ? city : inputCity;

                    if (string.IsNullOrWhiteSpace(city))
                    {
                        return;
                    }

                    Data = client.Query(city);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });
        }
    }
}