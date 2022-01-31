using System.Collections.Generic;
using System.IO;

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
            logPath = Path.GetFullPath(@"..\..\..\log.json");
            saveStatePath = Path.GetFullPath(@"..\..\..\state.json"); ;
            saveWorkList = new List<SaveWork>(5);
            errorMessage = new List<string>() { 
                "Error, you have already 5 savework, please delete one and retry",
                "Error, Does not find the name of the work you want to delete ",
                "Error, Savework with this name already exist",
                "Error, You can't rename you Savework with name you already given",
            };
            workInProgress = false;
        }
        public string GetLogPath()
        {
            return logPath;
        }

        public string GetSaveStatePath()
        {
            return saveStatePath;
        }
        public List<SaveWork> GetSaveWorkList()
        {
            return saveWorkList;
        }

        public List<string> GetErrorList()
        {
            return errorMessage;
        }

        public SaveWork FindbyName(string name)
        {
            foreach (SaveWork sv in saveWorkList)
            {
                if (sv.GetName() == name)
                {
                    return sv;
                }
            }
            return null;
        }

    }
}
