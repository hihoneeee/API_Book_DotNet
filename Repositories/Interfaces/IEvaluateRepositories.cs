using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IEvaluateRepositories
    {
        Task<Evaluate> CreateEvaluateAsync(Evaluate evaluate);
        Task<Evaluate> UpdateEvaluateAsync(Evaluate oldEva, Evaluate newEva);

    }
}
