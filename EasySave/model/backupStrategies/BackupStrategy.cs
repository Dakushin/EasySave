using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace EasySave.model.backupStrategies;

public abstract class BackupStrategy
{
    protected readonly object PauseLock = new();
    private bool _isPaused;
    protected bool IsCancelled;
    protected long TotalBytesToCopy;
    protected int FileLeftToDo = 0;
    protected event EventHandler<long>? BytesCopied;
    protected BackupState _backupState;
    private bool _iscrypted;

    public bool Execute(Backup backup)
    {
        var sourceFolderPath = backup.SourcePath;
        var targetFolderPath = backup.TargetPath;
        _iscrypted = backup.Crypted;
        _backupState = new BackupState(backup.Name, "ACTIVE");
        long totalBytesCopied = 0;

        BytesCopied = (_, bytesCopied) =>
        {
            totalBytesCopied += bytesCopied;
            var progression = totalBytesCopied * 100 / TotalBytesToCopy;

            backup.Progression = (int) progression;
            _backupState.SetProgression((int) progression);
            UpdateSaveState(_backupState);
        };

        ExecuteInternally(sourceFolderPath, targetFolderPath);

        var cancelled = IsCancelled;
        if (!cancelled)
        {
            EndBackup(_backupState);
        }
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

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0 && !IsCancelled)
                    {
                        lock (PauseLock)
                        {
                            BytesCopied(this, currentBlockSize);

                            target.Write(buffer, 0, currentBlockSize);
                 
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
        var log = new Log(_backupState.Name, sourceFilePath, targetFilePath, string.Empty,
                           new FileInfo(sourceFilePath).Length, sw.ElapsedMilliseconds, DateTime.Now.ToString());
        Model.GetInstance().GetLogFileFormat().SaveInFormat<Log>(Model.GetInstance().GetLogPath(), log);
    }

    protected List<string> GetAllFileFromDirectory(string sourcePathDirectoy, bool firstdirectory)
    {

        var files = new List<string>();
        if(firstdirectory)
        {
            files.AddRange(Directory.GetFiles(sourcePathDirectoy));
        }
        foreach (var directory in Directory.GetDirectories(sourcePathDirectoy))
        {
            var list = Directory.GetDirectories(directory);
            if (list.Length > 0) files.AddRange(GetAllFileFromDirectory(directory, false));

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

    private void EndBackup(BackupState backupState) //Update SaveState to END
    {
        backupState.SetSourceFilePath("");
        backupState.SetTargetFilePath("");
        backupState.SetTotalFileSize(0);
        backupState.SetState("END");
        backupState.SetTotalFilesToCopy(0);
        backupState.SetTotalFilesLeftToDo(0);
        UpdateSaveState(backupState);
    }

    private void UpdateSaveState(BackupState backupState) //Update the Save state file
    {
        List<BackupState> states = null;
        FileFormat Json = new Json();
        if (File.Exists(Model.GetInstance().GetSaveStatePath()))
            states = Json.UnSerialize<BackupState>(Model.GetInstance().GetSaveStatePath());
        File.Delete(Model.GetInstance().GetSaveStatePath());
        var enter = false;
        if (states != null)
        {
            for (var i = 0; i < states.Count; i++)
                if (states[i].GetName() == backupState.GetName())
                {
                    states[i] = backupState;
                    enter = true;
                }

            if (!enter) states.Add(backupState);
            foreach (var sv in states) Json.SaveInFormat(Model.GetInstance().GetSaveStatePath(), sv);
        }
        else
        {
            Json.SaveInFormat(Model.GetInstance().GetSaveStatePath(), backupState);
        }
    }

    protected string GetPathWithDirectory(string sourceFilePath, string sourceFolderPath, string targetFolderPath)
    {
        return Path.Combine(targetFolderPath, sourceFilePath.Replace(sourceFolderPath + Path.DirectorySeparatorChar, null));
    }

    public void SetFileLeftToDo(int Fltd)
    {
        FileLeftToDo = Fltd;
    }

    protected void DoAllCopy(List<string> FileToCopy, string sourceFolderPath, string targetFolderPath)
    {
        _backupState.SetTotalFilesToCopy(FileToCopy.Count);
        FileLeftToDo = FileToCopy.Count;
        foreach(var sourceFilePath in FileToCopy)
        {
            if (IsCancelled) break;
            _backupState.SetTotalFileSize(new FileInfo(sourceFilePath).Length);
            var targetFilePath = GetPathWithDirectory(sourceFilePath, sourceFolderPath, targetFolderPath);
            _backupState.SetSourceFilePath(sourceFilePath);
            _backupState.SetTargetFilePath(targetFilePath);

            Stopwatch sw = Stopwatch.StartNew();
            CopyFile(sourceFilePath, targetFilePath);
            sw.Stop();
            var log = new Log(_backupState.Name, sourceFilePath, targetFilePath, string.Empty,
                               new FileInfo(sourceFilePath).Length, sw.ElapsedMilliseconds, DateTime.Now.ToString(CultureInfo.InvariantCulture));
            Model.GetInstance().GetLogFileFormat().SaveInFormat<Log>(Model.GetInstance().GetLogPath(), log);
            FileLeftToDo--;
            _backupState.SetTotalFilesLeftToDo(FileLeftToDo);
            UpdateSaveState(_backupState);
        }
    }
}