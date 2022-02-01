﻿using Newtonsoft.Json;

namespace EasySave.model;

internal class Json : FileFormat
{
    public override void SaveInFormat<T>(string path, T obj) //Function that serialise an objet to json file
    {
        var list = new List<T>();
        if (File.Exists(path)) list = UnSerialize<T>(path);
        list.Add(obj);
        var s = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(path, s);
    }

    public override List<T> UnSerialize<T>(string path) //Function generic that Deserialise an json file by item
    {
        if (File.Exists(path)) return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));

        return null;
    }
}