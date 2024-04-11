﻿using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IHealthProfessionalsService
    {
        Task AddBusiness(EditBusinessDTO model);
        Task EditProfessional(int vendorId, EditBusinessDTO model);
        List<Healthprofessional> GetBusiness(int professionId);
        Task<EditBusinessDTO> GetHealthProfessionalInfo(int vendorId);
        Task<List<VendorsDTO>> GetFilteredHealthProfessionals(string searchVendor, int profesionType);
    }
}