namespace FootballTennis.Web.Models
{
    public sealed class ErrorModel
    {
        public int? StatusCode { get; init; }
        public string? TraceId { get; init; }
        public string? Message { get; init; }

        public bool ShowTraceId()
        {
            return !string.IsNullOrWhiteSpace(TraceId);
        }
    }
}
