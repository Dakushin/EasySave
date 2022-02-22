using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using EasySave.model;
using EasySave.model.backupStrategies;
using EasySave.properties;
using EasySave.translation;
using EasySave.view;
using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class BackupsViewModel : ViewModelBase
{
    //PRIVATE VARIABLE
    private readonly Model _model;
    private readonly View _view;
    private string _filterText;

    //CONSTRUCTOR
    public BackupsViewModel(View v)
    {
        _view = v;
        _model = Model.GetInstance();
    }

    public BackupsViewModel() : this(null)
    {
    }

    public ObservableCollection<Backup> Backups => _model.GetBackupList();

    public Backup SelectedBackup { get; set; }

    //PUBLIC EVENT
    public event EventHandler<string> OnProgressUpdate;

    public void ExecuteSelectedBackup()
    {
        ExecuteBackup(SelectedBackup);
    }

    public void DeleteSelectedBackup()
    {
        DeleteBackup(SelectedBackup.Name);
    }

    public void ResumeSelectedBackup()
    {
        SelectedBackup.BackupStrategy.Resume();
    }

    public void PauseSelectedBackup()
    {
        SelectedBackup.BackupStrategy.Pause();
    }

    public void CancelSelectedBackup()
    {
        SelectedBackup.BackupStrategy.Cancel();
    }

    public void CreateBackup(string name, string sourcePath, string targetPath, BackupStrategy backupStrategy, bool isEncrypted)
    {
        if (_model.GetBackupList().Count < 5) //check if we have more than 5 backups
        {
            if (_model.FindbyName(name) == null) //check if it already exists
            {
                if (Directory.Exists(sourcePath) || File.Exists(sourcePath)) // check if source path is good
                {
                    targetPath = Path.GetFullPath(targetPath);
                    _model.GetBackupList().Add(new Backup(name, sourcePath, targetPath, backupStrategy, isEncrypted));
                    NotifySuccess(Resources.Backup_Created);
                }
                else
                {
                    NotifyError(Resources.Error_Wrong_SourcePath);
                }
            }
            else
            {
                NotifyError(Resources.Error_Backup_Already_Exists);
            }
        }
        else
        {
            NotifyError(Resources.Error_Too_Many_Backups);
        }
    }

    public void DeleteBackup(string name) //function to delete a Savework by name
    {
        var sv = _model.FindbyName(name);
        if (sv != null)
        {
            _model.GetBackupList().Remove(sv);
            NotifySuccess(Resources.Success);
        }
        else
        {
            NotifyError(Resources.Error_Backup_Not_Found);
        }
    }

    private async void ExecuteBackup(Backup backup)
    {
        var success = await Task.Run(backup.Execute);
        if (success)
            
            NotifySuccess($"{backup.Name} {Resources.Success_Execution}");
        else
            NotifyError($"{backup.Name} {Resources.Cancelled}");
    }

   

    private void GetAllFileFromDirectory(string[] directories, List<string> files) //return all file in a directory
    {
        foreach (var directory in directories)
        {
            var list = Directory.GetDirectories(directory);
            if (list.Length > 0) GetAllFileFromDirectory(list, files);

            files.AddRange(Directory.EnumerateFiles(directory));
        }
    }

    public void ExecAllBackup() //Execute all savework
    {
        if (_model.GetBackupList().Count > 0)
            foreach (var sw in _model.GetBackupList())
                ExecuteBackup(sw);
        else
            NotifyInfo(Resources.Info_No_Backup);
    }

    public void ChangeLanguage(Language language) //Function to change language
    {
        CultureInfo.CurrentUICulture = language switch
        {
            Language.English => CultureInfo.GetCultureInfo("en"),
            Language.French => CultureInfo.GetCultureInfo("fr"),
            _ => CultureInfo.CurrentUICulture
        };
        NotifySuccess(Resources.Success);
    }

    public void ChangeFileFormat(FileFormat fileFormat)
    {
        _model.SetLogFileFormat(fileFormat);
        NotifySuccess(Resources.Success);
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

    private bool CheckToCrypt(string file)
    {
        foreach (var ext in _model.GetListExtentionToCheck())
            if (ext == Path.GetExtension(file))
                return false;
        return true;
    }

    public void Filter(string filterText)
    {
        _filterText = filterText;

        var view = CollectionViewSource.GetDefaultView(Backups);
        view.Filter = item =>
        {
            if (item is Backup backup) return backup.Name.ToLower().Contains(_filterText.ToLower());

            return true;
        };
    }
}