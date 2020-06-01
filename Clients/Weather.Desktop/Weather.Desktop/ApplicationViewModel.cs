using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Weather.Desktop.Annotations;

namespace Weather.Desktop
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private WeatherInfo selectedWeather;
        private string selectedCity;
        private readonly WeatherService _weatherService;

        public ObservableCollection<string> Cities { get; set; }

        private RelayCommand selectCityCommand;
        public RelayCommand SelectCityCommand
        {
            get
            {
                return selectCityCommand ??= new RelayCommand(obj =>
                {
                    var tomorrowWeather = _weatherService.GetTomorrowWeatherAsync((string) obj);
                    tomorrowWeather.GetAwaiter().OnCompleted(() => SelectedWeather = tomorrowWeather.Result);
                });
            }
        }

        public WeatherInfo SelectedWeather
        {
            get => selectedWeather;
            set
            {
                selectedWeather = value;
                OnPropertyChanged(nameof(SelectedWeather));
            }
        }

        public string SelectedCity
        {
            get => selectedCity;
            set
            {
                SelectCityCommand.Execute(value);
                selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        public ApplicationViewModel()
        {
            _weatherService = new WeatherService();
            Cities = new ObservableCollection<string>(_weatherService.GetAllCitiesAsync().GetAwaiter().GetResult());
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
