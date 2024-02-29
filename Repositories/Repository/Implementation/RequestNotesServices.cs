using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System.Globalization;

namespace Repositories.Repository.Implementation
{
    public class RequestNotesServices : IRequestNotesServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IRequestServices requestServices;

        private readonly IRequestStatusLogServices requestStatusLogServices;
        private readonly IRequestClientServices requestClientServices;

        public RequestNotesServices(HalloDocDbContext _context, IRequestServices requestServices, IRequestStatusLogServices requestStatusLogServices, IRequestClientServices requestClientServices)
        {
            this._context = _context;
            this.requestServices = requestServices;
            this.requestStatusLogServices = requestStatusLogServices;
            this.requestClientServices = requestClientServices;
        }

        public Requestnote? GetRequestNotes(int requestId)
        {
            return _context.Requestnotes.FirstOrDefault(a => a.Requestid == requestId);
        }

        public ViewNotesDTO GetViewRequestNotes(int requestId)
        {
            Requestclient? client = requestClientServices.GetClient(requestId);
            Requestnote? adminNotes = GetRequestNotes(requestId);
            List<Requeststatuslog>? transferNotes = requestStatusLogServices.GetTransferNotes(requestId);

            ViewNotesDTO model = new()
            {
                AdminNotes = adminNotes.Adminnotes,
                TransferNotes = transferNotes.Select(a => a.Notes).ToList(),
                PhysicianNotes = adminNotes.Physiciannotes,
                RequestId = requestId,
            };
            return model;
        }
        public void AddNotes(ViewNotesDTO model, int requestId)
        {
            Request? request = requestServices.GetRequest(requestId);
            Requestnote? requestNotes = GetRequestNotes(requestId);
            //Requeststatuslog? transferNotes = requestStatusLogServices.GetTransferNotes(model.RequestId);

            if (requestNotes != null)
            {
                requestNotes.Adminnotes = model.AdditionalNotes;
                requestNotes.Physiciannotes = model.PhysicianNotes;
                requestNotes.Createdby = request.Email;
                requestNotes.Modifiedby = request.Email;
                requestNotes.Modifieddate = DateTime.Now;

                _context.Requestnotes.Update(requestNotes);
            }
            else
            {
                Requestnote? addNote = new Requestnote()
                {
                    Requestid = requestId,
                    Adminnotes = model.AdditionalNotes,
                    Physiciannotes = model.PhysicianNotes,
                    Createdby = "Admin",
                    Createddate = DateTime.Now,
                };
                _context.Requestnotes.Add(addNote);
            }
            _context.SaveChanges();
        }
    }
}
