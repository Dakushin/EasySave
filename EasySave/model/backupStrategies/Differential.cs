using System.IO;

namespace EasySave.model.backupStrategies;

public class Differential : BackupStrategy
{
    protected override void ExecuteInternally(string sourceFolderPath, string targetFolderPath)
    {
        var filesToCopy = GetFilesToCopy(sourceFolderPath, targetFolderPath);
        
        foreach (var sourceFilePath in filesToCopy)
        {
            CopyFile(sourceFilePath, Path.Combine(targetFolderPath, Path.GetFileName(sourceFilePath)));
        }
    }

    private List<string> GetFilesToCopy(string sourceFolderPath, string targetFolderPath)
    {
        var filesToCopy = new List<string>();
        
        foreach (var sourceFilePath in Directory.GetFiles(sourceFolderPath))
        {
            var targetFilePath = Path.Combine(targetFolderPath, Path.GetFileName(sourceFilePath));
            
            if (File.Exists(targetFilePath)) // the file have a backup => check if the backups is up to date 
            {
                var sourceFileLastWriteTime = File.GetLastWriteTime(sourceFilePath);
                var targetFileLastWriteTime = File.GetLastWriteTime(targetFilePath);

                if (sourceFileLastWriteTime > targetFileLastWriteTime)
                {
                    filesToCopy.Add(sourceFilePath);
                    TotalBytesToCopy += new FileInfo(sourceFilePath).Length;
                }
            }
            else // the file don't have a backup => copy it
            {
                filesToCopy.Add(sourceFilePath);
                TotalBytesToCopy += new FileInfo(sourceFilePath).Length;
            }
        }

        return filesToCopy;
    }

    public override string GetName()
    {
        return "Differential";
    }
}