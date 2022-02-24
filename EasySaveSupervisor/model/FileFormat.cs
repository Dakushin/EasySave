using System.IO;

namespace EasySaveSupervisor.model;

public abstract class FileFormat
{

    //ABSTRACT FUNCTION
    public abstract void SaveInFormat<T>(string path, T obj);
    public abstract List<T> UnSerialize<T>(string path);

    protected string Checkpath(string path, string extention)
    {
        if (Path.GetExtension(path) != extention)
        {
            if (Path.GetExtension(path) == string.Empty)
                path += extention;
            else
                path = path.Replace(Path.GetExtension(path), extention);
        }

        return path;
    }
}