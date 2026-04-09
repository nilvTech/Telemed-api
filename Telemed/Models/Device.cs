using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Device
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string SerialNumber { get; set; } = null!;

    public string DeviceId { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public string ModelNumber { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public byte[]? DevicePicture { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
