namespace Telemed.DTOs;

public class CreatePatientDto
{
    public string FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }  
    public string Address { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string MaritalStatus { get; set; } = default!;
    public string MRN { get; set; } = default!;
}