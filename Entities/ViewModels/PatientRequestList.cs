namespace Entities.ViewModels
{
    public class PatientRequestList
    {
        public int RequestId {  get; set; } 

        public DateTime CreatedDate { get; set; }

        public RequestStatus CurrentStatus { get; set; }

        public int Document { get; set; }
    }
}
