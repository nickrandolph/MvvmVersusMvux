using Refit;

namespace MvvmVersusMvux.Services.Endpoints
{
    [Headers("Content-Type: application/json")]
    public interface IApiClient
    {
        [Get("/api/weatherforecast")]
        Task<ApiResponse<IImmutableList<WeatherForecast>>> GetWeather(CancellationToken cancellationToken = default);
        [Get("/3/search/movie?query={searchTerm}")]
        //[Headers("authorization: bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwNDQ5MjhlNjNjMzIxMmZjODZlNzNjNGM5NjhkNTAyNSIsInN1YiI6IjRlN2MwN2M4N2I5YWExNTc2MjAwMDBjZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.oWSS5sf3dlXvdGveX2Wx7NBywhVhAqT_OC2DzcgAWPI")]
        Task<SearchResults> GetMovies(string searchTerm, CancellationToken cancellationToken = default);
    }
}