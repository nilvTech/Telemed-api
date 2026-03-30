// Services/PaymentService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PaymentService : IPaymentService
{
    private readonly TelemedDbContext _context;

    public PaymentService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto)
    {
        // Validate Appointment exists
        var appointmentExists = await _context.Appointments
            .AnyAsync(a => a.Appointmentid == dto.Appointmentid);
        if (!appointmentExists)
            throw new ArgumentException($"Appointment with ID {dto.Appointmentid} does not exist.");

        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException($"Patient with ID {dto.Patientid} does not exist.");

        // Validate Provider exists
        var providerExists = await _context.Providers
            .AnyAsync(p => p.Providerid == dto.Providerid);
        if (!providerExists)
            throw new ArgumentException($"Provider with ID {dto.Providerid} does not exist.");

        // Validate Amount
        if (dto.Amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero.");

        var entity = PaymentMapper.ToEntity(dto);
        _context.Payments.Add(entity);
        await _context.SaveChangesAsync();

        // Reload with related data for full response
        var created = await _context.Payments
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync(p => p.Paymentid == entity.Paymentid);

        return PaymentMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetAllAsync()
    {
        var list = await _context.Payments
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Appointment)
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PaymentMapper.ToResponseDto);
    }

    public async Task<PaymentResponseDto?> GetByIdAsync(int id)
    {
        var payment = await _context.Payments
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync(p => p.Paymentid == id);

        if (payment == null) return null;
        return PaymentMapper.ToResponseDto(payment);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var list = await _context.Payments
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Appointment)
            .Where(p => p.Patientid == patientId)
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PaymentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetByProviderIdAsync(int providerId)
    {
        var list = await _context.Payments
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Appointment)
            .Where(p => p.Providerid == providerId)
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PaymentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetByAppointmentIdAsync(int appointmentId)
    {
        var list = await _context.Payments
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Appointment)
            .Where(p => p.Appointmentid == appointmentId)
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PaymentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetByStatusAsync(string status)
    {
        var list = await _context.Payments
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Appointment)
            .Where(p => p.Status!.ToLower() == status.ToLower())
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PaymentMapper.ToResponseDto);
    }

    public async Task<PaymentResponseDto?> UpdateAsync(int id, UpdatePaymentDto dto)
    {
        var entity = await _context.Payments
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync(p => p.Paymentid == id);

        if (entity == null) return null;

        // Validate Amount if provided
        if (dto.Amount.HasValue && dto.Amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero.");

        // Validate Status value if provided
        var validStatuses = new[] { "Pending", "Paid", "Failed" };
        if (!string.IsNullOrEmpty(dto.Status) &&
            !validStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"Invalid status. Allowed values: Pending, Paid, Failed.");

        PaymentMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return PaymentMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Payments.FindAsync(id);
        if (entity == null) return false;

        _context.Payments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}