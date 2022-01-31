using System.IO;
using System.Collections.Generic;
using System;

namespace EasySave
{
    internal class ViewModel
    {
        private View view;
        private Model model;
        private LanguageSystem lsInstance;

        public ViewModel(View v)
        {
            view = v;
            model = new Model();
            lsInstance = LanguageSystem.GetInstance();
        }

        public void CreateSaveWork(string name, string sourcePath, string targetPath, Type type)
        {
            if (model.GetSaveWorkList().Count < 5)
            {

                if (model.FindbyName(name) == null)
                {
                    model.GetSaveWorkList().Add(new SaveWork(name, sourcePath, targetPath, type));
                    view.AfficherText("Succesfull");
                }
                else
                {
                    view.AfficherText(model.GetErrorList()[2]);
                }

            } else
            {
                view.AfficherText(model.GetErrorList()[0]);
            }

        }

        private void UpdateSaveState(SaveState saveState)
        {
            List<SaveState> states = null;
            if (File.Exists(model.GetSaveStatePath()))
            {
                states = saveState.GetFileFormat().UnSerialize<SaveState>(model.GetSaveStatePath());
            }
            File.Delete(model.GetSaveStatePath());
            bool enter = false;
            if (states != null)
            {
                for(int i = 0; i < states.Count; i++)
                {
                    if (states[i].GetName() == saveState.GetName())
                    {
                        states[i] = saveState;
                        enter = true;
                    }
                }
                if(!enter)
                {
                    states.Add(saveState);
                }
                foreach(SaveState sv in states)
                {
                    sv.GetFileFormat().SaveInFormat<SaveState>(model.GetSaveStatePath(), sv);
                }
            }
            else
            {
                saveState.GetFileFormat().SaveInFormat(model.GetSaveStatePath(), saveState);
            }

        }

        public void DeleteSaveWork(string name)
        {
            SaveWork sv = model.FindbyName(name);
            if(sv != null)
            {
                model.GetSaveWorkList().Remove(sv);
            } 
            else
            {
                view.AfficherText(model.GetErrorList()[1]);
            }
        }

        public void TryRecupFromSaveStatePath()
        {

        }

        public void ExecSaveWork(string name)
        {
            
            SaveWork sv = model.FindbyName(name);
            if (Directory.Exists(sv.GetSourcePath()) || File.Exists(sv.GetSourcePath()))
            {
                if (!Directory.Exists(sv.GetTargetPath()))
                {
                    Directory.CreateDirectory(sv.GetTargetPath());
                }

                List<string> files = new List<string>();
                files.AddRange(Directory.EnumerateFiles(sv.GetSourcePath()));
                GetAllFileFromDirectory(Directory.GetDirectories(sv.GetSourcePath()), files);
                SaveState saveState = new SaveState(sv.GetName(), "ACTIVE", new JSON());
                switch (sv.Gettype())
                {
                    case Type.Complet:
                        {
                            saveState.SetTotalFilesToCopy(files.Count);
                            int FileLeftToDo = files.Count;
                            foreach (string file in files)
                            {
                                string filePath = file.Replace(sv.GetSourcePath() + Path.DirectorySeparatorChar, null);
                                string targetPath = Path.Combine(sv.GetTargetPath(), filePath);
                                saveState.SetSourceFilePath(file);
                                saveState.SetTargetFilePath(targetPath);

                                float time = DateTime.Now.Millisecond;
                                File.Copy(file, targetPath, true);
                                time = DateTime.Now.Millisecond - time;
                                FileLeftToDo--;
                                saveState.SetTotalFilesLeftToDo(FileLeftToDo);
                                Log log = new Log(sv.GetName(), file, targetPath, string.Empty, File.ReadAllBytes(file).Length, time, DateTime.Now.ToString(), new JSON());
                                log.getFileFormat().SaveInFormat(model.GetLogPath(), log);
                                UpdateSaveState(saveState);
                            }
                            EndSaveWork(saveState);
                            break;
                        }
                    case Type.Differential:
                        {
                            saveState.SetTotalFilesToCopy(files.Count);
                            int FileLeftToDo = files.Count;
                            foreach (string file in files)
                            {
                                string filePath = file.Replace(sv.GetSourcePath() + Path.DirectorySeparatorChar, null);
                                string targetPath = Path.Combine(sv.GetTargetPath(), filePath);
                                saveState.SetSourceFilePath(file);
                                saveState.SetTargetFilePath(targetPath);
                                if (File.Exists(targetPath))
                                {
                                    FileStream fs1 = new FileStream(file, FileMode.Open);
                                    FileStream fs2 = new FileStream(targetPath, FileMode.Open);

                                    bool same = true;
                                    if (fs1.Length == fs2.Length)
                                    {
                                        int fb1 = fs1.ReadByte();
                                        int fb2 = fs2.ReadByte();

                                        while(fb1 != -1 && same)
                                        {
                                            if(fb1 == fb2)
                                            {
                                                same = true;
                                            } else
                                            {
                                                same = false;
                                            }
                                            fb1 = fs1.ReadByte();
                                            fb2 = fs2.ReadByte();
                                        }

                                        // Close the file
                                        fs1.Close();
                                        fs2.Close();

                                        if(!same)
                                        {
                                            float time = DateTime.Now.Millisecond;
                                            File.Copy(file, targetPath, true);
                                            time = DateTime.Now.Millisecond - time;
                                            FileLeftToDo--;
                                            saveState.SetTotalFilesLeftToDo(FileLeftToDo);
                                            Log log = new Log(sv.GetName(), file, targetPath, string.Empty, File.ReadAllBytes(file).Length, time, DateTime.Now.ToString(), new JSON());
                                            log.getFileFormat().SaveInFormat(model.GetLogPath(), log);
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
            foreach(string directory in directories)
            {

                string[] list = Directory.GetDirectories(directory);
                if (list.Length > 0)
                {
                    GetAllFileFromDirectory(list, files);
                }

                files.AddRange(Directory.EnumerateFiles(directory));
            }
        }
        public void ExecAllSaveWork()
        {
            if (model.GetSaveWorkList().Count > 0)
            {
                foreach (SaveWork sw in model.GetSaveWorkList())
                {
                    ExecSaveWork(sw.GetName());
                }
            } else
            {
                view.AfficherText("no sw");
            }
        }

        public void RenameSaveWork(string name, string rename)
        {
            SaveWork sv = model.FindbyName(name);
            if (sv != null)
            {
                SaveWork sv2 = model.FindbyName(rename);
                if(sv2 == null)
                {
                    sv.SetName(rename);
                }
                else
                {
                    view.AfficherText(model.GetErrorList()[3]);
                }
                
            }
            else
            {
                view.AfficherText(model.GetErrorList()[1]);
            }
        }

        public void AfficherAllSaveWork()
        {
             foreach(SaveWork sv in model.GetSaveWorkList())
            {
                view.AfficherText("Name : "+ sv.GetName());
                view.AfficherText("Source Path : " + sv.GetSourcePath());
                view.AfficherText("Source Path : " + sv.GetTargetPath());
                switch(sv.Gettype())
                {
                    case Type.Complet:
                        {
                            view.AfficherText("Type : Complet");
                            break;
                        }
                    case Type.Differential:
                        {
                            view.AfficherText("Type : Differential");
                            break;
                        }
                }
            }
        }

        


    }
}
