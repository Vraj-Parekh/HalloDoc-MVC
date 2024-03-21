using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using HalloDoc.Utility;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repositories.Repository.Implementation
{
    public class RequestClientServices : IRequestClientServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IEmailSender emailSender;

        public RequestClientServices(HalloDocDbContext _context,IEmailSender emailSender)
        {
            this._context = _context;
            this.emailSender = emailSender;
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

        public void SendAgreement(int requestId, string phoneNumber, string email)
        {
            Requestclient? requestClient = GetClient(requestId);
            if (requestClient is not null)
            {
                requestClient.Email = email;
                requestClient.Phonenumber = phoneNumber;
                _context.Requestclients.Update(requestClient);

                emailSender.SendEmailAsync(email, "Agreement", $"Tap the link to accept or cancel the agreement: <a href=\"https://localhost:44396/Patient/ReviewAgreement/{requestId}\">Agreement Link</a>");
            }
        }

        public void UpdateMobileEmail(int requestId,string email,string phoneNumber)
        {
            Requestclient? clientData = GetClient(requestId);

            clientData.Phonenumber = phoneNumber;
            clientData.Email = email;

            _context.Requestclients.Update(clientData);
            _context.SaveChanges();
        }
    }
}
