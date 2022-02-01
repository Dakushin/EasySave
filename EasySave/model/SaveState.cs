using Newtonsoft.Json;

namespace EasySave.model;

internal class SaveState
{
    //Private Variable 
    private readonly FileFormat _fileFormat;

    //JSON CONSTRUCTOR
    [JsonConstructor]
    public SaveState(string name, string sourceFilePath, string targetFilePath, string state, int nbFilesToCopy,
        int totalFilesSize, int nbFilesLeftToDo, int progression)
    {
        Name = name;
        SourceFilePath = sourceFilePath;
        TargetFilePath = targetFilePath;
        State = state;
        NbFilesToCopy = nbFilesToCopy;
        TotalFilesSize = totalFilesSize;
        NbFilesLeftToDo = nbFilesLeftToDo;
        Progression = progression;
    }

    //CONSTRUCTOR
    public SaveState(string name, string state, FileFormat fileFormat)
    {
        Name = name;
        SourceFilePath = null;
        TargetFilePath = null;
        State = state;
        NbFilesToCopy = 0;
        TotalFilesSize = 0;
        NbFilesLeftToDo = 0;
        Progression = 0;
        _fileFormat = fileFormat;
    }

    //public variables for the json serializer
    public string Name { get; set; }

    public string SourceFilePath { get; set; }

    public string TargetFilePath { get; set; }

    public string State { get; set; }

    public int NbFilesToCopy { get; set; }

    public int TotalFilesSize { get; set; }

    public int NbFilesLeftToDo { get; set; }

    public int Progression { get; set; }

    //GETTER AND SETTER
    public string GetName()
    {
        return Name;
    }

    public void SetSourceFilePath(string sourceFilePath)
    {
        SourceFilePath = sourceFilePath;
    }

    public string GetState()
    {
        return State;
    }

    public void SetState(string state)
    {
        State = state;
    }

    public int GetNbFilesToCopy()
    {
        return NbFilesToCopy;
    }

    public void SetTotalFilesToCopy(int totalFilesToCopy)
    {
        NbFilesToCopy = totalFilesToCopy;
    }

    public int GetTotalFilesSize()
    {
        return TotalFilesSize;
    }

    public void SetTotalFileSize(int totalFilesSize)
    {
        TotalFilesSize = totalFilesSize;
    }

    public int GetNbFilesLeftToDo()
    {
        return NbFilesLeftToDo;
    }

    public void SetTotalFilesLeftToDo(int nbFileLeftToDo)
    {
        NbFilesLeftToDo = nbFileLeftToDo;
    }

    public int GetProgression()
    {
        return Progression;
    }

    public void SetProgression(int progression)
    {
        Progression = progression;
    }

    public void SetTargetFilePath(string targetFilePath)
    {
        TargetFilePath = targetFilePath;
    }

    public FileFormat GetFileFormat()
    {
        return _fileFormat;
    }
}