using SimpleManualDispatcher.Shared.Domain;

namespace SimpleManualDispatcher.Server.API.Repository;

public interface IReadableJobRepository
{
    // Job一覧の取得
    IAsyncEnumerable<Job> GetJobs();

}
