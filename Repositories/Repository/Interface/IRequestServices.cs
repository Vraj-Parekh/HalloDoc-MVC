using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Repositories.Repository.Interface
{
    public interface IRequestServices
    {
        void AddCloseCaseData( int requestId);
        Task AddRequest(CreateRequestDTO model);
        Task<bool> AgreeWithAgreementAsync(int requestId);
        void AssignCase(int assignReqId, string phyRegion, string phyId, string assignNote);
        void ClearCase(int requestId);
        Task DeletePatientRecord(int requestId);
        Task<List<Request>> GetAllRequests(int status);
        ViewDocumentList GetCloseCaseInfo(int requestId);
        object GetCount();
        ViewDocumentList GetDocumentData(int requestId);
        Task<List<Request>> GetFilteredRequests(int requesttypeid, int status, int pageIndex, int pageSize);
        Task<Pagination<SearchRecordsDTO>> GetfilteredSearchRecords(string patientName, string email, string phoneNumber, int requestStatus, int requestType, DateTime fromDateOfService, DateTime toDateOfService, string providerName,int page,int itemsPerPage);
        SendAgreement GetMobileEmail(SendAgreement model, int requestId);
        List<AdminDashboardDTO> GetPatientdata(int requesttypeid, int status, int pageIndex, int pageSize, string searchQuery, int regionId, out int totalCount);
        Task<List<PatientRecordsDTO>> GetPatientRecord(int userId);
        Request? GetRequest(int requestId);
        ViewCaseDTO GetViewCase(int requestId);
        bool IsPatientPresent(string email);
        bool IsRequestPending(int requestId, string email);
        Task<bool> RejectAgreementAsync(int requestId, string message);
    }
}