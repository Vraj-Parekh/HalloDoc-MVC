using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class EncounterDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Date of Service is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfService { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string? PresentHistory { get; set; }

        public string? MedicalHistory { get; set; }

        public string? Medications { get; set; }

        public string? Allergies { get; set; }

        public decimal? Temp { get; set; }

        public int? HR { get; set; }

        public int? RR { get; set; }

        public string? BloodPressureSystolic { get; set; }
        public string? BloodPressureDiastolic { get; set; }

        public decimal? O2 { get; set; }

        public string? Pain { get; set; }

        public string? Heent { get; set; }

        public string? CV { get; set; }

        public string? Chest { get; set; }

        public string? ABD { get; set; }

        public string? Extr { get; set; }

        public string? Skin { get; set; }

        public string? Neuro { get; set; }

        public string? Other { get; set; }

        public string? Diagnosis { get; set; }

        [Required(ErrorMessage = "Treatment Plan is required")]
        public string TreatmentPlan { get; set; }

        public string? Dispensed { get; set; }

        public string? Procedures { get; set; }

        public string? Followup { get; set; }
    }
}
