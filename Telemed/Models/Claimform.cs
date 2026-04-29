using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Claimform
{
    public long Claimformid { get; set; }

    public string? Patientname { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Insuranceplan { get; set; }

    public string? Policynumber { get; set; }

    public string? Insuredname { get; set; }

    public DateOnly? Dateofillness { get; set; }

    public string? Referringprovider { get; set; }

    public string? Diagnosiscode { get; set; }

    public DateOnly? Servicedate { get; set; }

    public string? Servicecptcode { get; set; }

    public string? Servicedescription { get; set; }

    public decimal? Serviceamount { get; set; }

    public decimal? Totalamount { get; set; }

    public DateTime? Createdat { get; set; }

    public long? Createdby { get; set; }
}
