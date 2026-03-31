namespace Telemed.DTOs
{
    public class CreateVideoSessionDto
    {
        public int EncounterId { get; set; }
        public long ProviderId { get; set; }
        public long PatientId { get; set; }

        public DateTime? StartTime { get; set; }

        public string? CallStatus { get; set; }  // Scheduled / Ringing / InProgress / Completed / Cancelled

        public string? RecordingUrl { get; set; }
        public string? VideoName { get; set; }

        public byte[]? VideoData { get; set; }
        public long? VideoSize { get; set; }
        public int? DurationSeconds { get; set; }
    }
}