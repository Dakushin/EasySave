using System.IO;
using System.Diagnostics;
using EasySave.view.wpf.core;
using EasySave.properties;
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
    public event EventHandler? ProcessPause;

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

    public void ReTry(List<string> FileToCopy, string sourceFolderPaht, string targetFolderPAth)
    {
        DoAllCopy(FileToCopy, sourceFolderPaht, targetFolderPAth);
    }

    protected void DoAllCopy(List<string> FileToCopy, string sourceFolderPath, string targetFolderPath)
    {
        _backupState.SetTotalFilesToCopy(FileToCopy.Count);
        var i = FileLeftToDo = FileToCopy.Count - (FileToCopy.Count - FileLeftToDo);
        var nameofprocess = CheckIfWorkProcessIsOpen(Model.GetInstance().GetListProcessToCheck());
        if (nameofprocess == string.Empty)
        {
            for(; i < FileToCopy.Count; i++)
            {
                if (IsCancelled) break;
                _backupState.SetTotalFileSize(new FileInfo(FileToCopy[i]).Length);
                var targetFilePath = GetPathWithDirectory(FileToCopy[i], sourceFolderPath, targetFolderPath);
                _backupState.SetSourceFilePath(FileToCopy[i]);
                _backupState.SetTargetFilePath(FileToCopy[i]);
                long timetocrypt = 0;
                Stopwatch sw = Stopwatch.StartNew();
                nameofprocess = CheckIfWorkProcessIsOpen(Model.GetInstance().GetListProcessToCheck());
                if (nameofprocess == string.Empty)
                {
                    if (_iscrypted && CheckToCrypt(FileToCopy[i]))
                    {
                        timetocrypt = Cryptage(FileToCopy[i], targetFilePath);
                    } else
                    {
                        CopyFile(FileToCopy[i], targetFilePath);
                    }
                }
                else
                {
                    ProcessPause.Invoke(this, new EventArgs());
                }
                sw.Stop();
                var log = new Log(_backupState.Name, FileToCopy[i], targetFilePath, string.Empty,
                                   new FileInfo(FileToCopy[i]).Length, sw.ElapsedMilliseconds, DateTime.Now.ToString(), timetocrypt);
                Model.GetInstance().GetLogFileFormat().SaveInFormat<Log>(Model.GetInstance().GetLogPath(), log);
                FileLeftToDo--;
                _backupState.SetTotalFilesLeftToDo(FileLeftToDo);
                UpdateSaveState(_backupState);
            }
        } else
        {
            throw new ProcessExecption(nameofprocess, this, sourceFolderPath, targetFolderPath, FileToCopy, 2);
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
        cryptosoft.StartInfo.Arguments = $"{sourcePath} {targetPath}";
        cryptosoft.StartInfo.UseShellExecute = true;
        cryptosoft.Start();
        cryptosoft.WaitForExit();
        return cryptosoft.ExitCode;
    }

    private string CheckIfWorkProcessIsOpen(List<string> listOfProcessToCheck) //Function for check if job Process is on
    {
        foreach (var ProcessToCheck in listOfProcessToCheck)
        {
            var processes = Process.GetProcessesByName(ProcessToCheck);
            if (processes.Length > 0) return ProcessToCheck;
        }

        return string.Empty;
    }

}