using System.IO;

namespace EasySave.model.backupStrategies;

public abstract class BackupStrategy
{
    protected readonly object PauseLock = new();
    protected bool IsCancelled;
    protected event EventHandler<long>? BytesCopied;
    protected long TotalBytesToCopy; 
    private bool _isPaused;

    public bool Execute(Backup backup)
    {
        var sourceFolderPath = backup.SourcePath;
        var targetFolderPath = backup.TargetPath;
        
        long totalBytesCopied = 0;

        BytesCopied = (_, bytesCopied) =>
        {
            totalBytesCopied += bytesCopied;
            long progression = totalBytesCopied * 100 / TotalBytesToCopy;

            backup.Progression = (int) progression;
        };
        
        ExecuteInternally(sourceFolderPath, targetFolderPath);
        
        var cancelled = IsCancelled;
        ResetState(backup);
        return !cancelled;
    }

    protected abstract void ExecuteInternally(string sourceFolderPath, string targetFolderPath);
    
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
        
        IsCancelled = true;
    }

    protected void ResetState(Backup backup)
    {
        backup.Progression = 0;
        TotalBytesToCopy = 0;
        IsCancelled = false;
        Resume();
    }
    
    protected static long GetDirectorySize(string folderPath)
    {
        var directoryInfo = new DirectoryInfo(folderPath);
        return directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fileInfo => fileInfo.Length);
    }

    protected void CopyFile(string sourceFilePath, string targetFilePath)
    {
        var buffer = new byte[1024 * 1024]; // 1MB buffer

        try
        {
            using (FileStream source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            {
                using (FileStream target = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
                {
                    int currentBlockSize;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        lock (PauseLock)
                        {
                            BytesCopied(this, currentBlockSize);

                            target.Write(buffer, 0, currentBlockSize);

                            if (IsCancelled)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (IOException)
        {
            IsCancelled = true;
        }
    }
}