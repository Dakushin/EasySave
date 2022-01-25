using System;
using System.Collections.Generic;

namespace EasySave
{
    internal class Log<T> where T : FileFormat
    {
        private SaveWork saveWork;
        private int fileSave;
        private T fileFormat;
        private int fileTransferTime;
        private string time;

        public Log(T fF, SaveWork sW, int fS, int fTT, string time)
        {
            
        }
    }
}
