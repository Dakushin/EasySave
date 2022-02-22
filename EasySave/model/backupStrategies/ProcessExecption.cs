using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model.backupStrategies
{
    internal class ProcessExecption : Exception
    {
        public BackupStrategy backupStrategy { get; set; }
        public string sourceFolderPath { get; set; }
        public string targetFolderPath { get; set; }
        public List<string> FileToCopy { get; set; }
        public int exception { get; set; }
        public string Name { get; set; }

        public ProcessExecption(string Name, BackupStrategy backupStrategy, string sourceFolderPath, string targetFolderPath, List<string> FileToCopy, int exception)
        {
            this.Name = Name;
            this.backupStrategy = backupStrategy;
            this.sourceFolderPath = sourceFolderPath;
            this.targetFolderPath = targetFolderPath;
            this.FileToCopy = FileToCopy;
            this.exception = exception;
        }
    }
}
