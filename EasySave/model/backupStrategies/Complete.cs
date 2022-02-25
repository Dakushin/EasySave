namespace EasySave.model.backupStrategies;

public class Complete : BackupStrategy
{
    protected override void ExecuteInternally(string sourceFolderPath, string targetFolderPath) //Get all file
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