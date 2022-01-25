
namespace EasySave
{
    internal class ViewModel
    {
        private View view;
        private Model model;

        public ViewModel(View v)
        {
            view = v;
            model = new Model();
        }

        public void CreateSaveWork(string name, string sourcePath, string targetPath)
        {

        }

        public void UpdateSaveState(SaveState saveState)
        {

        }

        public void ChangeLanguage()
        {

        }

        public void DeleteSaveWork(string name)
        {
        }

        public void TryRecupFromSaveStatePath()
        {

        }

        public void ExecAllSaveWork()
        {

        }

        public void RenameSaveWork(string name)
        {

        }


    }
}
