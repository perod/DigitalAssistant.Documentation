using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleChatBot
{
    public interface IWeatherService
    {
        Task<string> GetForecastFromService(string place);
    }
    [Serializable]
    public class WeatherService : IWeatherService
    {
        public async Task<string> GetForecastFromService(string place)
        {
            string apiKey = "c965ad5f173f596bedbd455662713833";
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", place, apiKey));
            var response = await httpClient.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<JObject>();
                return result.GetValue("weather").ToObject<JArray>().First<JToken>()["description"].ToString();
            }
            return "Don't know that place";
        }
    }
}
