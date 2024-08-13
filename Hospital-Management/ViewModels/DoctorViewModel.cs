namespace Hospital_Management.ViewModels
{
    public class DoctorViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Img { get; set; }
        public int SpecialityId { get; set; }
        public int AvgRating { get; set; }
    }
}