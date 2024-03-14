using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class EncounterFormService : IEncounterFormService
    {
        private readonly HalloDocDbContext _context;
        private readonly IRequestClientServices requestClientServices;

        public EncounterFormService(HalloDocDbContext _context,IRequestClientServices requestClientServices)
        {
            this._context = _context;
            this.requestClientServices = requestClientServices;
        }
        private DateTime GenerateDateOfBirth(int? year, string? month, int? date)
        {
            DateTime finalDate = new DateTime(year ?? 1900, DateTime.ParseExact(month ?? "January", "MMMM", CultureInfo.CurrentCulture).Month, date ?? 01);
            return finalDate;
        }
        public EncounterDTO GetEncounterInfo(int requestId)
        {
            Requestclient? requestClient = requestClientServices.GetClient(requestId);
            if(requestClient == null) { return null; }

            EncounterDTO? encounter = new EncounterDTO()
            {
                FirstName = requestClient.Firstname,
                LastName = requestClient.Lastname ?? "",
#warning:include location in requestCLient table
                //Location = requestClient.Location,
                Location = requestClient.Street + ", " + requestClient.City + ", " + requestClient.State + ", " + requestClient.Zipcode,
                DateOfBirth = GenerateDateOfBirth(requestClient.Intyear, requestClient.Strmonth, requestClient.Intdate),
                PhoneNumber = requestClient.Phonenumber??"",
                Email = requestClient.Email??"",
            };
            return encounter;
        }

        public void AddEncounterInfo(int requestId,EncounterDTO data)
        {
            Encounterform encounterform = new Encounterform()
            {
                Historyofpresentillnessorinjury = data.PresentHistory,
                Medicalhistory = data.MedicalHistory,
                Medications = data.Medications,
                Allergies = data.Allergies,
                Temp = data.Temp,
                Hr = data.HR,
                Rr = data.RR,
                Bloodpressuresystolic = data.BloodPressureSystolic,
                Bloodpressurediastolic = data.BloodPressureDiastolic,
                O2 = data.O2,
                Pain = data.Pain,
                Heent = data.Heent,
                Cv = data.CV,
                Chest = data.Chest,
                Abd = data.ABD,
                Extremeties = data.Extr,
                Skin = data.Skin,
                Neuro = data.Neuro,
                Other = data.Other,
                Diagnosis = data.Diagnosis,
                Treatmentplan = data.TreatmentPlan,
                Medicationsdispensed = data.Dispensed,
                Procedures = data.Procedures,
                Followup = data.Followup,
                Requestid = requestId,
            };
        }
    }
}
