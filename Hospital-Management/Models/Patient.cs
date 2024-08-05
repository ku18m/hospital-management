namespace Hospital_Management.Models
{
    public class Patient: ApplicationUser
    {
        public virtual List<Record> Records { get; set; }

        public virtual List<Reservation> Reservations { get; set; }

        public virtual List<Rate> Rates { get; set; }

        public int AbscentTimes { get; set; } = 0;
    }
}
