using SimpleManualDispatcher.Shared.Domain;

namespace SimpleManualDispatcher.Server.API.Repository;

public interface IEditableJobRepository
{
    // JOBの登録・更新
    Task UpsertJobAsync(Job newJob);

    // JOBの削除
    Task DeleteJobAsync(ushort JobId);
}
