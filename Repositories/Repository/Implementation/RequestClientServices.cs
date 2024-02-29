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
            Requestclient? clientData = GetClient(data.RequestId);

            clientData.Phonenumber = data.PhoneNumber;
            clientData.Email = data.Email;

            _context.Requestclients.Update(clientData);
            _context.SaveChanges();
        }
    }
}
