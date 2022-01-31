using Newtonsoft.Json;
using System;

namespace EasySave
{
    internal class SaveState
    {
        private string name;
        private string sourceFilePath;
        private string targetFilePath;
        private string state;
        private int totalFilesToCopy;
        private int totalFilesSize;
        private int nbFileLeftToDo;
        private int progression;
        private FileFormat fileFormat;

        //public variable for the json serializer
        public string Name { get { return name; } set { name = value; } }
        public string SourceFilePath { get { return sourceFilePath; } set { sourceFilePath = value; } }
        public string TargetFilePath { get { return targetFilePath; } set { targetFilePath = value; } }
        public string State { get { return state; } set { state = value; } }
        public int TotalFilesToCopy { get { return totalFilesToCopy; } set { totalFilesSize = value; } }
        public int TotalFilesSize { get { return totalFilesSize; } set { totalFilesSize = value; } }
        public int NbFilesLeftToDo { get { return nbFileLeftToDo; } set { nbFileLeftToDo = value; } }
        public int Progression { get { return progression; } set { progression = value; } }

        [JsonConstructor]
        public SaveState(string Name, string SourceFilePath, string TargetFilePath, string State, int TotalFilesToCopy, int TotalFilesSize, int NbFilesLeftToDo, int Progression)
        {
            this.name = Name;
            this.sourceFilePath = SourceFilePath;
            this.targetFilePath = TargetFilePath;
            this.state = State;
            this.totalFilesToCopy = TotalFilesToCopy;
            this.totalFilesSize = TotalFilesSize;
            this.nbFileLeftToDo = NbFilesLeftToDo;
            this.progression = Progression;
        }

        public SaveState(string name, string state, FileFormat fileFormat)
        {
            this.name = name;
            sourceFilePath = null;
            targetFilePath = null;
            this.state = state;
            totalFilesToCopy = 0;
            totalFilesSize = 0;
            nbFileLeftToDo = 0;
            progression = 0;
            this.fileFormat = fileFormat;
        }

        public string GetName()
        {
            return name;
        }
        public void SetSourceFilePath(string sourceFilePath)
        {
            this.sourceFilePath = sourceFilePath;
        }
        public string GetState()
        {
            return state;

        }
        public void SetState(string State)
        {
            this.state = State;
        }

        public int GetTotalFilesToCopy()
        {
            return TotalFilesToCopy;
        }
        public void SetTotalFilesToCopy(int TotalFilesToCopy)
        {
            this.totalFilesToCopy = TotalFilesToCopy;
        }
        public int GetTotalFilesSize()
        {
            return TotalFilesSize;
        }
        public void SetTotalFileSize(int TotalFilesSize)
        {
            this.totalFilesSize = TotalFilesSize;
        }
        public int GetNbFilesLeftToDo()
        {
            return nbFileLeftToDo;
        }
        public void SetTotalFilesLeftToDo(int nbFileLeftToDo)
        {
            this.nbFileLeftToDo = nbFileLeftToDo;
        }

        public int GetProgression()
        {
            return progression;
        }
        public void SetProgression(int Progression)
        {
            this.progression = Progression;
        }

        public void SetTargetFilePath(string targetFilePath)
        {
            this.targetFilePath = targetFilePath;
        }

        public FileFormat GetFileFormat()
        {
            return fileFormat;
        }
    }
}



