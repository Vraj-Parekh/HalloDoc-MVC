using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public bool IsRequestPending(int requestId, string email)
        {
            return _context.Requests.FirstOrDefault(a => a.Requestid == requestId)?.Status == (int)RequestStatus.Pending;
        }

        public async Task<bool> AgreeWithAgreementAsync(int requestId)
        {
            Request? request = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
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

            Request? request = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
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
            //TempData["requestId"] = requestId;
            Request? request = _context.Requests.FirstOrDefault(a => a.Requestid == requestId);

            if (request is null) return null;

            Requestclient? client = requestClientServices.GetClient(requestId);
            if (client is null) return null;

            ViewCaseDTO? data = new ViewCaseDTO()
            {
                ConfirmationNumber = request.Confirmationnumber,
                PatientNotes = client.Notes,
                FirstName = client.Firstname,
                LastName = client.Lastname,
                DateOfBirth = GenerateDateOfBirth(client.Intyear, client.Strmonth, client.Intdate),
                PhoneNumber = client.Phonenumber,
                Email = client.Email,
                Region = client.City,
                //BusinessName = client.Firstname,---check if businesstype id or not then show name or address
                BusinessName = (request.Requesttypeid == 1) ? client.Firstname : client.City,
            };
            return data;
        }

        public List<AdminDashboardDTO> GetPatientdata(int requesttypeid,int status)
        {
            List<Request>? r = _context.Requests.Where(a =>( a.Requesttypeid == requesttypeid || requesttypeid == 5) && a.Status == status).Include(a => a.Requestclients).ToList();
            List<AdminDashboardDTO> admin = new List<AdminDashboardDTO>();
            foreach (Request req in r)
            {
                Requestclient? rc = req.Requestclients.First();
                AdminDashboardDTO? AdminDashboard = new AdminDashboardDTO
                {
                    RequestId = req.Requestid,
                    Name = rc.Firstname,
                    Dob = GenerateDateOfBirth(rc.Intyear, rc.Strmonth, rc.Intdate),
                    Requestor = (RequestTypeId)req.Requesttypeid + ", " + req.Firstname,
                    RequestedDate = req.Createddate,
                    Phone = req.Phonenumber,
                    Address = rc.Address,
                    Notes = rc.Notes,
                    RequestTypeId = req.Requesttypeid,
                    //ChatWith =,
                };
                admin.Add(AdminDashboard);
            }
            return admin;

        }
    }
}