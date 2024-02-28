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
        private readonly IRequestNotesServices requestNotesServices;
        private readonly IRequestStatusLogServices requestStatusLogServices;
        private readonly IRequestClientServices requestClientServices;

        public RequestNotesServices(HalloDocDbContext _context, IRequestServices requestServices,IRequestNotesServices requestNotesServices,IRequestStatusLogServices requestStatusLogServices,IRequestClientServices requestClientServices)
        {
            this._context = _context;
            this.requestServices = requestServices;
            this.requestNotesServices = requestNotesServices;
            this.requestStatusLogServices = requestStatusLogServices;
            this.requestClientServices = requestClientServices;
        }

        public Requestnote? GetRequestNotes(int requestId)
        {
            return _context.Requestnotes.FirstOrDefault(a=>a.Requestid == requestId);
        }

        public void ViewNotes(int requestId)
        {
            Requestclient? client = requestClientServices.GetClient(requestId);
            //pending
        }
        public void AddNotes(ViewNotesDTO model)
        {
            Request? request = requestServices.GetRequest(model.RequestId);
            Requestnote? requestNotes = GetRequestNotes(model.RequestId);
            //Requeststatuslog? transferNotes = requestStatusLogServices.GetTransferNotes(model.RequestId);

            if (requestNotes != null)
            {
                requestNotes.Adminnotes = model.AdminNotes;
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
                    Requestid = model.RequestId,
                    Adminnotes = model.AdminNotes,
                    Physiciannotes = model.PhysicianNotes,
                    Createdby = request.Email,
                    Createddate = DateTime.Now,
                };
                _context.Requestnotes.Add(addNote);
            }
            _context.SaveChanges();
        }
    }
}
