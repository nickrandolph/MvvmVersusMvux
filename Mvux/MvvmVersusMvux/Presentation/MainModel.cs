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

        public IState<string> SearchText => State<string>.Empty(this);

        //public IListFeed<Result> Movies => 
        //    ListFeed.Async(
        //        async ct => (await _client.GetMovies(await SearchText??string.Empty, ct)).Results);

        public IListFeed<Result> Movies =>
            SearchText.SelectAsync(
                async (search, ct) => (await _client.GetMovies(search, ct)).Results).AsListFeed();

    }
}