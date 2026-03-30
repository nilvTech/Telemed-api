namespace Telemed.DTOs
{
    public class VideoSessionResponseDto
    {
        public int VideoSessionId { get; set; }

        public int EncounterId { get; set; }
        public int ProviderId { get; set; }
        public int PatientId { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string? CallStatus { get; set; }

        public string? RecordingUrl { get; set; }
        public string? VideoName { get; set; }
        public long? VideoSize { get; set; }
        public int? DurationSeconds { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}