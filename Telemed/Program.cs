using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Telemed.Middleware;
using Telemed.Models;
using Telemed.Services;
using Telemed.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL timestamp behavior
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// == CRITICAL JWT CLAIM FIXES ==
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// -----------------------
// DbContext
// -----------------------
builder.Services.AddDbContext<TelemedDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------
// JWT Authentication
// -----------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!.Trim();

Console.WriteLine($"Secret Key Length: {secretKey.Length}");
Console.WriteLine($"Issuer: {jwtSettings["Issuer"]}");
Console.WriteLine($"Audience: {jwtSettings["Audience"]}");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.MapInboundClaims = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"]!.Trim(),

        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"]!.Trim(),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RequireExpirationTime = true,

        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.NameIdentifier
    };

    // Debug Events - Very Helpful
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"❌ Auth Failed: {context.Exception.GetType().Name} | {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = context.Principal?.FindFirst(ClaimTypes.Role)?.Value
                    ?? context.Principal?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            Console.WriteLine($"✅ Token Validated Successfully — UserId: {userId} | Role: {role}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"⚠️ Challenge Triggered on Path: {context.Request.Path}");
            Console.WriteLine($"   Error: {context.Error} | Description: {context.ErrorDescription}");
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value
                    ?? context.HttpContext.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
            Console.WriteLine($"🚫 Forbidden - Role mismatch | Current Role: {role ?? "NULL"}");
            return Task.CompletedTask;
        }
    };
});

// -----------------------
// Authorization
// -----------------------
builder.Services.AddAuthorization();

// -----------------------
// Controllers
// -----------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

// -----------------------
// Swagger with JWT 
// -----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "US Telemedicine API",
        Version = "v1",
        Description = "US Telemedicine REST API with JWT Authentication"
    });

    // Correct JWT Security Definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token in format: Bearer {your-token-here}"
    });

    // Global Security Requirement - Makes Authorize button visible
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


// -----------------------
// Services Registration
// -----------------------
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientAlertService, PatientAlertService>();
builder.Services.AddScoped<IPatientFollowUpService, PatientFollowUpService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IPatientSummaryService, PatientSummaryService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentDocumentService, AppointmentDocumentService>();
builder.Services.AddScoped<IAppointmentNoteService, AppointmentNoteService>();
builder.Services.AddScoped<IAppointmentStatusHistoryService, AppointmentStatusHistoryService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IConsultationDiagnosisService, ConsultationDiagnosisService>();
builder.Services.AddScoped<IConsultationNoteService, ConsultationNoteService>();
builder.Services.AddScoped<IConsultationPrescriptionService, ConsultationPrescriptionService>();
builder.Services.AddScoped<IEncounterService, EncounterService>();
builder.Services.AddScoped<IVideoSessionService, VideoSessionService>();
builder.Services.AddScoped<IVitalService, VitalService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IFilemasterService, FilemasterService>();
builder.Services.AddScoped<IProviderInfoService, ProviderInfoService>();
builder.Services.AddScoped<IProviderGroupService, ProviderGroupService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IConditionMasterService, ConditionMasterService>();
builder.Services.AddScoped<IPatientConditionService, PatientConditionService>();
builder.Services.AddScoped<IPatientTaskService, PatientTaskService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IRpmmonitoringService, RpmmonitoringService>();
builder.Services.AddScoped<ICareplanService, CareplanService>();
builder.Services.AddScoped<ISmartgoalService, SmartgoalService>();
builder.Services.AddScoped<IClinicalOrderService, ClinicalOrderService>();
builder.Services.AddScoped<IFollowupService, FollowupService>();
builder.Services.AddScoped<IPatientsSummaryService, PatientsSummaryService>();
builder.Services.AddScoped<ICarePatientsummaryService, CarePatientsummaryService>();
builder.Services.AddScoped<ICareteamService, CareteamService>();
builder.Services.AddScoped<ICareteampatientService, CareteampatientService>();
builder.Services.AddScoped<IAdminclaimService, AdminclaimService>();
builder.Services.AddScoped<ICareteamproviderService, CareteamproviderService>();
builder.Services.AddScoped<IClaimformService, ClaimformService>();

var app = builder.Build();

// -----------------------
// Middleware Pipeline - 
// -----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();      // ←= Must come before Authorization
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();   //  Last

app.MapControllers();
app.Run();