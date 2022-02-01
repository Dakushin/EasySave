namespace EasySave.model;

internal enum SaveType //Enum for the type
{
    Complete,
    Differential
}

internal class SaveWork
{
    //Private member data
    private readonly SaveType _saveType;
    private readonly string _sourcePath;
    private readonly string _targetPath;
    private string _name;

    //CONSTRUCTOR
    public SaveWork(string name, string sourcePath, string targetPath, SaveType saveType)
    {
        _name = name;
        _sourcePath = sourcePath;
        _targetPath = targetPath;
        _saveType = saveType;
    }

    //GETTER AND SETTER
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