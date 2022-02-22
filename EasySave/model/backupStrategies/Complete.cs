using System.IO;

namespace EasySave.model.backupStrategies;

public class Complete : BackupStrategy
{
    protected override void ExecuteInternally(string sourceFolderPath, string targetFolderPath)
    {
        TotalBytesToCopy = GetDirectorySize(sourceFolderPath);
        var filesToCopy = GetAllFileFromDirectory(sourceFolderPath, true);
        DoAllCopy(filesToCopy, sourceFolderPath, targetFolderPath);
    }

    public override string GetName()
    {
        return "Complete";
    }
}