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
            errorMessage = new List<string>() { "Error, you have already 5 savework, please delete one and retry",
                                                ""};
            workInProgress = false;
        }

        public List<SaveWork> GetSaveWorkList()
        {
            return saveWorkList;
        }

        public List<string> GetErrorList()
        {
            return errorMessage;
        }
    }
}
