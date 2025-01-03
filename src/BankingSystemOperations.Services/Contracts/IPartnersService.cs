﻿using System.ComponentModel.DataAnnotations;
using BankingSystemOperations.Data.Dtos;
using BankingSystemOperations.Services.Results;

namespace BankingSystemOperations.Services.Contracts;

public interface IPartnersService
{
    Task<PaginatedList<PartnerDto>> GetPartnersAsync(int pageNumber, int pageSize);
    
    Task<PartnerDto> GetPartnerByIdAsync(Guid partnerId);
    
    Task<PaginatedList<MerchantDto>> GetPartnerMerchantsByIdAsync(Guid partnerId, int pageNumber, int pageSize);

    Task<string> PreparePartnersForCsvExportAsync();
    
    Task<ValidationResult> InsertPartnerAsync(PartnerDto dto);
    
    Task<ValidationResult> UpdatePartnerAsync(PartnerDto dto);
    
    Task<ValidationResult> DeletePartnerAsync(Guid partnerId);
}