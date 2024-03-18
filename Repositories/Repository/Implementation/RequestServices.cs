using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using Repositories.Repository.Interface;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Repositories.Repository.Implementation
{
    public class RequestServices : IRequestServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IRequestStatusLogServices requestStatusLogServices;
        private readonly IRequestClientServices requestClientServices;

        public RequestServices(HalloDocDbContext _context, IRequestStatusLogServices requestStatusLogServices, IRequestClientServices requestClientServices)
        {
            this._context = _context;
            this.requestStatusLogServices = requestStatusLogServices;
            this.requestClientServices = requestClientServices;
        }

        public Request? GetRequest(int requestId)
        {
            return _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
        }

        public bool IsRequestPending(int requestId, string email)
        {
            return _context.Requests.FirstOrDefault(a => a.Requestid == requestId)?.Status == (int)RequestStatus.Pending;
        }

        public async Task<bool> AgreeWithAgreementAsync(int requestId)
        {
            Request? request = GetRequest(requestId);
            if (request is null)
                return false;

            request.Status = (int)RequestStatus.Active;
            _context.Requests.Update(request);

            await requestStatusLogServices.AddRequestStatusLogAsync(request, RequestStatus.Active);


            int changes = await _context.SaveChangesAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }

        //Reject the Agreement
        public async Task<bool> RejectAgreementAsync(int requestId, string message)
        {

            Request? request = GetRequest(requestId);
            if (request is null)
                return false;

            request.Status = (int)RequestStatus.ToClosed;
            _context.Requests.Update(request);

            await requestStatusLogServices.AddRequestStatusLogAsync(request, RequestStatus.ToClosed, message);
            int changes = await _context.SaveChangesAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }
        private DateTime GenerateDateOfBirth(int? year, string? month, int? date)
        {

            DateTime finalDate = new DateTime(year ?? 1900, DateTime.ParseExact(month ?? "January", "MMMM", CultureInfo.CurrentCulture).Month, date ?? 01);
            return finalDate;
        }
        public ViewCaseDTO GetViewCase(int requestId)
        {
            Request? request = GetRequest(requestId);

            if (request is null)
                return null;

            Requestclient? client = requestClientServices.GetClient(requestId);
            if (client is null)
                return null;

            ViewCaseDTO? data = new ViewCaseDTO()
            {
                ConfirmationNumber = request.Confirmationnumber,
                PatientNotes = client.Notes,
                FirstName = client.Firstname,
                LastName = client.Lastname,
                DateOfBirth = GenerateDateOfBirth(client.Intyear, client.Strmonth, client.Intdate),
                PhoneNumber = client.Phonenumber ?? "",
                Email = client.Email ?? "",
                Region = client.City,
                RequestId = requestId,
                BusinessName = (request.Requesttypeid == 1) ? client.Firstname : client.City,
                RequestStatusType = request.Status,
            };
            return data;
        }

        public Object GetCount()
        {
            RequestCount? count = new RequestCount
            {
                NewCount = _context.Requests.Where(r => r.Status == 1).Count(),
                PendingCount = _context.Requests.Where(r => r.Status == 16).Count(),
                ActiveCount = _context.Requests.Where(r => r.Status == 3 || r.Status == 5 || r.Status == 6).Count(),
                ConcludeCount = _context.Requests.Where(r => r.Status == 18).Count(),
                ToCloseCount = _context.Requests.Where(r => r.Status == 3 || r.Status == 21 || r.Status == 8).Count(),
                UnpaidCount = _context.Requests.Where(r => r.Status == 19).Count()
            };
            return count;
        }
        public List<AdminDashboardDTO> GetPatientdata(int requesttypeid, int status, int pageIndex, int pageSize, out int totalCount)
        {
            Dictionary<int, int[]> statusMap = new()
            {
                {1, new int[1]{ 1} },
                {2, new int[1]{ 16} },
                {3, new int[3]{ 3,5,6} },
                {4, new int[1]{ 18} },
                {5, new int[3]{ 3,21,8} },
                {6, new int[1]{ 19} }
            };

          totalCount = _context.Requests.Count(a => statusMap[status].Contains(a.Status) && (requesttypeid == 5 || requesttypeid == a.Requesttypeid));

            List<Request>? request = _context.Requests
                .Where(a => statusMap[status]
                .Contains(a.Status) && (requesttypeid == 5 || requesttypeid == a.Requesttypeid))
                .Include(a => a.Requestclients)
                .Skip(pageIndex > 0 ? (pageIndex - 1) * pageSize : 0)
                .Take(pageSize)
                .ToList();


            List<AdminDashboardDTO> admin = new List<AdminDashboardDTO>();
            foreach (Request req in request)
            {
                Requestclient? requestClient = req.Requestclients.First();
                AdminDashboardDTO? AdminDashboard = new AdminDashboardDTO
                {
                    RequestId = req.Requestid,
                    FirstName = requestClient.Firstname,
                    LastName =requestClient.Lastname,
                    Dob = GenerateDateOfBirth(requestClient.Intyear, requestClient.Strmonth, requestClient.Intdate),
                    Requestor = (RequestTypeId)req.Requesttypeid + ", " + req.Firstname,
                    RequestedDate = req.Createddate,
                    Phone = req.Phonenumber,
                    ClientPhone = requestClient.Phonenumber,
                    Email = req.Email,
#warning:add address in requestCLient table
                    Address = requestClient.City,
                    PhysicianName = _context.Physicians.Where(a => a.Physicianid == req.Physicianid).Select(phy => phy.Firstname).FirstOrDefault(),
                    Notes = _context.Requeststatuslogs.Where(a => a.Requestid == req.Requestid).OrderBy(a => a.Createddate).LastOrDefault()?.Notes,
                    RequestTypeId = req.Requesttypeid,
                    Status = status,
                };
                admin.Add(AdminDashboard);
            }
            return admin;
        }
        public void AssignCase(int requestId, string phyRegion, string phyId, string notes)
        {
            Request? request = GetRequest(requestId);
            if (request != null)
            {
                int physicianId = int.Parse(phyId);

                Requeststatuslog model = new();
                model.Requestid = requestId;
                model.Notes = notes;
                model.Status = 16; //pending
                model.Createddate = DateTime.Now;
                model.Physicianid = physicianId;

                request.Status = 16; //pending
                request.Physicianid = physicianId;

                _context.Requeststatuslogs.Add(model);
                _context.Requests.Update(request);

                _context.SaveChanges();
            }
        }

        public ViewDocumentList GetDocumentData(int requestId)
        {
            Request? request = _context.Requests.Where(a => a.Requestid == requestId).Include(a => a.Requestclients).Include(a => a.Requestwisefiles).FirstOrDefault();
            if (request is null)
            {
                return null;
            }

            List<FileData> data = new();
            ICollection<Requestwisefile>? files = request.Requestwisefiles;
            ICollection<Requestclient>? name = request.Requestclients;

            if (files is not null)
            {
                foreach (Requestwisefile file in files)
                {
                    FileData FileDataList = new()
                    {
                        FileName = file.Filename,
                        CreatedDate = file.Createddate,
                        DocumentId = file.Requestwisefileid
                    };
                    data.Add(FileDataList);
                }
            }
            ViewDocumentList doc = new()
            {
                Name = request.Firstname + request.Lastname,
                ConfirmationNumber = request.Confirmationnumber,
                Document = data,
                RequestId = request.Requestid,
            };
            return doc;
        }

        public void ClearCase(int requestId)
        {
            Request? request = GetRequest(requestId);

            if (request is not null)
            {
                request.Status = 12;//Clear
                request.Modifieddate = DateTime.Now;

                Requeststatuslog? requestStatusLog = new Requeststatuslog()
                {
                    Status = 12, //clear
                    Createddate = DateTime.Now,
                    Requestid = requestId,
                };

                _context.Requests.Update(request);
                _context.Requeststatuslogs.Add(requestStatusLog);
                _context.SaveChanges();
            }
        }

        public SendAgreement GetMobileEmail(SendAgreement model, int requestId)
        {
            Request? request = GetRequest(requestId);
            if (request is not null)
            {
                model.PhoneNumber = request.Phonenumber;
                model.Email = request.Email;
                return model;
            }
            return null;
        }
    }
}