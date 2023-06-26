using System.Text.Json.Serialization;

namespace MvvmVersusMvux.DataContracts
{
    /// <summary>
    /// A Weather Forecast for a specific date
    /// </summary>
    /// <param name="Date">Gets the Date of the Forecast.</param>
    /// <param name="TemperatureC">Gets the Forecast Temperature in Celsius.</param>
    /// <param name="Summary">Get a description of how the weather will feel.</param>
    public record WeatherForecast(DateOnly Date, double TemperatureC, string? Summary)
    {
        /// <summary>
        /// Gets the Forecast Temperature in Fahrenheit
        /// </summary>
        public double TemperatureF => 32 + (TemperatureC * 9 / 5);
    }


    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public record Result(
        [property: JsonPropertyName("adult")] bool? Adult,
        [property: JsonPropertyName("backdrop_path")] string BackdropPath,
        [property: JsonPropertyName("genre_ids")] IReadOnlyList<int?> GenreIds,
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("original_language")] string OriginalLanguage,
        [property: JsonPropertyName("original_title")] string OriginalTitle,
        [property: JsonPropertyName("overview")] string Overview,
        [property: JsonPropertyName("popularity")] double? Popularity,
        [property: JsonPropertyName("poster_path")] string PosterPath,
        [property: JsonPropertyName("release_date")] string ReleaseDate,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("video")] bool? Video,
        [property: JsonPropertyName("vote_average")] double? VoteAverage,
        [property: JsonPropertyName("vote_count")] int? VoteCount
    );

    public record SearchResults(
        [property: JsonPropertyName("page")] int? Page,
        [property: JsonPropertyName("results")] IReadOnlyList<Result> Results,
        [property: JsonPropertyName("total_pages")] int? TotalPages,
        [property: JsonPropertyName("total_results")] int? TotalResults
    );


}