using System.Collections.Generic;

namespace EasySave
{
    internal class Model
    {
        private string logPath;
        private string saveStatePath;
        private List<SaveWork> saveWorkList;
        private List<string> errorMessage;
        private bool workInProgress;

        public Model()
        {
            logPath = "";
            saveStatePath = "";
            saveWorkList = new List<SaveWork>(5);
            errorMessage = new List<string>();
            workInProgress = false;
        }
    }
}
