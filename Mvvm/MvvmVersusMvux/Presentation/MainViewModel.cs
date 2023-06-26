using Windows.Media.Protection.PlayReady;

namespace MvvmVersusMvux.Presentation
{
    public partial class MainViewModel : ObservableObject
    {
        private IApiClient _client;

        [ObservableProperty]
        private IReadOnlyList<Result> _movies;

        [ObservableProperty]
        private string? _searchText;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotSearching))]
        private bool _isSearching;

        public bool IsNotSearching => !IsSearching;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private bool _noResults;

        public MainViewModel(
            IApiClient client)
        {
            _client = client;
        } 

        [RelayCommand]
        public async Task Search()
        {
            try
            {
                IsSearching = true;
                IsError = false;

                var moviesResult = await _client.GetMovies(SearchText);
                Movies = moviesResult.Results;
                NoResults = Movies.Count == 0;
            }
            catch
            {
                IsError = true;
            }
            finally
            {
                IsSearching = false;
            }
        }
    }
}