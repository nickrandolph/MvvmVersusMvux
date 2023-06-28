using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Media.Protection.PlayReady;

namespace MvvmVersusMvux.Presentation
{
    public partial class MainViewModel : ObservableObject
    {
        private IApiClient _client;

        [ObservableProperty]
        private IncrementalObservableCollection<Result> _movies;

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

                var moviesResult = await _client.GetMovies(SearchText, 1);
                var movies = new IncrementalObservableCollection<Result>();
                movies.AddRange(moviesResult.Results);
                movies.TotalResults = moviesResult.TotalResults ?? 0;
                movies.PageNumber = 1;
                movies.LoadCallback = async () =>
                {
                    var moreMovies = await _client.GetMovies(SearchText, ++Movies.PageNumber);
                    return (moreMovies.Results, moreMovies.TotalResults ?? 0);
                };
                NoResults = movies.Count == 0;
                Movies = movies;
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

    public class IncrementalObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public Func<Task<(IEnumerable<T> Results, int TotalResults)>>? LoadCallback { get; set; }

        public bool HasMoreItems => Count < TotalResults;

        public int PageNumber { get; set; }

        public int TotalResults { get; set; }


        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
            => InternalLoadMoreItemsAsync().AsAsyncOperation();

        private async Task<LoadMoreItemsResult> InternalLoadMoreItemsAsync()
        {
            if (LoadCallback is null)
            {
                return new LoadMoreItemsResult();
            }
            var results = await LoadCallback();
            this.AddRange(results.Results);
            TotalResults = results.TotalResults;
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(TotalResults)));
            return new LoadMoreItemsResult((uint)results.Results.Count());
        }
    }
}