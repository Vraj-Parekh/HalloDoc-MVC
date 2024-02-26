using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Repositories.Repository.Interface;

namespace Repositories.Repository.Implementation
{
    public class RequestClientServices : IRequestClientServices
    {
        private readonly HalloDocDbContext _context;

        public RequestClientServices(HalloDocDbContext _context)
        {
            this._context = _context;
        }
        public Requestclient GetClient(int requestId)
        {
            Requestclient? requestClient = _context.Requestclients.First(a => a.Requestid == requestId);


            return requestClient;
        }
        public void UpdateCase(ViewCaseDTO data)
        {
            Requestclient? clientData = _context.Requestclients.FirstOrDefault(a => a.Requestid == 54);

            clientData.Notes = data.PatientNotes;
            clientData.Firstname = data.FirstName;
            clientData.Lastname = data.LastName;
            clientData.Intdate = data.DateOfBirth.Day;
            clientData.Strmonth = data.DateOfBirth.ToString("MMM");
            clientData.Intyear = data.DateOfBirth.Year;
            clientData.Phonenumber = data.PhoneNumber;
            clientData.Email = data.Email;

            _context.Requestclients.Update(clientData);
            _context.SaveChanges();
        }
    }
}
