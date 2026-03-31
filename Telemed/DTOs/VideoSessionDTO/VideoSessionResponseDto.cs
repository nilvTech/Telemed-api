namespace Telemed.DTOs
{
    public class VideoSessionResponseDto
    {
        public long VideoSessionId { get; set; }

        public long EncounterId { get; set; }
        public long ProviderId { get; set; }
        public long PatientId { get; set; }

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