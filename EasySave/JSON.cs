using System.IO;
//using System.Text.Json;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace EasySave
{

    internal class JSON : FileFormat
    {
        public override void SaveInFormat<T>(string path, T obj)
        {
            List<T> list = new List<T>();
            if (File.Exists(path))
            {
                list = UnSerialize<T>(path);
            }
            list.Add(obj);
            string s = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(path, s);
        }

        public override List<T> UnSerialize<T>(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
            } else
            {
                return null;
            }
        }
    }
}
