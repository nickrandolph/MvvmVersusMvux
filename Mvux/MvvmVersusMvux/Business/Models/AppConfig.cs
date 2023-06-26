namespace MvvmVersusMvux.Business.Models
{
    public record AppConfig
    {
        public string? Environment { get; init; }

        public string? AccessToken { get; set; }
    }
}