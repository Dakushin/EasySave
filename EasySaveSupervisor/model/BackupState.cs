using Newtonsoft.Json;

namespace EasySaveSupervisor.model;

public class BackupState
{
    //JSON CONSTRUCTOR
    [JsonConstructor]
    public BackupState(string name, string state, string sourceFilePath = "", string targetFilePath = "",
        int nbFilesToCopy = 0,
        long totalFilesSize = 0, int nbFilesLeftToDo = 0, int progression = 0)
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

    //public variables for the json serializer
    public string Name { get; set; }

    public string SourceFilePath { get; set; }

    public string TargetFilePath { get; set; }

    public string State { get; set; }

    public int NbFilesToCopy { get; set; }

    public long TotalFilesSize { get; set; }

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

    public long GetTotalFilesSize()
    {
        return TotalFilesSize;
    }

    public void SetTotalFileSize(long totalFilesSize)
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
}