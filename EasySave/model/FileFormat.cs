namespace EasySave.model;

public abstract class FileFormat
{
    //ABSTRACT FUNCTION
    public abstract void SaveInFormat<T>(string path, T obj);
    public abstract List<T> UnSerialize<T>(string path);
}