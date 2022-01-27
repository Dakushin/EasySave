
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
<<<<<<< HEAD
                if (model.FindbyName(name) == null)
                {
                    model.GetSaveWorkList().Add(new SaveWork(name, sourcePath, targetPath, type));
                    view.AfficherText("Succesfull");
                }
                else
                {
                    view.AfficherText(model.GetErrorList()[2]);
                }
=======
                model.GetSaveWorkList().Add(new SaveWork(name, sourcePath, targetPath, type));
>>>>>>> origin/main
            } else
            {
                view.AfficherText(model.GetErrorList()[0]);
            }
<<<<<<< HEAD
            
=======
>>>>>>> origin/main
        }

        public void UpdateSaveState(SaveState saveState)
        {

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

        public void ExecAllSaveWork()
        {

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
