namespace EasySave.model.backupStrategies;

public abstract class BackupStrategy
{
    protected readonly object PauseLock = new();
    protected bool isCancelled;
    private bool _isPaused;
    
    public abstract bool Execute(Backup backup);
    
    public abstract string GetName();

    public void Pause()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            Monitor.Enter(PauseLock);
        }
    }

    public void Resume()
    {
        if (_isPaused)
        {
            _isPaused = false;
            Monitor.Exit(PauseLock);
        }
    }

    public void Cancel()
    {
        if (_isPaused) // cancel even is the task is paused
        {
            Resume();
        }
        
        isCancelled = true;
    }

    protected void ResetState(Backup backup)
    {
        backup.Progression = 0;
        isCancelled = false;
        Resume();
    }
}