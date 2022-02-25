using System.IO;

namespace EasySave.model;

public abstract class FileFormat
{
    //ABSTRACT FUNCTION
    public abstract void SaveInFormat<T>(string path, T obj);
    public abstract List<T> UnSerialize<T>(string path);

    protected string Checkpath(string path, string extention) //function to check the path of the file format or add it it missing
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