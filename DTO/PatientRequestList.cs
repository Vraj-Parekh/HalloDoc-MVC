namespace HalloDoc_Project.DTO
{
    public class PatientRequestList
    {
        public DateTime CreatedDate { get; set; }

        public RequestStatus CurrentStatus { get; set; }

        public int Document { get; set; }
    }
}
