using System;
using System.Collections.Generic;

namespace EasySave
{
    internal class Log<T> where T : FileFormat
    {
        private SaveWork saveWork;
        private int fileSize;
        private T fileFormat;
        private float fileTransferTime;
        private string time;

        public Log(T fF, SaveWork sW, int fS, int fTT, string time)
        {
            
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

        public void setTime(string Time)
        {
            this.time = Time;

        }

        public T getFileFormat()
        {
            return fileFormat;
        }

        public void setFileFormat(T FileFormat)
        {
            this.fileFormat = FileFormat;
        
        }
    }
}
