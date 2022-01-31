namespace EasySave.model;

internal enum SaveType
{
    Complete,
    Differential
}

internal class SaveWork
{
    private readonly SaveType _saveType;
    private readonly string _sourcePath;
    private readonly string _targetPath;
    private string _name;

    public SaveWork(string name, string sourcePath, string targetPath, SaveType saveType)
    {
        _name = name;
        _sourcePath = sourcePath;
        _targetPath = targetPath;
        _saveType = saveType;
    }

    public string GetName()
    {
        return _name;
    }

    public void SetName(string n)
    {
        _name = n;
    }

    public string GetSourcePath()
    {
        return _sourcePath;
    }

    public string GetTargetPath()
    {
        return _targetPath;
    }

    public SaveType Gettype()
    {
        return _saveType;
    }
}