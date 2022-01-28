
namespace EasySave
{
    internal class SaveState
    {
        SaveWork SaveWork;
        private string State;
        private int TotalFilesToCopy;
        private int TotalFilesSize;
        private int NbFileLeftToDo;
        private int Progression;
        FileFormat fileFormat;

        public SaveState()
        {

        }

        public string GetState(){
            return State;
           
        }
        public void SetState(string State){
            this.State = State;
        }
        
        public int GetTotalFilesToCopy()
        {
            return TotalFilesToCopy;
        }
        public void SetTotalFilesToCopy(int TotalFilesToCopy)
        {
            this.TotalFilesToCopy = TotalFilesToCopy;
        }
        public int GetTotalFilesSize()
        {
            return TotalFilesSize;
        }
        public void SetTotalFileSize(int TotalFilesSize)
        {
            this.TotalFilesSize = TotalFilesSize;
        }
        public int GetNbFilesLeftToDo()
        {
            return NbFileLeftToDo;
        }
        public void SetTotalFilesLeftToDo(int nbFileLeftToDo)
        {
            this.NbFileLeftToDo= nbFileLeftToDo;
        }
        
        public int GetProgression() 
        {
            return Progression;
        }
        public void SetProgression(int Progression)
        {
            this.Progression=Progression;
        }



    }
}


