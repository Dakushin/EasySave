using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasySave
{
    public class Log
    {
        private string saveName;
        
        private string sourceFilePAth;

        private string targetFilePAth;
        
        private string destpath;
        private int fileSize;
        private float fileTransferTime;
        private string currentTime;
        private FileFormat fileFormat;

        //public variable for the json serializer
        public string Name { get { return saveName; } }
        public string FileSource { get { return sourceFilePAth; } }
        public string FileTarget { get { return targetFilePAth; } }
        public string destPath { get { return destpath; } }
        public int FileSize { get { return fileSize; } }
        public float FileTransferTime { get { return fileTransferTime; } }
        public string time { get { return currentTime; } }

        public Log(string saveName, string sourceFilePath, string targetFilePath, string destPath, int fileSize, float fileTransferTime, string currenttime, FileFormat fileFormat)
        {
            this.saveName = saveName;
            this.sourceFilePAth = sourceFilePath;
            this.targetFilePAth = targetFilePath;
            this.destpath = destPath;
            this.fileSize = fileSize;
            this.fileTransferTime = fileTransferTime;
            this.currentTime = currenttime;
            this.fileFormat = fileFormat;
        }

        [JsonConstructor]
        public Log(string Name, string FileSource, string FileTarget, string destPath,int FileSize, float FileTransferTime, string time)
        {
            this.saveName = Name;
            this.sourceFilePAth = FileSource;
            this.targetFilePAth = FileTarget;
            this.destpath = destPath;
            this.fileSize = FileSize;
            this.fileTransferTime = FileTransferTime;
            this.currentTime = time;
        }

        public int getFileSize()
        {
            return fileSize;
        }

        public void setFileSize(int FileSize)
        {
            this.fileSize = FileSize;
        }

        public float getFileTransferTime()
        {
            return fileTransferTime;
        }

        public void setFileTransferTime(float FileTransferTime)
        {
            this.fileTransferTime = FileTransferTime;
        }

        public string getTime()
        {
            return time;
        }

        public void setTime(string currentTime)
        {

            this.currentTime = currentTime;

        }

        public FileFormat getFileFormat()
        {
            return fileFormat;
        }

        public void setFileFormat(FileFormat FileFormat)
        {
            this.fileFormat = FileFormat;
        
        }
    }
}
