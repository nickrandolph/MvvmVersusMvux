using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.Media.Protection.PlayReady;

namespace MvvmVersusMvux.Presentation
{
    public partial class MainViewModel : ObservableObject
    {
        private IApiClient _client;

        [ObservableProperty]
        private IncrementalList<Result> _movies;

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

                var moviesResult = await _client.GetMovies(SearchText,1);
                Movies = new IncrementalList<Result>();
                Movies.AddRange(moviesResult.Results);
                Movies.HasMoreItems = moviesResult.TotalResults > moviesResult.Results.Count;
                Movies.PageNumber = 1;
                Movies.LoadCallback = async () =>
                {
                    var movies = await _client.GetMovies(SearchText, ++Movies.PageNumber);
                    return (movies.Results, Movies.Count + movies.Results.Count < movies.TotalResults);
                };
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

    public class IncrementalList<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public Func<Task<(IEnumerable<T> Results, bool MoreItems)>>? LoadCallback { get; set; }

        public bool HasMoreItems { get; set; }
        public int PageNumber { get; set; }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return InternalLoadMoreItemsAsync((int)count).AsAsyncOperation();
            //return Task.Run(async () =>
            //{
            //    if (LoadCallback is null)
            //    {
            //        return new LoadMoreItemsResult();
            //    }
            //    var results = await LoadCallback();
            //    this.AddRange(results.Results);
            //    HasMoreItems = results.MoreItems;
            //    return new LoadMoreItemsResult((uint)results.Results.Count());
            //}).AsAsyncOperation();
        }

        private async Task<LoadMoreItemsResult> InternalLoadMoreItemsAsync(int count)
        {
            if (LoadCallback is null)
            {
                return new LoadMoreItemsResult();
            }
            var results = await LoadCallback();
            this.AddRange(results.Results);
            HasMoreItems = results.MoreItems;
            return new LoadMoreItemsResult((uint)results.Results.Count());
        }
    }
}