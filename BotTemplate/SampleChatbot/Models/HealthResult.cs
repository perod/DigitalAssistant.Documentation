namespace SampleChatbot.Models
{
    public class HealthResult
    {
        public HealthStatus Status { get; set; }
        public string Message { get; set; }
    }

    public enum HealthStatus
    {
        Ok,
        Warning,
        Error
    }
}
