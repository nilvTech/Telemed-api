using Telemed.Models;
using Telemed.DTOs;

namespace Telemed.Mappers
{
    public static class VideoSessionMapper
    {
        public static Videosession ToEntity(CreateVideoSessionDto dto)
        {
            return new Videosession
            {
                Encounterid = dto.EncounterId,
                Providerid = dto.ProviderId,
                Patientid = dto.PatientId,

                Starttime = dto.StartTime,
                Callstatus = dto.CallStatus ?? "Scheduled",

                Recordingurl = dto.RecordingUrl,
                Videoname = dto.VideoName,
                Videodata = dto.VideoData,
                Videosize = dto.VideoSize,
                Durationseconds = dto.DurationSeconds,

                Createdat = DateTime.UtcNow
            };
        }

        public static void UpdateEntity(Videosession entity, UpdateVideoSessionDto dto)
        {
            entity.Starttime = dto.StartTime ?? entity.Starttime;
            entity.Endtime = dto.EndTime ?? entity.Endtime;
            entity.Callstatus = dto.CallStatus ?? entity.Callstatus;

            entity.Recordingurl = dto.RecordingUrl ?? entity.Recordingurl;
            entity.Videoname = dto.VideoName ?? entity.Videoname;

            if (dto.VideoData != null)
                entity.Videodata = dto.VideoData;

            entity.Videosize = dto.VideoSize ?? entity.Videosize;
            entity.Durationseconds = dto.DurationSeconds ?? entity.Durationseconds;

            entity.Updatedat = DateTime.UtcNow;
        }

        public static VideoSessionResponseDto ToDto(Videosession entity)
        {
            return new VideoSessionResponseDto
            {
                VideoSessionId = entity.Videosessionid,
                EncounterId = entity.Encounterid,
                ProviderId = entity.Providerid,
                PatientId = entity.Patientid,

                StartTime = entity.Starttime,
                EndTime = entity.Endtime,

                CallStatus = entity.Callstatus,

                RecordingUrl = entity.Recordingurl,
                VideoName = entity.Videoname,
                VideoSize = entity.Videosize,
                DurationSeconds = entity.Durationseconds,

                CreatedAt = entity.Createdat,
                UpdatedAt = entity.Updatedat
            };
        }
    }
}