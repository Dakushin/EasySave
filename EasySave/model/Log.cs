using Newtonsoft.Json;

namespace EasySave.model;

public class Log
{
    //CONSTRUCTOR
    [JsonConstructor]
    public Log(string name, string fileSource, string fileTarget, string destPath, int fileSize, float fileTransferTime,
        string time, int timeToCrypt)
    {
        Name = name;
        FileSource = fileSource;
        FileTarget = fileTarget;
        DestPath = destPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
        Time = time;
        TimeToCrypt = timeToCrypt;
    }

    public Log()
    {
    }


    //public variable for the json serializer
    public string Name { get; }

    public string FileSource { get; set; }

    public string FileTarget { get; set; }

    public string DestPath { get; set; }

    public int FileSize { get; set; }

    public float FileTransferTime { get; set; }

    public string Time { get; set; }

    public int TimeToCrypt { get; set; }

    //GETTER AND SETTER
    public int GetFileSize()
    {
        return FileSize;
    }

    public void SetFileSize(int fileSize)
    {
        this.FileSize = fileSize;
    }

    public float GetFileTransferTime()
    {
        return FileTransferTime;
    }

    public void SetFileTransferTime(float fileTransferTime)
    {
        this.FileTransferTime = fileTransferTime;
    }

    public string GetTime()
    {
        return Time;
    }

    public void SetTime(string currentTime)
    {
        Time = currentTime;
    }
}