using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface IExersiceSessionRepository
{

    Task<ExerciseSession?> AddSessionAsync(ExerciseSession session);
    Task<ExerciseSession?> UpdateSessionsAsync(ExerciseSession session, int id);
    Task<ExerciseSession?> DeleteSessionsAsync(int id);
    Task<ExerciseSession?> GetSessionsByIdAsync(int id);
    Task<ICollection<ExerciseSession>?> GetAllSessionsAsync(int pageNr, int pageSize);

    Task<ICollection<ExerciseSession>?> GetSessionsByUserId(string userId, int pageNr, int pageSize);

}
