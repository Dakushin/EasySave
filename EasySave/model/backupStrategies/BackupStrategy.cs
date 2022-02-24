using System.Diagnostics;
using System.IO;

namespace EasySave.model.backupStrategies;

public abstract class BackupStrategy
{
    private static readonly ReaderWriterLockSlim UpdateLock = new();
    private bool isPaused;
    protected bool IsCancelled;
    protected long TotalBytesToCopy;
    protected int FileLeftToDo = 0;
    protected event EventHandler<long>? BytesCopied;
    protected readonly object PauseLock = new();
    protected BackupState _backupState;
    private static bool DoToHighFile = false;
    private bool _iscrypted;
    private bool doPriorityFile;
    private int Threadthatdopause;

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
        if (!cancelled) EndBackup(_backupState);
        ResetState(backup);
        return !cancelled;
    }

    protected abstract void ExecuteInternally(string sourceFolderPath, string targetFolderPath);

    public abstract string GetName();

    public void Pause()
    {
        if (!isPaused)
        {
            Threadthatdopause = Thread.CurrentThread.ManagedThreadId;
            isPaused = true;
            Monitor.Enter(PauseLock);
        }
    }

    public void Resume()
    {
        if (isPaused)
        {

            if (Threadthatdopause == Thread.CurrentThread.ManagedThreadId)
            {
                isPaused = false;
                Monitor.Exit(PauseLock);
            }
        }
    }

    public void Cancel()
    {
        if (isPaused) // cancel even is the task is paused
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

    protected void CopyFile(string sourceFilePath, string targetFilePath)
    {
        var buffer = new byte[1024 * 1024]; // 1MB buffer
        try
        {
            using (var source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            {
                CreateDirectoryIfNotExist(targetFilePath);
                using (var target = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
                {
                    int currentBlockSize;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0 && !IsCancelled)
                        lock (PauseLock)
                        {
                            BytesCopied(this, currentBlockSize);

                            target.Write(buffer, 0, currentBlockSize);
                        }
                }
            }
        }
        catch (IOException)
        {
            IsCancelled = true;
        }
    }

    protected List<string> GetAllFileFromDirectory(string sourcePathDirectoy, bool firstdirectory)
    {
        var files = new List<string>();
        if (firstdirectory) files.AddRange(Directory.GetFiles(sourcePathDirectoy));
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
        if (!Directory.Exists(newtargetpath)) Directory.CreateDirectory(Path.GetFullPath(newtargetpath));
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
        if (UpdateLock.TryEnterWriteLock(1000))
        {
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

            UpdateLock.ExitWriteLock();
        }
    }

    protected string GetPathWithDirectory(string sourceFilePath, string sourceFolderPath, string targetFolderPath)
    {
        return Path.Combine(targetFolderPath,
            sourceFilePath.Replace(sourceFolderPath + Path.DirectorySeparatorChar, null));
    }

    public void SetFileLeftToDo(int Fltd)
    {
        FileLeftToDo = Fltd;
    }

    protected void DoAllCopy(List<string> FileToCopy, string sourceFolderPath, string targetFolderPath)
    {
        _backupState.SetTotalFilesToCopy(FileToCopy.Count);
        FileToCopy = OrdonedPriorityFile(FileToCopy);
        List<string> ToHighFile = new List<string>();
        foreach (var fileSourcePath in FileToCopy)
        {
            if (IsCancelled) break;

            if (IsPriorityFile(fileSourcePath))
            {
                doPriorityFile = true;
                PauseAllOtherThread();
            }
            else
            {
                doPriorityFile = false;
                ResumeAllThread();
            }
            if (new FileInfo(fileSourcePath).Length >= 10000 && !doPriorityFile)
            {
                if (!DoToHighFile)
                {
                    DoToHighFile = true;
                    if (ToHighFile.Count > 0)
                    {
                        ToHighFile.Add(fileSourcePath);
                        DoCopy(ToHighFile.First(), GetPathWithDirectory(fileSourcePath, sourceFolderPath, targetFolderPath));
                        ToHighFile.RemoveAt(0);
                        
                    }
                    else
                    {
                        DoCopy(fileSourcePath, GetPathWithDirectory(fileSourcePath, sourceFolderPath, targetFolderPath));
                    }
                    DoToHighFile = false;
                }
                else
                {
                    ToHighFile.Add(fileSourcePath);
                }
            }
            else
            {
                DoCopy(fileSourcePath, GetPathWithDirectory(fileSourcePath, sourceFolderPath, targetFolderPath));
            }
            
        }

        if (doPriorityFile)
        {
            doPriorityFile = false;
            ResumeAllThread();
        }
    }


    private bool CheckToCrypt(string file)
    {
        foreach (var ext in Model.GetInstance().GetListExtentionToCheck())
            if (ext == Path.GetExtension(file))
                return false;
        return true;
    }

    private int Cryptage(string sourcePath, string targetPath)
    {
        var cryptosoft = new Process();
        cryptosoft.StartInfo.FileName = "Cryptosoft.exe";
        cryptosoft.StartInfo.Arguments = $"\"{sourcePath}\" \"{targetPath}\"";
        cryptosoft.StartInfo.UseShellExecute = false;
        cryptosoft.StartInfo.CreateNoWindow = true;
        cryptosoft.Start();
        cryptosoft.WaitForExit();
        return cryptosoft.ExitCode;
    }

    private List<string> OrdonedPriorityFile(List<string> FileToCopy)
    {
        var priotitylist = new List<string>();
        var list = new List<string>();
        foreach (var file in FileToCopy)
            if (IsPriorityFile(file))
                priotitylist.Add(file);
            else
                list.Add(file);
        priotitylist.AddRange(list);
        return priotitylist;
    }

    private bool IsPriorityFile(string file)
    {
        foreach (var extension in Model.GetInstance().GetListPriorityExtension())
            if (Path.GetExtension(file) == extension)
                return true;
        return false;
    }

    private void PauseAllOtherThread()
    {
        foreach (var backup in Model.GetInstance().GetBackupList())
            if (backup.IsExecute && !backup.BackupStrategy.doPriorityFile)
                backup.BackupStrategy.Pause();
    }

    private void ResumeAllThread()
    {
        var resumeall = false;
        foreach (var backup in Model.GetInstance().GetBackupList())
            if (!backup.BackupStrategy.doPriorityFile)
                resumeall = true;
        if (resumeall)
            foreach (var backup in Model.GetInstance().GetBackupList())
                backup.BackupStrategy.Resume();
    }

    private void DoCopy(string fileSourcePath, string targetFilePath)
    {
        _backupState.SetTotalFileSize(new FileInfo(fileSourcePath).Length);
        _backupState.SetSourceFilePath(fileSourcePath);
        _backupState.SetTargetFilePath(fileSourcePath);
        long timetocrypt = 0;
        var sw = Stopwatch.StartNew();
        if (_iscrypted && CheckToCrypt(fileSourcePath))
            timetocrypt = Cryptage(fileSourcePath, targetFilePath);
        else
            CopyFile(fileSourcePath, targetFilePath);
        sw.Stop();
        if (UpdateLock.TryEnterWriteLock(1000))
        {
            var log = new Log(_backupState.Name, fileSourcePath, targetFilePath, string.Empty,
                new FileInfo(fileSourcePath).Length, sw.ElapsedMilliseconds, DateTime.Now.ToString(), timetocrypt);
            Model.GetInstance().GetLogFileFormat().SaveInFormat(Model.GetInstance().GetLogPath(), log);
            UpdateLock.ExitWriteLock();
        }

        FileLeftToDo--;
        _backupState.SetTotalFilesLeftToDo(FileLeftToDo);
        UpdateSaveState(_backupState);
    }

}