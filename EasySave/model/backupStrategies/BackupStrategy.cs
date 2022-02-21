using System.IO;
using System.Diagnostics;
namespace EasySave.model.backupStrategies;

public abstract class BackupStrategy
{
    protected readonly object PauseLock = new();
    private bool _isPaused;
    private string _backupAssociateName;
    protected bool IsCancelled;
    protected long TotalBytesToCopy;
    protected event EventHandler<long>? BytesCopied;

    public bool Execute(Backup backup)
    {
        var sourceFolderPath = backup.SourcePath;
        var targetFolderPath = backup.TargetPath;
        _backupAssociateName = backup.Name;
        long totalBytesCopied = 0;

        BytesCopied = (_, bytesCopied) =>
        {
            totalBytesCopied += bytesCopied;
            var progression = totalBytesCopied * 100 / TotalBytesToCopy;

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
            Resume();

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

    protected void CopyFileCrypted(string sourceFilePath, string targetFilePath)
    {

    }
    protected void CopyFile(string sourceFilePath, string targetFilePath)
    {
        var buffer = new byte[1024 * 1024]; // 1MB buffer
        Stopwatch sw = Stopwatch.StartNew();
        try
        {
            using (var source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read)) 
            {
                CreateDirectoryIfNotExist(targetFilePath);
                using (var target = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
                {
                    int currentBlockSize;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        lock (PauseLock)
                        {
                            BytesCopied(this, currentBlockSize);

                            target.Write(buffer, 0, currentBlockSize);

                            if (IsCancelled) break;
                        }
                    }
                }
            }
        }
        catch (IOException)
        {
            IsCancelled = true;
        }
        sw.Stop();
        var log = new Log(_backupAssociateName, sourceFilePath, targetFilePath, string.Empty,
                           new FileInfo(sourceFilePath).Length, sw.ElapsedMilliseconds, DateTime.Now.ToString());
        Model.GetInstance().GetLogFileFormat().SaveInFormat<Log>(Model.GetInstance().GetLogPath(), log);
    }

    protected List<string> GetAllFileFromDirectory(string[] directories/*, List<string> files*/)
    {
        var files = new List<string>();
        foreach (var directory in directories)
        {
            var list = Directory.GetDirectories(directory);
            if (list.Length > 0) files.AddRange(GetAllFileFromDirectory(list/*, files*/));

            files.AddRange(Directory.EnumerateFiles(directory));
        }
        return files;
    }

    private void CreateDirectoryIfNotExist(string targetFilePath)
    {
        var lastfile = targetFilePath.Split(Path.DirectorySeparatorChar);
        var newtargetpath = targetFilePath.Replace(Path.DirectorySeparatorChar + lastfile[lastfile.Length - 1], null);
        if (!Directory.Exists(newtargetpath))
        {
            Directory.CreateDirectory(Path.GetFullPath(newtargetpath));
        }
    }

}