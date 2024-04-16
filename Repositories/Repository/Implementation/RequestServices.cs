using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Utilities;
using Repositories.Repository.Interface;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using static NPOI.HSSF.Util.HSSFColor;

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

        private string GetConfirmationNumber(string city, string lastname, string firstname, string count)
        {
            string regionAbr = city.Substring(0, 2);
            string date = DateTime.Now.ToString("dd");
            string month = DateTime.Now.ToString("MM");
            string last = lastname.Substring(0, 2);
            string first = firstname.Substring(0, 2);
            string requestCount = count;

            return regionAbr + date + month + last + first + requestCount;
        }

        public Request? GetRequest(int requestId)
        {
            return _context.Requests.FirstOrDefault(a => a.Requestid == requestId);
        }

        public bool IsPatientPresent(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        public async Task AddRequest(CreateRequestDTO model)
        {
            string count = _context.Requests.Where(a => a.Createddate.Date == DateTime.Now.Date).Count().ToString("0000");
            Request? request = new Request()
            {
                Requesttypeid = (int)RequestTypeId.Patient,

                Firstname = model.FirstName,
                Lastname = model.LastName,
                Phonenumber = model.PhoneNumber,
                Email = model.Email,
                Status = (int)RequestStatus.Unassigned,
                Confirmationnumber = GetConfirmationNumber(model.City, model.LastName, model.FirstName, count),
                Createddate = DateTime.Now,
                Isdeleted = false,
                Isurgentemailsent = false,
            };

            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();
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
                ActiveCount = _context.Requests.Where(r => r.Status == 2 || r.Status == 5 || r.Status == 6).Count(),
                ConcludeCount = _context.Requests.Where(r => r.Status == 18).Count(),
                ToCloseCount = _context.Requests.Where(r => r.Status == 3 || r.Status == 21 || r.Status == 8).Count(),
                UnpaidCount = _context.Requests.Where(r => r.Status == 19).Count()
            };
            return count;
        }

        public async Task<List<Request>> GetFilteredRequests(int requesttypeid, int status, int pageIndex, int pageSize)
        {
            Dictionary<int, int[]> statusMap = new()
            {
                {1, new int[1]{ 1} },//new
                {2, new int[1]{ 16} },//pending
                {3, new int[3]{ 2,5,6} },//active
                {4, new int[1]{ 18} },//conclude
                {5, new int[3]{ 3,21,8} },//to close
                {6, new int[1]{ 19} }//unpaid
            };

            List<Request>? request = await _context.Requests
               .Where(a => statusMap[status]
               .Contains(a.Status) && (requesttypeid == 5 || a.Requesttypeid == requesttypeid))
               .Include(a => a.Requestclients)
               .Skip(pageIndex > 0 ? (pageIndex - 1) * pageSize : 0)
               .Take(pageSize)
               .ToListAsync();

            return request;
        }

        public async Task<List<Request>> GetAllRequests(int status)
        {
            Dictionary<int, int[]> statusMap = new()
            {
                {1, new int[1]{ 1} },//new
                {2, new int[1]{ 16} },//pending
                {3, new int[3]{ 2,5,6} },//active
                {4, new int[1]{ 18} },//conclude
                {5, new int[3]{ 3,21,8} },//to close
                {6, new int[1]{ 19} }//unpaid
            };

            List<Request>? request = await _context.Requests
               .Where(a => statusMap[status].Contains(a.Status))
               .ToListAsync();

            return request;
        }

        public List<AdminDashboardDTO> GetPatientdata(int requesttypeid, int status, int pageIndex, int pageSize, string searchQuery, int regionId, out int totalCount)
        {
            Dictionary<int, int[]> statusMap = new()
            {
                {1, new int[1]{ 1} },//new
                {2, new int[1]{ 16} },//pending
                {3, new int[3]{ 2,5,6} },//active
                {4, new int[1]{ 18} },//conclude
                {5, new int[3]{ 3,21,8} },//to close
                {6, new int[1]{ 19} }//unpaid
            };

            IQueryable<Request>? query = _context.Requests
                .Where(a => statusMap[status].Contains(a.Status) && (requesttypeid == 5 || a.Requesttypeid == requesttypeid))
                .Include(a => a.Requestclients);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLower();
                query = query.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchQuery)
                                                                || rc.Lastname.ToLower().Contains(searchQuery))
                                       || a.Firstname.ToLower().Contains(searchQuery)
                                       || a.Lastname.ToLower().Contains(searchQuery));
            }

            if (regionId > 0)
            {
                query = query.Where(a => a.Requestclients.Any(rc => rc.Regionid == regionId));
            }

            totalCount = query.Count();

            List<Request>? request = query
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
                    LastName = requestClient.Lastname ?? "",
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
                    Region = requestClient.Region?.Name,
                    Status = status,
                };
                admin.Add(AdminDashboard);
            }
            return admin;
        }

        public void AssignCase(int requestId, string phyRegion, string phyId, string notes)//here notes is tranfer notes
        {
            Request? request = GetRequest(requestId);
            if (request != null)
            {
                int physicianId = int.Parse(phyId);

                Requeststatuslog model = new()
                {
                    Requestid = requestId,
                    Notes = notes,
                    Status = 16, //pending
                    Createddate = DateTime.Now,
                    Physicianid = physicianId,
                    Transtophysicianid = physicianId,
                };


                request.Status = 16; //pending
                request.Physicianid = physicianId;

                _context.Requeststatuslogs.Add(model);
                _context.Requests.Update(request);

                _context.SaveChanges();
            }
        }

        public ViewDocumentList GetDocumentData(int requestId)
        {
            Request? request = _context.Requests
                .Where(a => a.Requestid == requestId)
                .Include(a => a.Requestclients)
                .Include(a => a.Requestwisefiles)
                .FirstOrDefault();
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
                    if (file.Isdeleted == false)
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
            Requestclient? requestClient = requestClientServices.GetClient(requestId);
            if (request is not null && requestClient is not null)
            {
                model.PhoneNumber = request.Phonenumber;
                model.Email = request.Email;
                return model;
            }
            return null;
        }

        public ViewDocumentList GetCloseCaseInfo(int requestId)
        {
            Request? request = GetRequest(requestId);
            Requestclient? requestClient = requestClientServices.GetClient(requestId);

            if (request is not null && requestClient is not null)
            {
                ViewDocumentList? data = GetDocumentData(requestId);
                data.FirstName = requestClient.Firstname;
                data.LastName = requestClient.Lastname;
                data.DateOfBirth = GenerateDateOfBirth(requestClient.Intyear, requestClient.Strmonth, requestClient.Intdate);
                data.PhoneNumber = requestClient.Phonenumber;
                data.Email = requestClient.Email;

                return data;
            }
            return null;
        }

        public void AddCloseCaseData(int requestId)
        {
            Request? request = GetRequest(requestId);
            if (request != null)
            {
                Requeststatuslog requeststatuslog = new()
                {
                    Requestid = requestId,
                    Status = 19, //unpaid
                    Createddate = DateTime.Now,
                };

                Requestclosed requestClosed = new()
                {
                    Requestid = requestId,
                    Requeststatuslog = requeststatuslog,
                };

                request.Status = 19; //unpaid


                _context.Requeststatuslogs.Add(requeststatuslog);
                _context.Requestcloseds.Add(requestClosed);
                _context.Requests.Update(request);
                _context.SaveChanges();
            }
        }

        public async Task<List<PatientRecordsDTO>> GetPatientRecord(int userId)
        {
            List<Request>? patientRecords = await _context.Requests
                .Where(a => a.Userid == userId)
                .Include(a => a.Requestclients)
                .Include(a => a.Physician)
                .ToListAsync();

            if (patientRecords.Count > 0)
            {
                List<PatientRecordsDTO> modelList = new List<PatientRecordsDTO>();

                foreach (var item in patientRecords)
                {
                    PatientRecordsDTO model = new PatientRecordsDTO();
                    model.ClientName = item.Firstname + ", " + item.Lastname;
                    model.CreatedDate = item.Createddate.ToString("MMM d,yyyy");
                    model.Confirmation = item.Confirmationnumber;
                    model.ProviderName = (item.Physician != null) ? item.Physician.Firstname : "Not Assigned";
                    model.ConcludedDate = item.Createddate.ToString("MMM d,yyyy"); ;
                    model.Status = item.Status;
                    model.RequestId = item.Requestid;

                    modelList.Add(model);
                }

                return modelList;
            }
            return new List<PatientRecordsDTO>();
        }

        public async Task<Pagination<SearchRecordsDTO>> GetfilteredSearchRecords(string patientName, string email, string phoneNumber, int requestStatus, int requestType, DateTime fromDateOfService, DateTime toDateOfService, string providerName, int page, int itemsPerPage)
        {
            IQueryable<Request>? query = _context.Requests
                .Include(a => a.Requestclients)
                .Include(a => a.Physician)
                .Include(a => a.Requestnotes)
                .AsQueryable();

            if (!string.IsNullOrEmpty(patientName))
            {
                patientName = patientName.ToLower().Trim();
                query = query.Where(a=>a.Requestclients.Any(rc=>rc.Firstname.ToLower().Contains(patientName)));
            }

            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower().Trim();
                query = query.Where(a => a.Email.ToLower().Contains(email));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = phoneNumber.ToLower().Trim();
                query = query.Where(a => a.Phonenumber.ToLower().Contains(phoneNumber));
            }

            if (requestStatus != 0)
            {
                query = query.Where(a => a.Status == requestStatus);
            }

            if (requestType != 0)
            {
                query = query.Where(a => a.Requesttypeid == requestType);
            }

            if (fromDateOfService != DateTime.MinValue && toDateOfService != DateTime.MinValue)
            {
                query = query.Where(a => a.Createddate.Date >= fromDateOfService.Date && a.Accepteddate.Value.Date <= toDateOfService.Date);
            }

            if (!string.IsNullOrEmpty(providerName))
            {
                providerName = providerName.ToLower().Trim();
                query = query.Where(a => a.Physician.Firstname.ToLower().Contains(providerName));
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (page < 1) page = 1;

            int skip = (page - 1) * itemsPerPage;

            List<Request>? searchedRecords = await query
                .Where(a => a.Isdeleted != true)
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();

            List<SearchRecordsDTO> modelList = new List<SearchRecordsDTO>();

            foreach (var item in searchedRecords)
            {
                Requestclient? requestClient = item.Requestclients.FirstOrDefault();
                Physician? physician = item.Physician;
                Requestnote? requestNotes = item.Requestnotes.FirstOrDefault();

                SearchRecordsDTO model = new SearchRecordsDTO()
                {
                    PatientName = item.Firstname ?? "-",
                    Requestor = item.Requesttypeid,
                    DateofService = item.Createddate.ToString("MMM dd,yyy"),
                    CloseCaseDate = item.Createddate.ToString("MMM dd,yyy"),//pending
                    Email = item.Email ?? "-",
                    PhoneNumber = item.Phonenumber ?? "-",
                    Address = requestClient?.City ?? "-",
                    Zip = requestClient?.Zipcode ?? "-",
                    RequestStatus = item.Status,
                    Physician = physician?.Firstname ?? "-",
                    PhysicianNote = requestNotes?.Physiciannotes ?? "-",
                    CancelledByProviderNote = requestNotes?.Physiciannotes ?? "-",
                    AdminNote = requestNotes?.Adminnotes ?? "-",
                    PatientNote = requestNotes?.Physiciannotes ?? "-",
                    RequestId = item.Requestid,
                };
                modelList.Add(model);
            }

            return new Pagination<SearchRecordsDTO>
            {
                Data = modelList,
                TotalPages = totalPages,
                CurrentPage = page
            };
        }

        public async Task DeletePatientRecord(int requestId)
        {
            Request? record = _context.Requests.Where(a => a.Requestid == requestId).FirstOrDefault();
            if (record is not null)
            {
                record.Isdeleted = true;
                _context.Update(record);
                await _context.SaveChangesAsync();
            }
        }

        public List<ProviderDashboardDTO> GetProviderDashboardData(int requesttypeid, int status, int pageIndex, int pageSize, string searchQuery, out int totalCount)
        {
            Dictionary<int, int[]> statusMap = new()
            {
                {1, new int[1]{ 1} },//new
                {2, new int[1]{ 16} },//pending
                {3, new int[3]{ 2,5,6} },//active
                {4, new int[1]{ 18} },//conclude
            };

            IQueryable<Request>? query = _context.Requests
                .Where(a => statusMap[status].Contains(a.Status) && (requesttypeid == 5 || a.Requesttypeid == requesttypeid))
                .Include(a => a.Requestclients);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLower();
                query = query.Where(a => a.Requestclients.Any(rc => rc.Firstname.ToLower().Contains(searchQuery)
                                                                || rc.Lastname.ToLower().Contains(searchQuery))
                                       || a.Firstname.ToLower().Contains(searchQuery)
                                       || a.Lastname.ToLower().Contains(searchQuery));
            }

            totalCount = query.Count();

            List<Request>? request = query
                .Skip(pageIndex > 0 ? (pageIndex - 1) * pageSize : 0)
                .Take(pageSize)
                .ToList();


            List<ProviderDashboardDTO> provider = new List<ProviderDashboardDTO>();
            foreach (Request req in request)
            {
                Requestclient? requestClient = req.Requestclients.First();
                ProviderDashboardDTO? ProviderDashboard = new ProviderDashboardDTO()
                {
                    RequestId = req.Requestid,
                    FirstName = requestClient.Firstname,
                    LastName = requestClient.Lastname ?? "",
                    Phone = req.Phonenumber,
                    ClientPhone = requestClient.Phonenumber,
                    Address = requestClient.City,
                    RequestTypeId = req.Requesttypeid,
                    Status = status,
                };
                provider.Add(ProviderDashboard);
            }
            return provider;
        }
    }
}