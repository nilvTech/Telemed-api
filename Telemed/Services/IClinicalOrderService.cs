// Services/Interfaces/IClinicalOrderService.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Services.Interfaces;

public interface IClinicalOrderService
{
    // ===== ClinicalMaster =====
    Task<ClinicalMasterResponseDto> CreateMasterAsync(
        CreateClinicalMasterDto dto);
    Task<IEnumerable<ClinicalMasterResponseDto>> GetAllMastersAsync();
    Task<ClinicalMasterResponseDto?> GetMasterByIdAsync(long id);
    Task<IEnumerable<ClinicalMasterResponseDto>> GetMastersByTypeAsync(
        string ordertype);
    Task<IEnumerable<ClinicalMasterResponseDto>> SearchMastersAsync(
        string keyword);
    Task<ClinicalMasterResponseDto?> UpdateMasterAsync(
        long id, UpdateClinicalMasterDto dto);
    Task<bool> DeleteMasterAsync(long id);

    // ===== ClinicalOrder =====
    Task<ClinicalOrderResponseDto> CreateOrderAsync(CreateClinicalOrderDto dto);
    Task<IEnumerable<ClinicalOrderResponseDto>> GetAllOrdersAsync();
    Task<ClinicalOrderResponseDto?> GetOrderByIdAsync(long id);
    Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByPatientIdAsync(
        long patientId);
    Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByProviderIdAsync(
        long providerInfoId);
    Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByEncounterIdAsync(
        int encounterId);
    Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByStatusAsync(
        string status);
    Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByTypeAsync(
        string ordertype);
    Task<IEnumerable<ClinicalOrderResponseDto>> GetUrgentOrdersAsync();
    Task<ClinicalOrderResponseDto?> UpdateOrderAsync(
        long id, UpdateClinicalOrderDto dto);
    Task<ClinicalOrderResponseDto?> UpdateOrderStatusAsync(
        long id, ClinicalOrderStatusUpdateDto dto);
    Task<bool> DeleteOrderAsync(long id);

    // ===== FILE INTEGRATION (OPTIONAL BUT REAL WORLD) =====
   
}