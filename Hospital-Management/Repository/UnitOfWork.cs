using Hospital_Management.Data;
using Hospital_Management.Models;

namespace Hospital_Management.Repository
{
    public class UnitOfWork(
            ApplicationDbContext _context,
            IDoctorRepo _doctorRepository,
            IPatientRepo _patientRepository,
            IAssistantRepo _assistantRepository,
            IReservationRepo _reservationRepository,
            IArticleRepo _articleRepository,
            IRecordRepo _recordRepository,
            IRateRepo _rateRepository,
            ISpecialityRepo _specialityRepository
        ) : IUnitOfWork
    {
        public IDoctorRepo DoctorRepository { get; } = _doctorRepository;

        public IPatientRepo PatientRepository { get; } = _patientRepository;

        public IAssistantRepo AssistantRepository { get; } = _assistantRepository;

        public IArticleRepo ArticleRepository { get; } = _articleRepository;

        public IReservationRepo ReservationRepository { get; } = _reservationRepository;

        public IRateRepo RateRepository { get; } = _rateRepository;

        public IRecordRepo RecordRepository { get; } = _recordRepository;

        public ISpecialityRepo SpecialityRepository { get; } = _specialityRepository;

        public void Dispose()
        {
            _context.Dispose();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
