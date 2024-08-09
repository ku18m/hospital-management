using Hospital_Management.Models;

namespace Hospital_Management.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IDoctorRepo DoctorRepository { get; }
        IPatientRepo PatientRepository { get; }
        IAssistantRepo AssistantRepository { get; }
        IArticleRepo ArticleRepository { get; }
        IReservationRepo ReservationRepository { get; }
        IRateRepo RateRepository { get; }
        IRecordRepo RecordRepository { get; }
        ISpecialityRepo SpecialityRepository { get; }
        int Save();
    }
}
