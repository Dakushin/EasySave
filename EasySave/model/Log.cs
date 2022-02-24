using Newtonsoft.Json;

namespace EasySave.model;

public class Log
{
    //CONSTRUCTOR
    [JsonConstructor]
    public Log(string name, string fileSource, string fileTarget, string destPath, long fileSize, long fileTransferTime,
        string time, long timeToCrypt = 0)
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

    public long FileSize { get; set; }

    public long FileTransferTime { get; set; }

    public string Time { get; set; }

    public long TimeToCrypt { get; set; }

    //GETTER AND SETTER
    public long GetFileSize()
    {
        return FileSize;
    }

    public void SetFileSize(int fileSize)
    {
        FileSize = fileSize;
    }

    public float GetFileTransferTime()
    {
        return FileTransferTime;
    }

    public void SetFileTransferTime(long fileTransferTime)
    {
        FileTransferTime = fileTransferTime;
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