
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
                model.GetSaveWorkList().Add(new SaveWork(name, sourcePath, targetPath, type));
            }
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

        public void ExecSaveWork(string name)
        {

        }

        public void ExecAllSaveWork()
        {
            foreach (SaveWork sw in model.saveWorklist)
            {
                ExecSaveWork(sw.getName());                
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
