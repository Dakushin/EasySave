using System.Globalization;
using EasySave.model;
using EasySave.translation;
using EasySave.view;

namespace EasySave.viewmodel;

internal class ViewModel
{
    private readonly Model _model;
    private readonly View _view;

    public ViewModel(View v)
    {
        _view = v;
        _model = new Model();
    }

    public void CreateSaveWork(string name, string sourcePath, string targetPath, SaveType saveType)
    {
        if (_model.GetSaveWorkList().Count < 5)
        {
            if (_model.FindbyName(name) == null)
            {
                _model.GetSaveWorkList().Add(new SaveWork(name, sourcePath, targetPath, saveType));
                _view.DisplaySuccess(strings.Success);
            }
            else
            {
                _view.DisplayError(strings.Error_Backup_Already_Exists);
            }

            _model.GetSaveWorkList().Add(new SaveWork(name, sourcePath, targetPath, saveType));
        }
        else
        {
            _view.DisplayError(strings.Error_Too_Many_Backups);
        }
    }

    private void UpdateSaveState(SaveState saveState)
    {
        List<SaveState> states = null;
        if (File.Exists(_model.GetSaveStatePath()))
            states = saveState.GetFileFormat().UnSerialize<SaveState>(_model.GetSaveStatePath());
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
            foreach (var sv in states) sv.GetFileFormat().SaveInFormat(_model.GetSaveStatePath(), sv);
        }
        else
        {
            saveState.GetFileFormat().SaveInFormat(_model.GetSaveStatePath(), saveState);
        }
    }

    public void DeleteSaveWork(string name)
    {
        var sv = _model.FindbyName(name);
        if (sv != null)
            _model.GetSaveWorkList().Remove(sv);
        else
            _view.DisplayError(strings.Error_Backup_Not_Found);
    }

    public void TryRecupFromSaveStatePath()
    {
    }

    public void ExecSaveWork(string name)
    {
        var sv = _model.FindbyName(name);
        if (Directory.Exists(sv.GetSourcePath()) || File.Exists(sv.GetSourcePath()))
        {
            if (!Directory.Exists(sv.GetTargetPath())) Directory.CreateDirectory(sv.GetTargetPath());

            var files = new List<string>();
            files.AddRange(Directory.EnumerateFiles(sv.GetSourcePath()));
            GetAllFileFromDirectory(Directory.GetDirectories(sv.GetSourcePath()), files);
            var saveState = new SaveState(sv.GetName(), "ACTIVE", new Json());
            switch (sv.Gettype())
            {
                case SaveType.Complete:
                {
                    saveState.SetTotalFilesToCopy(files.Count);
                    var FileLeftToDo = files.Count;
                    foreach (var file in files)
                    {
                        var filePath = file.Replace(sv.GetSourcePath() + Path.DirectorySeparatorChar, null);
                        var targetPath = Path.Combine(sv.GetTargetPath(), filePath);
                        saveState.SetSourceFilePath(file);
                        saveState.SetTargetFilePath(targetPath);

                        float time = DateTime.Now.Millisecond;
                        File.Copy(file, targetPath, true);
                        time = DateTime.Now.Millisecond - time;
                        FileLeftToDo--;
                        saveState.SetTotalFilesLeftToDo(FileLeftToDo);
                        var log = new Log(sv.GetName(), file, targetPath, string.Empty, File.ReadAllBytes(file).Length,
                            time, DateTime.Now.ToString(), new Json());
                        log.GetFileFormat().SaveInFormat(_model.GetLogPath(), log);
                        UpdateSaveState(saveState);
                    }

                    EndSaveWork(saveState);
                    break;
                }
                case SaveType.Differential:
                {
                    saveState.SetTotalFilesToCopy(files.Count);
                    var FileLeftToDo = files.Count;
                    foreach (var file in files)
                    {
                        var filePath = file.Replace(sv.GetSourcePath() + Path.DirectorySeparatorChar, null);
                        var targetPath = Path.Combine(sv.GetTargetPath(), filePath);
                        saveState.SetSourceFilePath(file);
                        saveState.SetTargetFilePath(targetPath);
                        if (File.Exists(targetPath))
                        {
                            var fs1 = new FileStream(file, FileMode.Open);
                            var fs2 = new FileStream(targetPath, FileMode.Open);

                            var same = true;
                            if (fs1.Length == fs2.Length)
                            {
                                var fb1 = fs1.ReadByte();
                                var fb2 = fs2.ReadByte();

                                while (fb1 != -1 && same)
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

                                if (!same)
                                {
                                    float time = DateTime.Now.Millisecond;
                                    File.Copy(file, targetPath, true);
                                    time = DateTime.Now.Millisecond - time;
                                    FileLeftToDo--;
                                    saveState.SetTotalFilesLeftToDo(FileLeftToDo);
                                    var log = new Log(sv.GetName(), file, targetPath, string.Empty,
                                        File.ReadAllBytes(file).Length, time, DateTime.Now.ToString(), new Json());
                                    log.GetFileFormat().SaveInFormat(_model.GetLogPath(), log);
                                    UpdateSaveState(saveState);
                                }

                                EndSaveWork(saveState);
                            }
                        }
                    }

                    break;
                }
            }
        }
    }

    private void EndSaveWork(SaveState saveState)
    {
        saveState.SetSourceFilePath("");
        saveState.SetTargetFilePath("");
        saveState.SetTotalFileSize(0);
        saveState.SetState("END");
        saveState.SetTotalFilesToCopy(0);
        saveState.SetTotalFilesLeftToDo(0);
        UpdateSaveState(saveState);
    }

    private void GetAllFileFromDirectory(string[] directories, List<string> files)
    {
        foreach (var directory in directories)
        {
            var list = Directory.GetDirectories(directory);
            if (list.Length > 0) GetAllFileFromDirectory(list, files);

            files.AddRange(Directory.EnumerateFiles(directory));
        }
    }

    public void ExecAllSaveWork()
    {
        if (_model.GetSaveWorkList().Count > 0)
            foreach (var sw in _model.GetSaveWorkList())
                ExecSaveWork(sw.GetName());
        else
            _view.DisplayText(strings.Info_No_Backup);
    }

    public void RenameSaveWork(string name, string rename)
    {
        var sv = _model.FindbyName(name);
        if (sv != null)
        {
            var sv2 = _model.FindbyName(rename);
            if (sv2 == null)
                sv.SetName(rename);
            else
                _view.DisplayError(strings.Error_Backup_Already_Exists);
        }
        else
        {
            _view.DisplayError(strings.Error_Backup_Not_Found);
        }
    }

    public void AfficherAllSaveWork()
    {
        foreach (var sv in _model.GetSaveWorkList())
        {
            _view.DisplayText($"{strings.Name}: {sv.GetName()}");
            _view.DisplayText($"{strings.Source_Path}: {sv.GetSourcePath()}");
            _view.DisplayText($"{strings.Target_Path}: {sv.GetTargetPath()}");
            switch (sv.Gettype())
            {
                case SaveType.Complete:
                {
                    _view.DisplayText(strings.Type_Complete);
                    break;
                }
                case SaveType.Differential:
                {
                    _view.DisplayText(strings.Type_Differential);
                    break;
                }
            }
        }
    }

    public static void ChangeLanguage(Language language)
    {
        CultureInfo.CurrentUICulture = language switch
        {
            Language.English => CultureInfo.GetCultureInfo("en"),
            Language.French => CultureInfo.GetCultureInfo("fr"),
            _ => CultureInfo.CurrentUICulture
        };
    }
}