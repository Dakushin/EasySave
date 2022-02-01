using Newtonsoft.Json;

namespace EasySave.model;

public class Log
{
    private FileFormat _fileFormat;

    //CONSTRUCTOR
    public Log(string saveName, string sourceFilePath, string targetFilePath, string destPath, int fileSize,
        float fileTransferTime, string currentTime, FileFormat fileFormat)
    {
        Name = saveName;
        FileSource = sourceFilePath;
        FileTarget = targetFilePath;
        this.destPath = destPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        time = currentTime;
        _fileFormat = fileFormat;
    }

    //JSON CONSTRUCTOR
    [JsonConstructor]
    public Log(string name, string fileSource, string fileTarget, string destPath, int fileSize, float fileTransferTime,
        string time)
    {
        Name = name;
        FileSource = fileSource;
        FileTarget = fileTarget;
        this.destPath = destPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        this.time = time;
    }

    //public variable for the json serializer
    public string Name { get; }

    public string FileSource { get; }

    public string FileTarget { get; }

    public string destPath { get; }

    public int FileSize { get; private set; }

    public float FileTransferTime { get; private set; }

    public string time { get; private set; }

    //GETTER AND SETTER
    public int GetFileSize() 
    {
        return FileSize;
    }

    public void SetFileSize(int FileSize)
    {
        this.FileSize = FileSize;
    }

    public float GetFileTransferTime()
    {
        return FileTransferTime;
    }

    public void SetFileTransferTime(float FileTransferTime)
    {
        this.FileTransferTime = FileTransferTime;
    }

    public string GetTime()
    {
        return time;
    }

    public void SetTime(string currentTime)
    {
        time = currentTime;
    }

    public FileFormat GetFileFormat()
    {
        return _fileFormat;
    }

    public void SetFileFormat(FileFormat fileFormat)
    {
        _fileFormat = fileFormat;
    }
}