namespace EasySave.model.backupStrategies;

public class Differential : BackupStrategy
{
    public override bool Execute(Backup backup)
    {
        var x = 1;

        while (!isCancelled && ++x <= 100)
        {
            lock(PauseLock)
            {
                Thread.Sleep(10);
                backup.Progression = x;
            }
        }

        var cancelled = isCancelled;
        ResetState(backup);
        return !cancelled;
    }

    public override string GetName()
    {
        return "Differential";
    }
}