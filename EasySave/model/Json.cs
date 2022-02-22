using System.IO;
using Newtonsoft.Json;

namespace EasySave.model;

internal class Json : FileFormat
{
    private string extention = ".json";
    public override void SaveInFormat<T>(string path, T obj) //Function that serialise an objet to json file
    {
        path = Checkpath(path, extention);
        var list = new List<T>();
        
        try
        {
            Lock.EnterWriteLock();

            if (File.Exists(path)) list = UnSerialize<T>(path);
            list.Add(obj);
            var s = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(path, s);
        }
        finally
        {
            Lock.ExitWriteLock();
        }
    }

    public override List<T> UnSerialize<T>(string path) //Function generic that Deserialise an json file by item
    {
        path = Checkpath(path, extention);
        if (File.Exists(path)) return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));

        return null;
    }
}