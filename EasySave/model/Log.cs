using Newtonsoft.Json;

namespace EasySave.model;

public class Log
{

    //CONSTRUCTOR
    [JsonConstructor]
    public Log(string name, string fileSource, string fileTarget, string destPath, int fileSize, float fileTransferTime,
       string time, int timetocrypt)
    {
        Name = name;
        FileSource = fileSource;
        FileTarget = fileTarget;
        this.destPath = destPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        this.time = time;
        this.timetocrypt = timetocrypt;
    }



    //public variable for the json serializer
    public string Name { get; }

    public string FileSource { get; set; }

    public string FileTarget { get; set; }

    public string destPath { get; set; }

    public int FileSize { get; set; }

    public float FileTransferTime { get; set; }

    public string time { get; set; }

    public int timetocrypt { get; set; }
    public Log()
    { }

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

}