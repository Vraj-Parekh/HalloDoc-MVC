namespace HalloDoc_Project.DTO
{
    public class ViewDocumentList
    {
        public List<FileData> Document { get; set; }
        public string Name { get; set; }
        public string ConfirmationNumber { get; set; }
    }

    public class FileData
    {
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DocumentId { get; set; }
    }
}