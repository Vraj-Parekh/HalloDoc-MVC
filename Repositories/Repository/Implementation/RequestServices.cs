using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System.Globalization;

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
                //BusinessName = client.Firstname,---check if businesstype id or not then show name or address
                BusinessName = (request.Requesttypeid == 1) ? client.Firstname : client.City,
            };
            return data;
        }

        public List<AdminDashboardDTO> GetPatientdata(int requesttypeid, int status)
        {
            List<Request>? request = _context.Requests.Where(a => a.Status == status).Include(a => a.Requestclients).ToList();

            List<AdminDashboardDTO> admin = new List<AdminDashboardDTO>();

            foreach (Request req in request)
            {
                Requestclient? requestClient = req.Requestclients.First();
                AdminDashboardDTO? AdminDashboard = new AdminDashboardDTO
                {
                    RequestId = req.Requestid,
                    Name = requestClient.Firstname,
                    Dob = GenerateDateOfBirth(requestClient.Intyear, requestClient.Strmonth, requestClient.Intdate),
                    Requestor = (RequestTypeId)req.Requesttypeid + ", " + req.Firstname,
                    RequestedDate = req.Createddate,
                    Phone = req.Phonenumber,
                    Address = requestClient.Address,
                    Notes = requestClient.Notes,
                    RequestTypeId = req.Requesttypeid,
                };
                admin.Add(AdminDashboard);
            }
            return admin;
        }
        public void AssignCase(int requestId, string phyRegion, string phyId, string assignNote)
        {
            Request? request = GetRequest(requestId);
            if (request != null)
            {
                int physicianId = int.Parse(phyId);

                Requeststatuslog model = new();
                model.Requestid = requestId;
                model.Notes = assignNote;
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
    }
}