namespace MvvmVersusMvux.Presentation
{
    public partial record MainModel
    {
        private IApiClient _client;

        public MainModel(
            IApiClient client)
        {
            _client = client;
        }

        public IState<string> SearchText => State<string>.Value(this, () => "xxx");

        // public IListFeed<Result> Movies =>
        //ListFeed.Async(
        //    async ct => (await _client.GetMovies(await SearchText ?? string.Empty, ct)).Results);

        public IState<int> TotalResults => State<int>.Value(this, () => 0);

        public IListFeed<Result> Movies =>
            ListFeed.AsyncPaginated<Result>(
                async (pageRequest, ct) =>
                {
                    var movies = await _client.GetMovies(await SearchText ?? string.Empty, (int)pageRequest.Index + 1, ct);
                    _ = TotalResults.Set(movies.TotalResults, ct);
                    return movies.Results;
                });

        //public IListFeed<Result> Movies =>
        //    SearchText.SelectAsync(
        //        async (search, ct) => (await _client.GetMovies(search, ct)).Results).AsListFeed();

    }
}