namespace EasySave.model.backupStrategies;

public class Complete : BackupStrategy
{
    public override bool Execute(Backup backup)
    {
        var x = 1;

        while (!isCancelled && ++x <= 100)
        {
            lock(PauseLock)
            {
                Thread.Sleep(30);
                backup.Progression = x;
            }
        }

        var cancelled = isCancelled;
        ResetState(backup);
        return !cancelled;
    }

    public override string GetName()
    {
        return "Complete";
    }
}