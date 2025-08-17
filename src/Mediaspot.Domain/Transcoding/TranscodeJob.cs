using Mediaspot.Domain.Common;
using Mediaspot.Domain.Transcoding.Events;

namespace Mediaspot.Domain.Transcoding;

public enum TranscodeStatus { Pending, Running, Succeeded, Failed }

public sealed class TranscodeJob : AggregateRoot
{
    public Guid AssetId { get; private set; }
    public Guid MediaFileId { get; private set; }
    public string Preset { get; private set; }
    public TranscodeStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TranscodeJob() { AssetId = Guid.Empty; MediaFileId = Guid.Empty; Preset = string.Empty; }

    public TranscodeJob(Guid assetId, Guid mediaFileId, string preset)
    {
        AssetId = assetId;
        MediaFileId = mediaFileId;
        Preset = preset;
        Status = TranscodeStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkRunning()
    {
        // 1. We change the status to Running
        Status = TranscodeStatus.Running;
        // 2. We created the event that the job has started.
        var enventJob = new TranscodeJobStarted(this.Id);
        UpdatedAt = DateTime.UtcNow;
        // 3. We register in the list of domain events the new event.
        this.Raise(enventJob);
    }

    public void MarkSucceeded()
    {
        // 1. We change the status to Succeeded
        Status = TranscodeStatus.Succeeded;
        // 2. We created the event that the job has Succeeded.
        var enventJob = new TranscodeJobSucceeded(this.Id);
        UpdatedAt = DateTime.UtcNow;
        // 3. We register in the list of domain events the new success.
        this.Raise(enventJob);
    }

    public void MarkFailed()
    {
        // 1. We change the status to Failed
        Status = TranscodeStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
        // 2. We created the event that described the job as Failed.
        var enventJob = new TranscodeJobFailed(this.Id);
        UpdatedAt = DateTime.UtcNow;
        // 3. We register in the list of domain events the failed event.
        this.Raise(enventJob);
    }
}