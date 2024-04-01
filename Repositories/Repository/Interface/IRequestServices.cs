﻿using Entities.Models;
using Entities.ViewModels;

namespace Repositories.Repository.Interface
{
    public interface IRequestServices
    {
        void AddCloseCaseData( int requestId);
        Task AddRequest(CreateRequestDTO model);
        Task<bool> AgreeWithAgreementAsync(int requestId);
        void AssignCase(int assignReqId, string phyRegion, string phyId, string assignNote);
        void ClearCase(int requestId);
        Task<List<Request>> GetAllRequests(int status);
        ViewDocumentList GetCloseCaseInfo(int requestId);
        object GetCount();
        ViewDocumentList GetDocumentData(int requestId);
        Task<List<Request>> GetFilteredRequests(int requesttypeid, int status, int pageIndex, int pageSize);
        SendAgreement GetMobileEmail(SendAgreement model, int requestId);
        List<AdminDashboardDTO> GetPatientdata(int requesttypeid, int status, int pageIndex, int pageSize, string searchQuery, int regionId, out int totalCount);
        Request? GetRequest(int requestId);
        ViewCaseDTO GetViewCase(int requestId);
        bool IsPatientPresent(string email);
        bool IsRequestPending(int requestId, string email);
        Task<bool> RejectAgreementAsync(int requestId, string message);
    }
}