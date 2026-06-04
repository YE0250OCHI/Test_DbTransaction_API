namespace SimpleManualDispatcher.Shared.Domain;

public record Job(
    int Id,
    JobStatus Status,
    JobState State,
    string From,
    string To,
    int AssignedVehicleId,
    DateTime QueuedAt,
    DateTime? AssignedAt,
    DateTime? FinishedAt
);
