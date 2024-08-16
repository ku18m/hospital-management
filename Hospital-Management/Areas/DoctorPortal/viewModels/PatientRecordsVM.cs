namespace Hospital_Management.ViewModels
{
    public class PatientRecordsVM
    {
        public string PatientName { get; set; }
        
        public List<RecordViewModel> Records { get; set; }
    }
}
