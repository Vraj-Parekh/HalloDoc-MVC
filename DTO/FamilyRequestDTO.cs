namespace HalloDoc_Project.DTO
{
    public class FamilyRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string RelationWithPatient { get; set; }
        public string Symptoms { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PatientEmail { get; set; }
        public string PatientStreet { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZipCode { get; set; }
        public string PatientRoomOrSuite { get; set; }
    }
}