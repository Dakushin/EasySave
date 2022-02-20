using System.IO;

namespace EasySave.model.backupStrategies;

public class Complete : BackupStrategy
{
    protected override void ExecuteInternally(string sourceFolderPath, string targetFolderPath)
    {
        TotalBytesToCopy = GetDirectorySize(sourceFolderPath);

        foreach (var sourceFilePath in Directory.GetFiles(sourceFolderPath))
        {
            CopyFile(sourceFilePath, Path.Combine(targetFolderPath, Path.GetFileName(sourceFilePath)));
        }
    }

    public override string GetName()
    {
        return "Complete";
    }
}