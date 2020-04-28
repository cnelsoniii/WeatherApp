using System;
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
                BindingContext = weatherData;
                //string weather = weatherData.LocationWeather.Visibility;
                //weatherData.WeatherIcon = WeatherDisplayIcon(weather);
            }
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
