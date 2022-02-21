using System.IO;

namespace EasySave.model.backupStrategies;

public class Complete : BackupStrategy
{
    protected override void ExecuteInternally(string sourceFolderPath, string targetFolderPath)
    {
        TotalBytesToCopy = GetDirectorySize(sourceFolderPath);
        foreach (var sourceFilePath in GetAllFileFromDirectory(Directory.GetDirectories(sourceFolderPath)))
        {
            var file = sourceFilePath.Replace(sourceFolderPath + Path.DirectorySeparatorChar, null);
            var targetFilePath = Path.Combine(targetFolderPath, file);
            CopyFile(sourceFilePath, targetFilePath);
        }
        
        
    }

    public override string GetName()
    {
        return "Complete";
    }
}