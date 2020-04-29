using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        RestService _restService;
        public MainPage()
        {
            InitializeComponent();
            string CurrentDateTime = DateTime.Now.ToString();
            _restService = new RestService();
        }

        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
            {
                WeatherForecastRoot weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                weatherData.Items[0].Main.ForecastTimeHours = ForecastTimeInHours();
                var dailyTemp = GetDailyTemperatures(weatherData);
                weatherData.Items[0].Main.TempMinRounded = GetMinDailyTemperature(dailyTemp);
                weatherData.Items[0].Main.TempMaxRounded = GetMaxDailyTemperature(dailyTemp);
                weatherData = SetDateAndTime(weatherData);

                BindingContext = weatherData;
                string TestTemp = weatherData.Items[0].Main.TemperatureRounded.ToString();

                //string weather = weatherData.LocationWeather.Visibility;
                //weatherData.WeatherIcon = WeatherDisplayIcon(weather);
            }
        }

        List<string> ForecastTimeInHours()
        {
            var result = new List<string>();
            var hourIncrementer = 3;
            for (int i = 0; i < 8; i++)
            {
                result.Add(DateTime.Now.AddHours(hourIncrementer).ToString("h tt"));
                hourIncrementer = hourIncrementer + 3;
            }

            return result;
        }

        WeatherForecastRoot SetDateAndTime(WeatherForecastRoot weatherData)
        {
            var result = weatherData;
            var dateTime = DateTime.Now;

            for (int i = 0; i < weatherData.Items.Count; i++)
            {
                result.Items[i].Main.DateAndTime = dateTime;
                dateTime = dateTime.AddHours(3);
            }
            return result;
        }

        List<int> GetDailyTemperatures(WeatherForecastRoot weatherData)
        {
            var result = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                result.Add(weatherData.Items[i].Main.TemperatureRounded);
            }
            result.Sort();

            return result;
        }

        int GetMinDailyTemperature(List<int> temperatureList)
        {
            return temperatureList[0];
        }

        int GetMaxDailyTemperature(List<int> temperatureList)
        {
            return temperatureList[7];
        }

        string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?q={_cityEntry.Text}";
            requestUri += "&units=imperial"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }

        public ImageSource WeatherDisplayIcon(string weather)
        {
            ImageSource image;
            string source = string.Empty;

            if (weather == "Thunderstorm")
                source = "Icons/rainthunder.png";
            else if (weather == "Drizzle" || weather == "Rain")
                source = "Icons/rain.png";
            else if (weather == "Snow")
                source = "Icons/snow.png";
            else if (weather == "Clear")
                source = "Icons/clear.png";
            else if (weather == "Clouds")
                source = "Icons/cloudy.png";
            else
                source = "Icons/warning.png";

            image = ImageSource.FromFile(source);
            return image;
        }
    }
}
