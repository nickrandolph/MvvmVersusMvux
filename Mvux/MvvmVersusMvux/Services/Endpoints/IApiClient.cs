using Refit;

namespace MvvmVersusMvux.Services.Endpoints
{
    [Headers("Content-Type: application/json")]
    public interface IApiClient
    {
        [Get("/api/weatherforecast")]
        Task<ApiResponse<IImmutableList<WeatherForecast>>> GetWeather(CancellationToken cancellationToken = default);

        //[Get("/3/search/movie?query={searchTerm}")]
        //Task<SearchResults> GetMovies(string searchTerm, CancellationToken cancellationToken = default);


        [Get("/3/search/movie?query={searchTerm}&page={page}")]
        Task<SearchResults> GetMovies(string searchTerm, int page, CancellationToken cancellationToken = default);
    }
}