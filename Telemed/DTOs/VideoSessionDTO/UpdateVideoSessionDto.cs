namespace Telemed.DTOs
{
    public class UpdateVideoSessionDto
    {
        public DateTime? StartTime { get; set; }

        public string? CallStatus { get; set; }

        public string? RecordingUrl { get; set; }
        public string? VideoName { get; set; }

        public byte[]? VideoData { get; set; }
        public long? VideoSize { get; set; }
        public int? DurationSeconds { get; set; }

        public DateTime? EndTime { get; set; }
    }
}