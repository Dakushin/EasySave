using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Media;
using System.Windows.Data;
using EasySave.model;
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
    //PUBLIC EVENT
    public event EventHandler<string> OnProgressUpdate;

    public ObservableCollection<SaveWork> Backups => _model.GetSaveWorkList();

    public SaveWork SelectedSaveWork { get; set; }

    //CONSTRUCTOR
    public BackupsViewModel(View v)
    {
        _view = v;
        _model = Model.GetInstance();

        TryRecupFromSaveStatePath();
    }

    public void ExecuteSelectedBackup()
    {
        ExecSaveWork(SelectedSaveWork.Name);
    }

    public void DeleteSelectedBackup()
    {
        DeleteSaveWork(SelectedSaveWork.Name);
    }

    public BackupsViewModel() : this(null) {}

    public void CreateSaveWork(string name, string sourcePath, string targetPath, SaveType saveType) //Function that create savework
    {
        if (_model.GetSaveWorkList().Count < 5) //check if we have more than 5 wavework
        {
            if (_model.FindbyName(name) == null) //check if already existe
            {
                if (Directory.Exists(sourcePath) || File.Exists(sourcePath)) // check if source path is good
                {
                    targetPath = Path.GetFullPath(targetPath);
                    _model.GetSaveWorkList().Add(new SaveWork(name, sourcePath, targetPath, saveType));
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

    private void UpdateSaveState(SaveState saveState) //Update the Save state file
    {
        List<SaveState> states = null;
        FileFormat Json = new Json();
        if (File.Exists(_model.GetSaveStatePath()))
            states = Json.UnSerialize<SaveState>(_model.GetSaveStatePath());
        File.Delete(_model.GetSaveStatePath());
        var enter = false;
        if (states != null)
        {
            for (var i = 0; i < states.Count; i++)
                if (states[i].GetName() == saveState.GetName())
                {
                    states[i] = saveState;
                    enter = true;
                }

            if (!enter) states.Add(saveState);
            foreach (var sv in states) Json.SaveInFormat(_model.GetSaveStatePath(), sv);
        }
        else
        {
            Json.SaveInFormat(_model.GetSaveStatePath(), saveState);
        }
    }

    public void DeleteSaveWork(string name) //function to delete a Savework by name
    {
        var sv = _model.FindbyName(name);
        if (sv != null)
        { 
            _model.GetSaveWorkList().Remove(sv);
            NotifySuccess(Resources.Success);
        }
        else
            NotifyError(Resources.Error_Backup_Not_Found);
    }

    public void TryRecupFromSaveStatePath() //Function to fetch savework unfinish from the savestate file
    {
        if (!File.Exists(_model.GetSaveStatePath())) return;

        FileFormat fileFormat = new Json();
        List<SaveState> saveStates;
        if (!File.Exists(_model.GetSaveStatePath())) return;
        
        saveStates = fileFormat.UnSerialize<SaveState>(_model.GetSaveStatePath());
        foreach (var sv in saveStates)
        {
            if (sv.GetNbFilesLeftToDo() > 0)
            {
                List<string> listDirectorySource = new List<string>(sv.SourceFilePath.Split(Path.DirectorySeparatorChar));
                List<string> listDirectoryTarget = new List<string>(sv.TargetFilePath.Split(Path.DirectorySeparatorChar));
                bool sameDirectory = false;
                while (!sameDirectory && listDirectorySource.Any() && listDirectoryTarget.Any()) //Loop to fetch the original directory from all path
                {
                    if (listDirectorySource.Last<string>() == listDirectoryTarget.Last<string>())
                    {
                        listDirectorySource.Remove(listDirectorySource.Last<string>());
                        listDirectoryTarget.Remove(listDirectoryTarget.Last<string>());
                    }
                    else
                    {
                        sameDirectory = true;
                    }
                }
                string sourcePath = string.Join(Path.DirectorySeparatorChar, listDirectorySource);
                string targetPath = string.Join(Path.DirectorySeparatorChar, listDirectoryTarget);
                _model.GetSaveWorkList().Add(new SaveWork(sv.GetName(), sourcePath, targetPath, SaveType.Differential));
            }
        }
        
    }
        public void ExecSaveWork(string name) //Function to do the action of copie-past in a save directory
        {
            var sv = _model.FindbyName(name);
            if (sv == null)
            {
                NotifyError(Resources.Error_NoSaveWorkOfThisName);
                return;
            }
            if (!Directory.Exists(sv.TargetPath)) Directory.CreateDirectory(sv.TargetPath);
            string NameOfProcessRunning = CheckIfWorkProcessIsOpen(_model.GetListProcessToCheck());
            if (NameOfProcessRunning != string.Empty)
            {
                NotifyError(Resources.Error_WorkingProcess);
                NotifyInfo(NameOfProcessRunning);
                return;
            }
            var files = new List<string>();
            files.AddRange(Directory.EnumerateFiles(sv.SourcePath));
            GetAllFileFromDirectory(Directory.GetDirectories(sv.SourcePath), files);
            var saveState = new SaveState(sv.Name, "ACTIVE");
            switch (sv.SaveType) //Do something diffrente in function of the type
            {
                case SaveType.Complete:
                    {
                        saveState.SetTotalFilesToCopy(files.Count);
                        var FileLeftToDo = files.Count;
                        //var Progression = 0f;
                        //CalculFillProgress(Progression);
                        foreach (var file in files)
                        {
                            var filePath = file.Replace(sv.SourcePath + Path.DirectorySeparatorChar, null);
                            var targetPath = Path.Combine(sv.SourcePath, filePath);
                            saveState.SetSourceFilePath(file);
                            saveState.SetTargetFilePath(targetPath);
                            
                            Stopwatch sw = new Stopwatch();
                            if (CheckToCrypt(file))
                            {
                                sw.Start();
                                int timetoencrypt = Cryptage(file, targetPath);
                                sw.Stop();
                                _model.GetLogFileFormat().SaveInFormat(_model.GetLogPath(),
                                new Log(sv.Name, file, targetPath, string.Empty,
                                File.ReadAllBytes(file).Length, (float)sw.ElapsedMilliseconds, DateTime.Now.ToString(), timetoencrypt));
                            }
                            else
                            {
                                sw.Start();
                                File.Copy(file, targetPath, true);
                                sw.Stop();
                                _model.GetLogFileFormat().SaveInFormat(_model.GetLogPath(),
                                new Log(sv.Name, file, targetPath, string.Empty,
                                File.ReadAllBytes(file).Length, (float)sw.ElapsedMilliseconds, DateTime.Now.ToString(), 0));
                            }

                            FileLeftToDo--;
                            //Progression = (float)(files.Count - FileLeftToDo) / (float)files.Count * 100f;
                            //CalculFillProgress(Progression);
                            saveState.SetTotalFilesLeftToDo(FileLeftToDo);
                            UpdateSaveState(saveState);

                        }

                        EndSaveWork(saveState);
                        //OnProgresseUpdate.Invoke(this, null);
                        //_view.DisplaySuccess(strings.Success);
                        break;
                    }
                case SaveType.Differential:
                    {
                        saveState.SetTotalFilesToCopy(files.Count);
                        var FileLeftToDo = files.Count;
                        foreach (var file in files)
                        {
                            var filePath = file.Replace(sv.SourcePath + Path.DirectorySeparatorChar, null);
                            var targetPath = Path.Combine(sv.SourcePath, filePath);
                            saveState.SetSourceFilePath(file);
                            saveState.SetTargetFilePath(targetPath);
                            //var Progression = 0f;
                            //CalculFillProgress(Progression);
                            if (File.Exists(targetPath)) //try to know if the file exist, to check if he was modified
                            {
                                var fs1 = new FileStream(file, FileMode.Open);
                                var fs2 = new FileStream(targetPath, FileMode.Open);

                                var same = true;
                                if (fs1.Length == fs2.Length)
                                {
                                    var fb1 = fs1.ReadByte();
                                    var fb2 = fs2.ReadByte();

                                    while (fb1 != -1 && same) //loop to check byte per byte the 2 files
                                    {
                                        if (fb1 == fb2)
                                            same = true;
                                        else
                                            same = false;
                                        fb1 = fs1.ReadByte();
                                        fb2 = fs2.ReadByte();
                                    }

                                    // Close the file
                                    fs1.Close();
                                    fs2.Close();

                                    if (!same) //if the file wasn't the same, he do the copy
                                    {
                                        DateTime datetime = DateTime.Now;
                                        File.Copy(file, targetPath, true);
                                        TimeSpan time = DateTime.Now.Subtract(datetime);


                                        saveState.SetTotalFilesLeftToDo(FileLeftToDo);
                                        //var log = new Log(sv.GetName(), file, targetPath, string.Empty,
                                        // File.ReadAllBytes(file).Length,(float) time.TotalMilliseconds, DateTime.Now.ToString());
                                        //_model.GetLogFileFormat().SaveInFormat(_model.GetLogPath(), log);
                                        UpdateSaveState(saveState);
                                    }



                                }

                                FileLeftToDo--;
                                // Progression = (float)(files.Count - FileLeftToDo) / (float)files.Count * 100f;
                                // CalculFillProgress(Progression);



                            }
                            else //if the file doesn't existe it will be create
                            {
                                DateTime datetime = DateTime.Now;
                                File.Copy(file, targetPath, true);
                                TimeSpan time = DateTime.Now.Subtract(datetime);


                                saveState.SetTotalFilesLeftToDo(FileLeftToDo);
                                //var log = new Log(sv.GetName(), file, targetPath, string.Empty,
                                //File.ReadAllBytes(file).Length, (float) time.TotalMilliseconds, DateTime.Now.ToString());
                                //_model.GetLogFileFormat().SaveInFormat(_model.GetLogPath(), log);
                                UpdateSaveState(saveState);
                                FileLeftToDo--;
                                // Progression = (float)(files.Count - FileLeftToDo) / (float)files.Count * 100f;
                                // CalculFillProgress(Progression);


                            }


                        }
                        EndSaveWork(saveState);
                        //OnProgresseUpdate.Invoke(this, null);
                        //_view.DisplaySuccess(strings.Success);
                        break;
                    }
            }
        }

        private void CalculFillProgress(float progress) //Calcul the fill progress of the progress bar
    {
        string stringReturn = "[";
        float nbDiez = progress / 2.5f;
        for(int i = 0; i < (int)nbDiez; i++)
        {
            stringReturn += '#';
        }
        for(int i = (int) nbDiez; i < 40; i++)
        {
            stringReturn += '~';
        }
        stringReturn += ']';
        //OnProgressUpdate?.Invoke(this, stringReturn);
    }
    private void EndSaveWork(SaveState saveState) //Update SaveState to END
    {
        saveState.SetSourceFilePath("");
        saveState.SetTargetFilePath("");
        saveState.SetTotalFileSize(0);
        saveState.SetState("END");
        saveState.SetTotalFilesToCopy(0);
        saveState.SetTotalFilesLeftToDo(0);
        UpdateSaveState(saveState);
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

    public void ExecAllSaveWork() //Execute all savework
    {
        if (_model.GetSaveWorkList().Count > 0)
            foreach (var sw in _model.GetSaveWorkList())
                ExecSaveWork(sw.Name);
        else
            NotifyInfo(Resources.Info_No_Backup);
    }

    public void RenameSaveWork(string name, string rename) //Rename a Savework
    {
        var sv = _model.FindbyName(name);
        if (sv != null)
        {
            var sv2 = _model.FindbyName(rename);
            if (sv2 == null)
            {
                sv.Name = rename;
                NotifySuccess(Resources.Success);
            }
            else
                NotifyError(Resources.Error_Backup_Already_Exists);
        }
        else
        {
            NotifyError(Resources.Error_Backup_Not_Found);
        }
    }

    public void DisplayAllSaveWork() //Display all savework
    {
        if (_model.GetSaveWorkList().Count != 0 || _model.GetSaveWorkList == null)
        {
            foreach (var sv in _model.GetSaveWorkList()) //loop through each Savework
            {
                NotifyInfo($"{Resources.Name}: {sv.Name}");
                NotifyInfo($"{Resources.Source_Path}: {sv.SourcePath}");
                NotifyInfo($"{Resources.Target_Path}: {sv.TargetPath}");
                switch (sv.SaveType)
                {
                    case SaveType.Complete:
                        {
                            NotifyInfo(Resources.Type_Complete);
                            break;
                        }
                    case SaveType.Differential:
                        {
                            NotifyInfo(Resources.Type_Differential);
                            break;
                        }
                }
            }
        }
        else
        {
            NotifyError(Resources.Error_NoSaveWork);
        }
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

    public void Todo()
    {
        SnackBarMessageQueue.Enqueue("TODO");
    }

        public void ChangeFileFormat(FileFormat fileFormat)
        {
            _model.SetLogFileFormat(fileFormat);
            NotifySuccess(Resources.Success);
        }


        private string CheckIfWorkProcessIsOpen(List<string> listOfProcessToCheck)//Function for check if job Process is on
        {
            foreach (string ProcessToCheck in listOfProcessToCheck)
            {
                Process[] processes = Process.GetProcessesByName(ProcessToCheck);
                if (processes.Length > 0) return ProcessToCheck;
            }
            return string.Empty;
        }

        private int Cryptage(string sourcePath, string targetPath)
        {
            Process cryptosoft = new Process();
            cryptosoft.StartInfo.FileName = "Cryptosoft.exe";
            cryptosoft.StartInfo.Arguments = $"{sourcePath} {targetPath}";
            cryptosoft.StartInfo.UseShellExecute = true;
            cryptosoft.Start();
            cryptosoft.WaitForExit();
            return cryptosoft.ExitCode;
        }
    private bool CheckToCrypt(string file)
    {
        foreach (string ext in _model.GetListExtentionToCheck())
        {
            if (ext == Path.GetExtension(file))
            {
                return false;
            }
        }
        return true;
    }

    private string _filterText;

    public void Filter(string filterText)
    {
        _filterText = filterText;
        
        ICollectionView view = CollectionViewSource.GetDefaultView(Backups);
        view.Filter = item =>
        {
            if (item is SaveWork backup)
            {
                return backup.Name.ToLower().Contains(_filterText.ToLower());
            }
            
            return true;
        };
    }
}